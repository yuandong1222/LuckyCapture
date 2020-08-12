using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Device.Camera
{
    public class CameraInfo
    {
        public string DisplayName { get; set; }

        public CameraSDKType SDKType { get; set; }

        public bool CanCool { get; set; }        
        public int MaxHeight { get; set; }
        public int MaxWeight { get; set; }
    }
}
