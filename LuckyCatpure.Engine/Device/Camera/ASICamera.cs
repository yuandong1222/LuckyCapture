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
            ASICameraDll2.ASI_ERROR_CODE asi_error_code = ASICameraDll2.ASI_ERROR_CODE.ASI_SUCCESS;

            try
            {
                camera_count = ASICameraDll2.ASIGetNumOfConnectedCameras();
            }
            catch (Exception e)
            {
                Log.ErrorException("Failed to ASIGetNumOfConnectedCameras.", e);
                return new Result { Code = ErrorCode.Error, Message = "Failed to ASIGetNumOfConnectedCameras.", Exception = e };
            }

            if (camera_count == 0)
                return new Result { Code = ErrorCode.OK, Message = "No ASI Camera Found" };

            for (int i = 0; i < camera_count; i++)
            {
                var cameraID = i;
                var pASICameraInfo = new ASICameraDll2.ASI_CAMERA_INFO();

                try
                {
                    asi_error_code = ASICameraDll2.ASIGetCameraProperty(out pASICameraInfo, i);
                }
                catch (Exception e)
                {
                    Log.ErrorException(string.Format("Failed to ASIGetCameraProperty for camera_id {0}, ASI_ERROR_CODE {1}", i, asi_error_code), e);
                    break;
                }
                if (asi_error_code != ASICameraDll2.ASI_ERROR_CODE.ASI_SUCCESS)
                {
                    Log.Error(string.Format("Failed to ASIGetCameraProperty for camera_id {0}, ASI_ERROR_CODE {1}", i, asi_error_code));
                    break;
                }

                //TODO: There are many other properties
                var cameraInfo = new CameraInfo();
                cameraInfo.SDKType = CameraSDKType.ASI;
                cameraInfo.DisplayName = pASICameraInfo.Name;
                cameraInfo.CanCool = pASICameraInfo.IsCoolerCam == ASICameraDll2.ASI_BOOL.ASI_TRUE;
                cameraInfo.MaxHeight = pASICameraInfo.MaxHeight;
                cameraInfo.MaxWeight = pASICameraInfo.MaxWidth;

                var camera = new ASICamera(cameraID, cameraInfo);
                cameraList.Add(camera);
            }

            return new Result { Code = ErrorCode.OK, Message = String.Format("ASI Camera Found, Total Count: {0}", camera_count) };
        }

        private int _CameraID;

        public int CameraID { get { return _CameraID; } }
        public CameraInfo CameraInfo { get; set; }

        public ASICamera(int cameraID, CameraInfo cameraInfo)
        {
            this._CameraID = cameraID;
            this.CameraInfo = cameraInfo;
        }

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
