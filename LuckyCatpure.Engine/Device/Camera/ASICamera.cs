using System;
using System.Collections.Generic;
using System.Text;

using LuckyCatpure.Engine.Common;

using ZWOptical.ASISDK;

namespace LuckyCatpure.Engine.Device.Camera
{
    public class ASICamera : ICamera
    {
        public static ICamera[] ScanCameras()
        {
            List<ICamera> cameras = new List<ICamera>();

            int camera_count = ASICameraDll2.ASIGetNumOfConnectedCameras();
            if (camera_count == 0) return cameras.ToArray();

            for (int i = 0; i < camera_count; i++)
            {
                var camera = new ASICamera();
                camera.CameraID = i;

                //TOTO: Build CameraInfo

                cameras.Add(camera);
            }
            return cameras.ToArray();
        }

        private int CameraID;

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
