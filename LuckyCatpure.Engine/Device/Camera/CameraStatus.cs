using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Device.Camera
{
    public enum CameraStatus
    {
        Unknown = 0,
        Connected = 1, //idle
        Working = 2,
        Error = 3,
    }
}
