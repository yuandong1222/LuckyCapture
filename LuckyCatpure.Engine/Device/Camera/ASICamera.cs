using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using LuckyCatpure.Engine.Common;

using ZWOptical.ASISDK;
using static ZWOptical.ASISDK.ASICameraDll2;

namespace LuckyCatpure.Engine.Device.Camera
{
    public class ASICamera : ICamera
    {
        public static Result ScanASICameras(List<ICamera> cameraList)
        {
            int camera_count = 0;
            ASI_ERROR_CODE asi_error_code = ASI_ERROR_CODE.ASI_SUCCESS;

            try
            {
                camera_count = ASIGetNumOfConnectedCameras();
            }
            catch (Exception e)
            {
                var message = "Failed to ASIGetNumOfConnectedCameras.";
                Log.Error(message, e);
                return new Result(ErrorCode.OperationFailed, message, e);
            }

            if (camera_count == 0)
                return new Result("No ASI Camera Found");

            for (int i = 0; i < camera_count; i++)
            {
                var cameraID = i;
                var pASICameraInfo = new ASI_CAMERA_INFO();
                Exception exception = null;

                try
                {
                    asi_error_code = ASIGetCameraProperty(out pASICameraInfo, i);
                }
                catch (Exception e)
                {
                    exception = e;
                }
                if (exception != null || asi_error_code != ASI_ERROR_CODE.ASI_SUCCESS)
                {
                    string message = string.Format("Failed to ASIGetCameraProperty for CameraID {0}, ASI_ERROR_CODE {1}", i, asi_error_code.ToString());
                    Log.Error(message, exception);
                    break;
                }

                var cameraInfo = new CameraInfo
                {
                    DisplayName = pASICameraInfo.Name,
                    SDKType = CameraSDKType.ASI,
                    IsColorCamera = pASICameraInfo.IsColorCam == ASI_BOOL.ASI_TRUE,
                    BayerPattern = GetASIBayerPattern(pASICameraInfo.BayerPattern),
                    SupportBins = pASICameraInfo.SupportedBins,
                    CanCool = pASICameraInfo.IsCoolerCam == ASI_BOOL.ASI_TRUE,
                    HasMechanicalShutter = pASICameraInfo.MechanicalShutter == ASI_BOOL.ASI_TRUE,
                    HasST4Port = pASICameraInfo.ST4Port == ASI_BOOL.ASI_TRUE,
                    IsUSB3Host = pASICameraInfo.IsUSB3Host == ASI_BOOL.ASI_TRUE,
                    IsUSB3Camera = pASICameraInfo.IsUSB3Camera == ASI_BOOL.ASI_TRUE,
                    PixelSize = Convert.ToInt32(pASICameraInfo.PixelSize * 1000),
                    ElecPerADU = Convert.ToInt32(pASICameraInfo.ElecPerADU * 1000000),
                    MaxHeight = pASICameraInfo.MaxHeight,
                    MaxWeight = pASICameraInfo.MaxWidth
                };

                var camera = new ASICamera(cameraID, cameraInfo);
                cameraList.Add(camera);

                Log.InfoFormat("Found ASI Camera {0}, CameraID {1}", cameraInfo.DisplayName, cameraID);
            }

            return new Result(String.Format("ASI Camera Found, Total Count: {0}", camera_count));
        }
        private static BayerPattern GetASIBayerPattern(ASI_BAYER_PATTERN bayerPattern)
        {
            switch (bayerPattern)
            {
                case ASI_BAYER_PATTERN.ASI_BAYER_RG:
                    return BayerPattern.RGGB;
                case ASI_BAYER_PATTERN.ASI_BAYER_BG:
                    return BayerPattern.BGGR;
                default:
                    return BayerPattern.Unknown;
            }
        }

        private int _CameraID;
        private CameraStatus _CameraStatus;

        public int CameraID { get { return _CameraID; } }
        public CameraStatus Status { get { return _CameraStatus; } }
        public CameraInfo CameraInfo { get; set; }

        public ASICamera(int cameraID, CameraInfo cameraInfo)
        {
            this._CameraID = cameraID;
            this.CameraInfo = cameraInfo;
        }

