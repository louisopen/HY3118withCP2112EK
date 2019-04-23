/////////////////////////////////////////////////////////////////////////////
// SLABCP2112.cs
// For SLABHIDtoSMBus.dll version 1.3
// and Silicon Labs CP2112 HID to SMBus
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
// Namespaces
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

/////////////////////////////////////////////////////////////////////////////
// SLABHIDtoSMBus.dll Namespace
/////////////////////////////////////////////////////////////////////////////

namespace SLAB_HID_TO_SMBUS
{
    /////////////////////////////////////////////////////////////////////////////
    // SLABHIDtoSMBus.dll Imports
    /////////////////////////////////////////////////////////////////////////////

    public class CP2112_DLL
    {
        /////////////////////////////////////////////////////////////////////////////
        // Return Code Definitions
        /////////////////////////////////////////////////////////////////////////////

        #region Return Code Definitions

        // HID_SMBUS_STATUS Return Codes
        public const byte HID_SMBUS_SUCCESS = 0x00;
        public const byte HID_SMBUS_DEVICE_NOT_FOUND = 0x01;
        public const byte HID_SMBUS_INVALID_HANDLE = 0x02;
        public const byte HID_SMBUS_INVALID_DEVICE_OBJECT = 0x03;
        public const byte HID_SMBUS_INVALID_PARAMETER = 0x04;
        public const byte HID_SMBUS_INVALID_REQUEST_LENGTH = 0x05;

        public const byte HID_SMBUS_READ_ERROR = 0x10;
        public const byte HID_SMBUS_WRITE_ERROR = 0x11;
        public const byte HID_SMBUS_READ_TIMED_OUT = 0x12;
        public const byte HID_SMBUS_WRITE_TIMED_OUT = 0x13;
        public const byte HID_SMBUS_DEVICE_IO_FAILED = 0x14;
        public const byte HID_SMBUS_DEVICE_ACCESS_ERROR = 0x15;
        public const byte HID_SMBUS_DEVICE_NOT_SUPPORTED = 0x16;

        public const byte HID_SMBUS_UNKNOWN_ERROR = 0xFF;

        public const byte HID_SMBUS_S0_IDLE = 0x00;
        public const byte HID_SMBUS_S0_BUSY = 0x01;
        public const byte HID_SMBUS_S0_COMPLETE = 0x02;
        public const byte HID_SMBUS_S0_ERROR = 0x03;

        // HID_SMBUS_TRANSFER_S0 = HID_SMBUS_S0_BUSY
        public const byte HID_SMBUS_S1_BUSY_ADDRESS_ACKED = 0x00;
        public const byte HID_SMBUS_S1_BUSY_ADDRESS_NACKED = 0x01;
        public const byte HID_SMBUS_S1_BUSY_READING = 0x02;
        public const byte HID_SMBUS_S1_BUSY_WRITING = 0x03;

        // HID_SMBUS_TRANSFER_S0 = HID_SMBUS_S0_ERROR
        public const byte HID_SMBUS_S1_ERROR_TIMEOUT_NACK = 0x00;
        public const byte HID_SMBUS_S1_ERROR_TIMEOUT_BUS_NOT_FREE = 0x01;
        public const byte HID_SMBUS_S1_ERROR_ARB_LOST = 0x02;
        public const byte HID_SMBUS_S1_ERROR_READ_INCOMPLETE = 0x03;
        public const byte HID_SMBUS_S1_ERROR_WRITE_INCOMPLETE = 0x04;
        public const byte HID_SMBUS_S1_ERROR_SUCCESS_AFTER_RETRY = 0x05;

        #endregion

        /////////////////////////////////////////////////////////////////////////////
        // String Definitions
        /////////////////////////////////////////////////////////////////////////////

        #region String Definitions

        // Product String Types
        public const uint HID_SMBUS_GET_VID_STR = 0x01;
        public const uint HID_SMBUS_GET_PID_STR = 0x02;
        public const uint HID_SMBUS_GET_PATH_STR = 0x03;
        public const uint HID_SMBUS_GET_SERIAL_STR = 0x04;
        public const uint HID_SMBUS_GET_MANUFACTURER_STR = 0x05;
        public const uint HID_SMBUS_GET_PRODUCT_STR = 0x06;

