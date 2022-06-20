using System;
using Microsoft.SPOT;
using GHIElectronics.NETMF.FEZ;
using Microsoft.SPOT.Hardware;
using System.Threading;
using GHIElectronics.NETMF.Hardware;
namespace MFConsoleApplication1
{
    public class Program
    {
        public static void Main()
        {
            OutputPort led = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.LED, true);
            var state = true;
            while (true)
            {
                state = !state;
                led.Write(state);

                Thread.Sleep(100);
            }

           

        }
    }

    public static class MotorCtrl
    {
        static const int DUTY_CYCLE = 20 * 1000;
        public static class MotorLeft
        {
            public static OutputPort Enable = new OutputPort((Cpu.Pin)FEZ_Pin.AnalogIn.An0, false);
            public static OutputPort Current = new OutputPort((Cpu.Pin)FEZ_Pin.AnalogIn.An2, false);
            public static OutputPort Clockwise = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di7, false);
            public static OutputPort CounterClockwise = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di8, false);
            public static PWM Speed = new PWM((PWM.Pin)FEZ_Pin.PWM.Di5);

            public static void Forward(byte speed) {
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
            static OutputPort Current = new OutputPort((Cpu.Pin)FEZ_Pin.AnalogIn.An3, false);
            public static OutputPort Clockwise = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di4, false);
            public static OutputPort CounterClockwise = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di9, false);
            public static PWM Speed = new PWM((PWM.Pin)FEZ_Pin.PWM.Di6);

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
}