        public Result Connect()
        {
            ASI_ERROR_CODE asi_error_code = ASI_ERROR_CODE.ASI_SUCCESS;
            Exception exception = null;
            Result result;

            try
            {
                asi_error_code = ASIOpenCamera(_CameraID);
            }
            catch (Exception e)
            {
                exception = e;
            }
            result = GetOperationResult("ASIOpenCamera", asi_error_code, exception);
            if (result.Code != ErrorCode.OK) return result;

            try
            {
                asi_error_code = ASIInitCamera(_CameraID);
            }
            catch (Exception e)
            {
                exception = e;
            }
            return GetOperationResult("ASIInitCamera", asi_error_code, exception);
        }
        public Result StartCapture(int millisecond, bool isDark)
        {
            ASI_ERROR_CODE asi_error_code = ASI_ERROR_CODE.ASI_SUCCESS;
            Exception exception = null;
            Result result;

            try
            {
                ASISetControlValue(_CameraID, ASI_CONTROL_TYPE.ASI_EXPOSURE, millisecond);
            }
            catch (Exception e)
            {
                exception = e;
            }
            result = GetOperationResult("ASISetControlValue_ASI_EXPOSURE", asi_error_code, exception);
            if (result.Code != ErrorCode.OK) return result;

            try
            {
                asi_error_code = ASIStartExposure(0, (isDark ? ASI_BOOL.ASI_TRUE : ASI_BOOL.ASI_FALSE));
            }
            catch (Exception e)
            {
                exception = e;
            }
            return GetOperationResult("ASIStartExposure_" + (isDark ? "Dark" : "NoDark"), asi_error_code, exception);
        }
        public Result GetCaputreStat(out CameraStatus captureStatus)
        {
            ASI_ERROR_CODE asi_error_code = ASI_ERROR_CODE.ASI_SUCCESS;
            Exception exception = null;
            Result result;

            ASI_EXPOSURE_STATUS aSI_EXPOSURE_STATUS = ASI_EXPOSURE_STATUS.ASI_EXP_WORKING;
            try
            {
                asi_error_code = ASIGetExpStatus(_CameraID, out aSI_EXPOSURE_STATUS);
            }
            catch (Exception e)
            {
                exception = e;
            }
            result = GetOperationResult("ASIGetExpStatus", asi_error_code, exception);

            captureStatus = result.Code == ErrorCode.OK ? ConvertToCameraStatus(aSI_EXPOSURE_STATUS) : CameraStatus.Unknown;
            return result;
        }


        public Result GetCaputreData(UInt16[] data)
        {
            ASI_ERROR_CODE asi_error_code = ASI_ERROR_CODE.ASI_SUCCESS;
            Exception exception = null;

            try
            {
                asi_error_code = ASIGetDataAfterExp(_CameraID, data, data.Length);
            }
            catch (Exception e)
            {
                exception = e;
            }
            return GetOperationResult("ASIGetDataAfterExp", asi_error_code, exception);
        }

        private CameraStatus ConvertToCameraStatus(ASI_EXPOSURE_STATUS aSI_EXPOSURE_STATUS)
        {
            switch (aSI_EXPOSURE_STATUS)
            {
                case ASI_EXPOSURE_STATUS.ASI_EXP_IDLE:
                    return CameraStatus.Idle;
                case ASI_EXPOSURE_STATUS.ASI_EXP_WORKING:
                    return CameraStatus.Working;
                case ASI_EXPOSURE_STATUS.ASI_EXP_SUCCESS:
                    return CameraStatus.CaptureSuccess;
                case ASI_EXPOSURE_STATUS.ASI_EXP_FAILED:
                    return CameraStatus.Error;
                default:
                    return CameraStatus.Unknown;
            }
        }
        private Result GetOperationResult(string operationName, ASI_ERROR_CODE asi_error_code, Exception exception)
        {
            if (exception == null && asi_error_code == ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Log.InfoFormat("{0} Success, CameraID {1}, CameraName {2}", operationName, _CameraID, CameraInfo.DisplayName);
                return new Result();
            }

            string message = String.Format("{0} Failed, CameraID {1}, CameraName {2}, ASI_ERROR_CODE {3}",
                operationName, _CameraID, CameraInfo.DisplayName, asi_error_code.ToString());

            Log.Error(message, exception);

            return new Result(ErrorCode.OperationFailed, message, exception);
        }
    }
}
