using LuckyCatpure.Engine.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Device.Camera
{
    public class CameraInfo
    {
        public string DisplayName { get; set; }

        public CameraSDKType SDKType { get; set; }

        public bool IsColorCamera { get; set; }
        BayerPattern BayerPattern { get; set; }

        public int MaxBin { get; set; }

        public bool CanCool { get; set; }

        public bool HasMechanicalShutter { get; set; }
        public bool HasST4Port { get; set; }

        public bool IsUSB3Host { get; set; }
        public bool IsUSB3Camera { get; set; }

        /// <summary>
        /// Pixel Size in nm
        /// </summary>
        public int PixelSize { get; set; }
        /// <summary>
        /// 1500 mean 1.5e per ADU
        /// </summary>
        public int ElecPerADU { get; set; }

        public int MaxHeight { get; set; }
        public int MaxWeight { get; set; }
    }
}
