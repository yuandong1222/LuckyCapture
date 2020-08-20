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

        CameraControlItem[] GetControlItems();

        Result SetControItemValue(CameraControlItemType type, int value);

        Result StartCapture(int millisecond, bool isDark);

        Result GetCaputreStat(out CameraStatus captureStatus);

        Result GetCaputreData(UInt16[] data);
    }
}
