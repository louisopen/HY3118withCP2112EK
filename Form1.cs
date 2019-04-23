using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SLAB_HID_TO_SMBUS;
using System.Threading;

namespace GetADCfromHY3118
{
    public struct BMP180CalibrationData
    {
        public short AC1;
        public short AC2;
        public short AC3;
        public ushort AC4;
        public ushort AC5;
        public ushort AC6;
        public short B1;
        public short B2;
        public short MB;
        public short MC;
        public short MD;
    }

    public partial class Form1 : Form
    {
        const ushort vid = 4292;  //10C4
        const ushort pid = 60048; //EA90
        IntPtr connectedDevice;
        //SlaveAssress 0x02–0xFE.
        const byte HY3118SlaveAddress = 0xA0;   //HY3118  address using bit1~bit7, bit0 is W/R

        public Form1()
        {
            InitializeComponent();
            uint numDevices = 0;
            SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_GetNumDevices(ref numDevices, vid, pid);

            //we only proceed if there is exactly one CP2112 device connected
            if (numDevices < 1)
            {
                MessageBox.Show("No devices Connected");
                this.Close();
            }

            if (numDevices > 1)
            {
                MessageBox.Show("More than one device connected. Abort");
                this.Close();
            }

            try
            {
                // aquire the device pointer so that we can reference it later.
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_Open(ref connectedDevice, 0, vid, pid);
                // initialize CP2112 device
                //HID_SMBUS_STATUS HidSmbus_SetSmbusConfig(HID_SMBUS_DEVICE device,DWORD bitRate, BYTE address, BOOL autoReadRespond, WORD writeTimeout,WORD readTimeout, BOOL sclLowTimeout, WORD transferRetries)
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_SetSmbusConfig(connectedDevice, 100000, 0x02, 0, 100, 100, 0, 2);
                //HID_SMBUS_STATUS HidSmbus_SetGpioConfig(HID_SMBUS_DEVICE device,BYTE direction, BYTE mode, BYTE special, BYTE clkDiv)
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_SetGpioConfig(connectedDevice, 0x20, 0x20, 0x13, 0xFF);   //GPIO5 output/push-pull/GPIO0,1,7 special function/clkDiv=48MHz/(2x255)
                //HID_SMBUS_STATUS HidSmbus_WriteLatch(HID_SMBUS_DEVICE device,BYTE latchValue, BYTE latchMask)
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteLatch(connectedDevice, 0, 0x20);     //"Low" active for GPIO5

                // initialize HY3118 config.
                //HID_SMBUS_STATUS HidSmbus_WriteRequest(HID_SMBUS_DEVICE device,BYTE slaveAddress, BYTE* buffer, BYTE numBytesToWrite)
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, HY3118SlaveAddress, new byte[] { 0x04, 0x6C }, 2);  //LDO using 3.0V, low speed transfer rate.
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, HY3118SlaveAddress, new byte[] { 0x03, 0x40 }, 2);  //clk = 1000KHz
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, HY3118SlaveAddress, new byte[] { 0x02, 0x50 }, 2);  //REF input select VDDA&VSSA
                //SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, HY3118SlaveAddress, new byte[] { 0x01, 0x1A }, 2);  //Using 0x1A for AN3,AN4 or 0x80 for AN1,AN2 
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, HY3118SlaveAddress, new byte[] { 0x01, 0x26 }, 2);  //Using 0x1A for AN3=REFO ,AN4=VSSA 
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, HY3118SlaveAddress, new byte[] { 0x00, 0x1C }, 2);  //ENADC, VDDA=3.0V, ENREFO=1.5V

                TickTimer.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Connecting fail");
                this.Close();
            }
        }
        
        private byte[] AddressRead(byte slaveAddress, byte[] req, ushort bytes2read)
        {
            byte status = 0;
            byte[] readbuff = new byte[61]; 
            byte[] valData = new byte[bytes2read];
            try
            {  
                //HID_SMBUS_STATUS HidSmbus_AddressReadRequest (HID_SMBUS_DEVICE device,BYTE slaveAddress, WORD numBytesToRead, BYTE targetAddressSize,BYTE targetAddress[16])
                /*
                1.device—is the device object pointer as returned by HidSmbus_Open().
                2.slaveAddress—is the address of the slave device to read from.This value must be between
                    0x02–0xFE.The least significant bit is the read / write bit for the SMBus transaction and must be 0.
                3.numBytesToRead—is the number of bytes to read from the device (1–512).
                4.targetAddressSize—is the size of the target address in bytes(1 - 16).
                5.targetAddress—is the address to read from the slave device.
                */
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_AddressReadRequest(connectedDevice, slaveAddress, bytes2read, 1, req);
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_ForceReadResponse(connectedDevice, bytes2read);

                for (byte totBytesRead = 0; totBytesRead < bytes2read;)
                {
                    byte bytesRead = 0;
                    //HID_SMBUS_STATUS HidSmbus_GetReadResponse(HID_SMBUS_DEVICE device, HID_SMBUS_S0* status, BYTE* buffer, BYTE bufferSize, BYTE* numBytesRead);
                    /*
                    1.device—is the device object pointer as returned by HidSmbus_Open().
                    2.status—returns the status of the read request.
                                    Definition Value Description
                                    HID_SMBUS_S0_IDLE 0x00 No transfers are currently active on the bus.
                                    HID_SMBUS_S0_BUSY 0x01 A read or write transfer is in progress.
                                    HID_SMBUS_S0_COMPLETE 0x02 A read or write transfer completed without error and without retry.
                                    HID_SMBUS_S0_ERROR 0x03 A read or write transfer completed with an error.
                    3.buffer—returns up to 61 read data bytes.
                    4.bufferSize—is the size of buffer and must be at least 61 bytes.
                    5.numBytesRead—returns the number of valid data bytes returned in buffer.
                    */
                    SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_GetReadResponse(connectedDevice, ref status, readbuff, 61, ref bytesRead);
                    for (int r = 0; r < bytesRead; r++)
                    {
                        int index = totBytesRead + r;
                        if ((0 <= index) && (index < bytes2read))
                            valData[index] = readbuff[r];
                    }
                    totBytesRead += bytesRead;
                }
            }
            catch
            {
            }
            return valData;
        }

        private bool isOpen()
        {
            int opened=0;
            SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_IsOpened(connectedDevice, ref opened);
            //HID_SMBUS_STATUS HidSmbus_IsOpened(HID_SMBUS_DEVICE device,BOOL* opened);
            //HID_SMBUS_SUCCESS 
            //HID_SMBUS_INVALID_DEVICE_OBJECT
            //HID_SMBUS_INVALID_PARAMETER
            return (opened == 1);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connectedDevice != null)
            {
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_Close(connectedDevice);
            }
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            if (isOpen())
            {
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteLatch(connectedDevice, 0x20, 0x20);  //Hi for GPIO5
                Thread.Sleep(500);
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteLatch(connectedDevice, 0, 0x20);     //"Low" active for GPIO5

                //SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, HY3118SlaveAddress, new byte[] { 0x03, 0x01 }, 2);
                //Thread.Sleep(100); //Wait for the conversion to take place

                //read the data
                byte[] readbuff = AddressRead(HY3118SlaveAddress, new byte[] { 0x05 }, 3);  //HY3118 regsister offset is 0x05 
                /*                                                                          //store the humidity into an int
                if ((readbuff[2] & 0x80) == 0x80)   //Sign(+-)
                {
                    readbuff[2] &= 0x80;
                }
                int adconverter = (readbuff[2] << 16) | (readbuff[1] << 8) | readbuff[0];
                */
                if ((readbuff[0] & 0x80) == 0x80)   //Sign(+-)
                {
                    readbuff[0] &= 0x80;
                }
                int adconverter = (readbuff[0] << 16) | (readbuff[1] << 8) | readbuff[2];
                //display
                label1.Text = adconverter.ToString();
            }
            else
            {
                MessageBox.Show("Cannot open it.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text=="Stop")
            {
                button1.Text = "Run";
                TickTimer.Enabled = false;
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, HY3118SlaveAddress, new byte[] { 0x00, 0x0C }, 2);  //ENADC, VDDA=3.0V, ENREFO=1.5V, ENADC is Off
            }
            else
            {
                button1.Text = "Stop";
                TickTimer.Enabled = true;
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, HY3118SlaveAddress, new byte[] { 0x00, 0x1C }, 2);  //ENADC, VDDA=3.0V, ENREFO=1.5V, ENADC is On
            }
        }
    }
}