        // String Lengths
        public const uint HID_SMBUS_DEVICE_STRLEN = 260;

        #endregion

        /////////////////////////////////////////////////////////////////////////////
        // SMBUS Definitions
        /////////////////////////////////////////////////////////////////////////////

        #region SMBUS Definitions

        // SMbus Configuration Limits
        public const uint HID_SMBUS_MIN_BIT_RATE = 1;
        public const ushort HID_SMBUS_MIN_TIMEOUT = 0;
        public const ushort HID_SMBUS_MAX_TIMEOUT = 1000;
        public const ushort HID_SMBUS_MAX_RETRIES = 1000;
        public const byte HID_SMBUS_MIN_ADDRESS = 0x02;
        public const byte HID_SMBUS_MAX_ADDRESS = 0xFE;

        // Read/Write Limits
        public const ushort HID_SMBUS_MIN_READ_REQUEST_SIZE = 1;
        public const ushort HID_SMBUS_MAX_READ_REQUEST_SIZE = 512;
        public const byte HID_SMBUS_MIN_TARGET_ADDRESS_SIZE = 1;
        public const byte HID_SMBUS_MAX_TARGET_ADDRESS_SIZE = 16;
        public const byte HID_SMBUS_MAX_READ_RESPONSE_SIZE = 61;
        public const byte HID_SMBUS_MIN_WRITE_REQUEST_SIZE = 1;
        public const byte HID_SMBUS_MAX_WRITE_REQUEST_SIZE = 61;

        #endregion

        /////////////////////////////////////////////////////////////////////////////
        // GPIO Definitions
        /////////////////////////////////////////////////////////////////////////////

        #region GPIO Definitions

        // GPIO Pin Direction Bit Value
        public const byte HID_SMBUS_DIRECTION_INPUT = 0;
        public const byte HID_SMBUS_DIRECTION_OUTPUT = 1;

        // GPIO Pin Mode Bit Value
        public const byte HID_SMBUS_MODE_OPEN_DRAIN = 0;
        public const byte HID_SMBUS_MODE_PUSH_PULL = 1;

        // GPIO Function Bitmask
        public const byte HID_SMBUS_MASK_FUNCTION_GPIO_7_CLK = 0x01;
        public const byte HID_SMBUS_MASK_FUNCTION_GPIO_0_TXT = 0x02;
        public const byte HID_SMBUS_MASK_FUNCTION_GPIO_1_RXT = 0x04;

        // GPIO Function Bit Value
        public const byte HID_SMBUS_GPIO_FUNCTION = 0;
        public const byte HID_SMBUS_SPECIAL_FUNCTION = 1;

        // GPIO Pin Bitmask
        public const byte HID_SMBUS_MASK_GPIO_0 = 0x01;
        public const byte HID_SMBUS_MASK_GPIO_1 = 0x02;
        public const byte HID_SMBUS_MASK_GPIO_2 = 0x04;
        public const byte HID_SMBUS_MASK_GPIO_3 = 0x08;
        public const byte HID_SMBUS_MASK_GPIO_4 = 0x10;
        public const byte HID_SMBUS_MASK_GPIO_5 = 0x20;
        public const byte HID_SMBUS_MASK_GPIO_6 = 0x40;
        public const byte HID_SMBUS_MASK_GPIO_7 = 0x80;

        #endregion

        /////////////////////////////////////////////////////////////////////////////
        // Part Number Definitions
        /////////////////////////////////////////////////////////////////////////////

        #region Part Number Definitions

        // Part Numbers
        public const byte HID_SMBUS_PART_CP2112 = 0x0C;

        #endregion

        /////////////////////////////////////////////////////////////////////////////
        // User Customization Definitions
        /////////////////////////////////////////////////////////////////////////////

        #region User Customization Definitions

