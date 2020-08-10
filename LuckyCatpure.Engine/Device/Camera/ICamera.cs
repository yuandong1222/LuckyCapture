using System;
using System.Collections.Generic;
using System.Text;

using LuckyCatpure.Engine.Common;

namespace LuckyCatpure.Engine.Device.Camera
{
    public interface ICamera
    {
        /// <summary>
        /// For User to kown which camera is
        /// </summary>
        string CameraName { get; set; }

        ErrorCode Connect();

        ErrorCode StartCapture(int millisecond);

        ErrorCode GetCaputreStat();
    }
}
