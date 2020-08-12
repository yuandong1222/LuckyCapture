using System;
using System.Collections.Generic;
using System.Text;

using LuckyCatpure.Engine.Common;

namespace LuckyCatpure.Engine.Device.Camera
{
    public interface ICamera
    {
        CameraInfo CameraInfo { get; set; }

        Result Connect();

        Result StartCapture(int millisecond);

        Result GetCaputreStat();

        Result GetCaputreData(UInt16[] data);
    }
}
