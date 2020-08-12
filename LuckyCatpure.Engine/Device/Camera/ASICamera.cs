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
                    Log.Error("Failed to ASIGetCameraProperty for camera_id {0}, ASI_ERROR_CODE {1}", i, asi_error_code);
                    break;
                }

                var cameraInfo = new CameraInfo
                {
                    DisplayName = pASICameraInfo.Name,
                    SDKType = CameraSDKType.ASI,
                    IsColorCamera = pASICameraInfo.IsColorCam == ASICameraDll2.ASI_BOOL.ASI_TRUE,
                    BayerPattern = GetBayerPattern(pASICameraInfo.BayerPattern),
                    SupportBins = pASICameraInfo.SupportedBins,
                    CanCool = pASICameraInfo.IsCoolerCam == ASICameraDll2.ASI_BOOL.ASI_TRUE,
                    HasMechanicalShutter = pASICameraInfo.MechanicalShutter == ASICameraDll2.ASI_BOOL.ASI_TRUE,
                    HasST4Port = pASICameraInfo.ST4Port == ASICameraDll2.ASI_BOOL.ASI_TRUE,
                    IsUSB3Host = pASICameraInfo.IsUSB3Host == ASICameraDll2.ASI_BOOL.ASI_TRUE,
                    IsUSB3Camera = pASICameraInfo.IsUSB3Camera == ASICameraDll2.ASI_BOOL.ASI_TRUE,
                    PixelSize = Convert.ToInt32(pASICameraInfo.PixelSize * 1000),
                    ElecPerADU = Convert.ToInt32(pASICameraInfo.ElecPerADU * 1000),
                    MaxHeight = pASICameraInfo.MaxHeight,
                    MaxWeight = pASICameraInfo.MaxWidth
                };

                var camera = new ASICamera(cameraID, cameraInfo);
                cameraList.Add(camera);

                Log.Info("Found ASI Camera: {0}", cameraInfo.DisplayName);
            }

            return new Result { Code = ErrorCode.OK, Message = String.Format("ASI Camera Found, Total Count: {0}", camera_count) };
        }

        private static BayerPattern GetBayerPattern(ASICameraDll2.ASI_BAYER_PATTERN bayerPattern)
        {
            switch (bayerPattern)
            {
                case ASICameraDll2.ASI_BAYER_PATTERN.ASI_BAYER_RG:
                    return BayerPattern.RGGB;
                case ASICameraDll2.ASI_BAYER_PATTERN.ASI_BAYER_BG:
                    return BayerPattern.BGGR;
                default:
                    return BayerPattern.Unknown;
            }
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
