using System;
using System.Collections.Generic;
using System.Text;

using LuckyCatpure.Engine.Common;

using ZWOptical.ASISDK;

namespace LuckyCatpure.Engine.Device.Camera
{
    public class ASICamera : ICamera
    {
        public static Result ScanCameras(List<ICamera> cameraList)
        {
            int camera_count = 0;

            //TODO: Should try-catch for every SDK Call
            //TODO: Should Add log
            try
            {
                camera_count = ASICameraDll2.ASIGetNumOfConnectedCameras();
            }
            catch
            {

            }
            if (camera_count == 0)
                return new Result { Code = ErrorCode.OK, Message = "No ASI Camera Found" };

            for (int i = 0; i < camera_count; i++)
            {
                var camera = new ASICamera();
                camera.CameraID = i;

                //TODO: Build CameraInfo

                cameraList.Add(camera);
            }


            return new Result { Code = ErrorCode.OK, Message = String.Format("ASI Camera Found, Total Count: {0}", camera_count) };
        }

        private int CameraID;

        public CameraInfo Info { get; set; }

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
