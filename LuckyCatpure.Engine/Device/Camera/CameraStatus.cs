using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Device.Camera
{
    public enum CameraStatus
    {
        Unknown = 0,
        Idle = 1, //Connected but not working
        Working = 2,
        Error = 3,
    }
}
