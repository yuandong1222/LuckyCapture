using LuckyCatpure.Engine.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Device.Camera
{
    public class ASICamera : ICamera
    {
        public CameraInfo Info { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ErrorCode Connect()
        {
            throw new NotImplementedException();
        }

        public ErrorCode GetCaputreData(ushort[] data)
        {
            throw new NotImplementedException();
        }

        public ErrorCode GetCaputreStat()
        {
            throw new NotImplementedException();
        }

        public ErrorCode StartCapture(int millisecond)
        {
            throw new NotImplementedException();
        }
    }
}
