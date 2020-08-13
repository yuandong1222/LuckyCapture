using System;
using System.Collections.Generic;
using System.Text;

using LuckyCatpure.Engine.Common;

namespace LuckyCatpure.Engine.Device.Camera
{
    public interface ICamera
    {
        CameraInfo CameraInfo { get; set; }

        CameraStatus Status { get; }

        Result Connect();

        Result StartCapture(int millisecond, bool isDark);

        Result GetCaputreStat();

        Result GetCaputreData(UInt16[] data);
    }
}
