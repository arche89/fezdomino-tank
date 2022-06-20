using System;
using Microsoft.SPOT;
using GHIElectronics.NETMF.FEZ;
using Microsoft.SPOT.Hardware;
using System.Threading;
using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.USBHost;

namespace MFConsoleApplication1
{
    public class Program
    {
        public static void Main()
        {
            USBHostController.DeviceConnectedEvent += USBHostController_DeviceConnectedEvent;

            while (true)
            {
                for (byte i = 0; i <= 100; i += 10)
                {
                    MotorCtrl.MotorLeft.Forward(i);
                    MotorCtrl.MotorRight.Forward(i);
                    Thread.Sleep(4000);
                }
            }

            Thread.Sleep(Timeout.Infinite);
        }

        static void USBHostController_DeviceConnectedEvent(USBH_Device device)
        {

            Debug.Print("Device connected: " + device.TYPE);

            if (device.TYPE == USBH_DeviceType.Joystick)
                UsbDevices.JoystickHandler.Init(device);

            if (device.TYPE == USBH_DeviceType.Mouse)
                UsbDevices.MouseHandler.Init(device);
        }
    }

    public static class MotorCtrl
    {
        const int DUTY_CYCLE = 20 * 1000;
        public static class MotorLeft
        {
            static OutputPort Enable = new OutputPort((Cpu.Pin)FEZ_Pin.AnalogIn.An0, false);
            static OutputPort Clockwise = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di7, false);
            static OutputPort CounterClockwise = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di8, false);
            static PWM Speed = new PWM((PWM.Pin)FEZ_Pin.PWM.Di5);

            public static void Forward(byte speed)
            {
                Enable.Write(true);
                Clockwise.Write(false);
                CounterClockwise.Write(true);
                Speed.Set(DUTY_CYCLE, speed);
            }

            public static void Backward(byte speed)
            {
                Enable.Write(true);
                Clockwise.Write(true);
                CounterClockwise.Write(false);
                Speed.Set(DUTY_CYCLE, speed);
            }
        }

        public static class MotorRight
        {
            static OutputPort Enable = new OutputPort((Cpu.Pin)FEZ_Pin.AnalogIn.An1, false);
            static OutputPort Clockwise = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di4, false);
            static OutputPort CounterClockwise = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di9, false);
            static PWM Speed = new PWM((PWM.Pin)FEZ_Pin.PWM.Di6);

            public static void Backward(byte speed)
            {
                Enable.Write(true);
                Clockwise.Write(false);
                CounterClockwise.Write(true);
                Speed.Set(DUTY_CYCLE, speed);
            }

            public static void Forward(byte speed)
            {
                Enable.Write(true);
                Clockwise.Write(true);
                CounterClockwise.Write(false);
                Speed.Set(DUTY_CYCLE, speed);
            }
        }
    }

    public static class UsbDevices
    {
        public static class JoystickHandler
        {
            static USBH_Joystick joystick;
            public static void Init(USBH_Device device)
            {
                joystick = new USBH_Joystick(device);
                joystick.JoystickXYMove += Joystick_JoystickXYMove;
            }

            static void Joystick_JoystickXYMove(USBH_Joystick sender, USBH_JoystickEventArgs args)
            {
                Debug.Print(sender.Cursor.X.ToString() + ", " + sender.Cursor.Y.ToString());
            }
        }

        public static class MouseHandler
        {
            static USBH_Mouse mouse;

            public static void Init(USBH_Device device)
            {
                mouse = new USBH_Mouse(device);
                mouse.SetCursorBounds(-100, 100, -100, 100);
                mouse.SetCursor(0, 0);
                mouse.MouseMove += mouse_MouseMove;

            }

            static void mouse_MouseMove(USBH_Mouse sender, USBH_MouseEventArgs args)
            {
                Debug.Print(sender.Cursor.X + ", " + sender.Cursor.Y);

            }
        }
    }
}
