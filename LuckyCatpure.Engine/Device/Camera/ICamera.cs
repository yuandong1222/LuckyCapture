using System;
using System.Collections.Generic;
using System.Text;

using LuckyCatpure.Engine.Common;

namespace LuckyCatpure.Engine.Device.Camera
{
    public interface ICamera
    {
        CameraInfo CameraInfo { get; set; }

        ErrorCode Connect();

        ErrorCode StartCapture(int millisecond);

        ErrorCode GetCaputreStat();

        ErrorCode GetCaputreData(UInt16[] data);
    }
}