        // User-Customizable Field Lock Bitmasks
        public const byte HID_SMBUS_LOCK_VID = 0x01;
        public const byte HID_SMBUS_LOCK_PID = 0x02;
        public const byte HID_SMBUS_LOCK_POWER = 0x04;
        public const byte HID_SMBUS_LOCK_POWER_MODE = 0x08;
        public const byte HID_SMBUS_LOCK_RELEASE_VERSION = 0x10;
        public const byte HID_SMBUS_LOCK_MFG_STR = 0x20;
        public const byte HID_SMBUS_LOCK_PRODUCT_STR = 0x40;
        public const byte HID_SMBUS_LOCK_SERIAL_STR = 0x80;

        // Field Lock Bit Values
        public const byte HID_SMBUS_LOCK_UNLOCKED = 1;
        public const byte HID_SMBUS_LOCK_LOCKED = 0;

        // Power Max Value (500 mA)
        public const byte HID_SMBUS_BUS_POWER_MAX = 0xFA;

        // Power Modes
        public const byte HID_SMBUS_BUS_POWER = 0x00;
        public const byte HID_SMBUS_SELF_POWER_VREG_DIS = 0x01;
        public const byte HID_SMBUS_SELF_POWER_VREG_EN = 0x02;

        // USB Config Bitmasks
        public const byte HID_SMBUS_SET_VID = 0x01;
        public const byte HID_SMBUS_SET_PID = 0x02;
        public const byte HID_SMBUS_SET_POWER = 0x04;
        public const byte HID_SMBUS_SET_POWER_MODE = 0x08;
        public const byte HID_SMBUS_SET_RELEASE_VERSION = 0x10;

        // USB Config Bit Values
        public const byte HID_SMBUS_SET_IGNORE = 0;
        public const byte HID_SMBUS_SET_PROGRAM = 1;

        // String Lengths
        public const byte HID_SMBUS_CP2112_MFG_STRLEN = 30;
        public const byte HID_SMBUS_CP2112_PRODUCT_STRLEN = 30;
        public const byte HID_SMBUS_CP2112_SERIAL_STRLEN = 30;

        #endregion

        /////////////////////////////////////////////////////////////////////////////
        // Exported Library Functions
        /////////////////////////////////////////////////////////////////////////////

        #region Exported Library Functions

        // HidSmbus_GetNumDevices
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetNumDevices(ref uint numDevices, ushort vid, ushort pid);

        // HidSmbus_GetString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetString(uint deviceNum, ushort vid, ushort pid, StringBuilder deviceString, uint options);

        // HidSmbus_GetOpenedString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetOpenedString(IntPtr device, StringBuilder deviceString, uint options);

        // HidSmbus_GetIndexedString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetIndexedString(uint deviceNum, ushort vid, ushort pid, uint stringIndex, StringBuilder deviceString);

        // HidSmbus_GetOpenedIndexedString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetOpenedIndexedString(IntPtr device, uint stringIndex, StringBuilder deviceString);

        // HidSmbus_GetAttributes
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetAttributes(uint deviceNum, ushort vid, ushort pid, ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber);

        // HidSmbus_GetOpenedAttributes
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetOpenedAttributes(IntPtr device, ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber);

        // HidSmbus_Open
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_Open(ref IntPtr device, uint deviceNum, ushort vid, ushort pid);

        // HidSmbus_Close
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_Close(IntPtr device);

        // HidSmbus_IsOpened
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_IsOpened(IntPtr device, ref int opened);

        // HidSmbus_ReadRequest
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_ReadRequest(IntPtr device, byte slaveAddress, ushort numBytesToRead);

        // HidSmbus_AddressReadRequest
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_AddressReadRequest(IntPtr device, byte slaveAddress, ushort numBytesToRead, byte targetAddressSize, byte[] targetAddress);

        // HidSmbus_ForceReadResponse
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_ForceReadResponse(IntPtr device, ushort numBytesToRead);

        // HidSmbus_ForceReadResponse
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetReadResponse(IntPtr device, ref byte status, byte[] buffer, byte bufferSize, ref byte numBytesRead);

