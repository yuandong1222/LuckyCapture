using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Device.Camera
{
    public enum CameraCaptureStat
    {
        Unknown = -1,
        Idle = 0,
        Working = 1,
        Finished = 2,
        Failed = 3,
    }
}
