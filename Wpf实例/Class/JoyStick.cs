using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;
using System.Threading;
using System.Windows;

namespace VideoPlayer
{
    /// <summary>
    /// .net封装的DirectInput进行读取usb设备信息
    /// </summary>
    class JoyStick
    {
        SharpDX.DirectInput.Joystick curJoystick;
        //尝试使用.net封装的DirectInput进行读取usb设备信息,未完善
        private void UseDirectInput()
        {
            var dirInput = new SharpDX.DirectInput.DirectInput();
            var typeJoystick = SharpDX.DirectInput.DeviceType.Joystick;
            var allDevices = dirInput.GetDevices();
            bool isGetJoystick = false;
            foreach (var item in allDevices)
            {
                if (typeJoystick == item.Type)
                {
                    curJoystick = new SharpDX.DirectInput.Joystick(dirInput, item.InstanceGuid);
                    curJoystick.Acquire();
                    isGetJoystick = true;
                    Thread t1 = new Thread(joyListening);
                    t1.IsBackground = true;
                    t1.Start();
                }
            }
            if (!isGetJoystick)
            {
                MessageBox.Show("没有插入手柄");
            }
        }
        private void joyListening()
        {
            MessageBox.Show("监听到手柄");
            while (true)
            {
                var joys = curJoystick.GetCurrentState();
                //(textBox1, joys.ToString());
                Thread.Sleep(100);
            }

        }
    }
}