        // HidSmbus_WriteRequest
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_WriteRequest(IntPtr device, byte slaveAddress, byte[] buffer, byte numBytesToWrite);

        // HidSmbus_TransferStatusRequest
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_TransferStatusRequest(IntPtr device);

        // HidSmbus_GetTransferStatusResponse
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetTransferStatusResponse(IntPtr device, ref byte status, ref byte detailedStatus, ref ushort numRetries, ref ushort bytesRead);

        // HidSmbus_CancelTransfer
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_CancelTransfer(IntPtr device);

        // HidSmbus_CancelIo
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_CancelIo(IntPtr device);

        // HidSmbus_SetTimeouts
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_SetTimeouts(IntPtr device, uint responseTimeout);

        // HidSmbus_GetTimeouts
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetTimeouts(IntPtr device, ref uint responseTimeout);

        // HidSmbus_SetSmbusConfig
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_SetSmbusConfig(IntPtr device, uint bitRate, byte address, int autoReadRespond, ushort writeTimeout, ushort readTimeout, int sclLowTimeout, ushort transferRetries);

        // HidSmbus_GetSmbusConfig
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetSmbusConfig(IntPtr device, ref uint bitRate, ref byte address, ref int autoReadRespond, ref ushort writeTimeout, ref ushort readTimeout, ref int sclLowtimeout, ref ushort transferRetries);

        // HidSmbus_Reset
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_Reset(IntPtr device);

        // HidSmbus_SetGpioConfig
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_SetGpioConfig(IntPtr device, byte direction, byte mode, byte function, byte clkDiv);

        // HidSmbus_GetGpioConfig
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetGpioConfig(IntPtr device, ref byte direction, ref byte mode, ref byte function, ref byte clkDiv);

        // HidSmbus_ReadLatch
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_ReadLatch(IntPtr device, ref byte latchValue);

        // HidSmbus_WriteLatch
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_WriteLatch(IntPtr device, byte latchValue, byte latchMask);

        // HidSmbus_GetPartNumber
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetPartNumber(IntPtr device, ref byte partNumber, ref byte version);

        // HidSmbus_GetLibraryVersion
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetLibraryVersion(ref byte major, ref byte minor, ref int release);

        // HidSmbus_GetHidLibraryVersion
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetHidLibraryVersion(ref byte major, ref byte minor, ref int release);

        // HidSmbus_GetHidGuid
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetHidGuid(ref Guid guid);

        #endregion

        /////////////////////////////////////////////////////////////////////////////
        // Exported Library Functions - Device Customization
        /////////////////////////////////////////////////////////////////////////////

        #region Exported Library Functions - Device Customization

        // HidSmbus_SetLock
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_SetLock(IntPtr device, byte lockValue);

        // HidSmbus_GetLock
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetLock(IntPtr device, ref byte lockValue);

        // HidSmbus_SetUsbConfig
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_SetUsbConfig(IntPtr device, ushort vid, ushort pid, byte power, byte powerMode, ushort releaseVersion, byte mask);

        // HidSmbus_GetUsbConfig
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetUsbConfig(IntPtr device, ref ushort vid, ref ushort pid, ref byte power, ref byte powerMode, ref ushort releaseVersion);

        // HidSmbus_SetManufacturingString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_SetManufacturingString(IntPtr device, byte[] manufacturingString, byte strlen);

        // HidSmbus_GetManufacturingString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetManufacturingString(IntPtr device, StringBuilder manufacturingString, ref byte strlen);

        // HidSmbus_SetProductString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_SetProductString(IntPtr device, byte[] productString, byte strlen);

        // HidSmbus_GetProductString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetProductString(IntPtr device, StringBuilder productString, ref byte strlen);

        // HidSmbus_SetSerialString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_SetSerialString(IntPtr device, byte[] serialString, byte strlen);

        // HidSmbus_GetSerialString
        [DllImport("SLABHIDtoSMBus.dll")]
        public static extern int HidSmbus_GetSerialString(IntPtr device, StringBuilder serialString, ref byte strlen);

        #endregion
    }
}
