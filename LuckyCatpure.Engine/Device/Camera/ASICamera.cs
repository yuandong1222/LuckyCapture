using System;
using System.Collections.Generic;
using System.Linq;

using LuckyCatpure.Engine.Common;
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
        private List<CameraControlItem> _CameraControlItemList = new List<CameraControlItem>();

        private int _LastExposureTime = -1;

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

            //asi_error_code = ASISetControlValue(_CameraID, ASI_CONTROL_TYPE.ASI_HIGH_SPEED_MODE, 1);
            try
            {
                asi_error_code = ASIInitCamera(_CameraID);
            }
            catch (Exception e)
            {
                exception = e;
            }
            result = GetOperationResult("ASIInitCamera", asi_error_code, exception);
            if (result.Code != ErrorCode.OK) return result;

            return InitialControlItems();
        }
        public Result Disconnect()
        {
            ASI_ERROR_CODE asi_error_code = ASI_ERROR_CODE.ASI_SUCCESS;
            Exception exception = null;
            try
            {
                asi_error_code = ASICloseCamera(_CameraID);
            }
            catch (Exception e)
            {
                exception = e;
            }
            return GetOperationResult("ASICloseCamera", asi_error_code, exception);
        }

        private Result InitialControlItems()
        {
            ASI_ERROR_CODE asi_error_code = ASI_ERROR_CODE.ASI_SUCCESS;
            Exception exception = null;
            Result result;

            int controlItemCount = 0;
            try
            {
                asi_error_code = ASIGetNumOfControls(_CameraID, out controlItemCount);
            }
            catch (Exception e)
            {
                exception = e;
            }
            result = GetOperationResult("ASIGetNumOfControls", asi_error_code, exception);
            if (result.Code != ErrorCode.OK) return result;


            for (int i = 0; i < controlItemCount; i++)
            {
                ASI_CONTROL_CAPS asiControlCaps;
                try
                {
                    asi_error_code = ASIGetControlCaps(_CameraID, i, out asiControlCaps);
                    _CameraControlItemList.Add(ConvertToCameraControlItem(asiControlCaps));
                }
                catch (Exception e)
                {
                    exception = e;
                }
                result = GetOperationResult("ASIGetNumOfControls", asi_error_code, exception);
                if (result.Code != ErrorCode.OK) return result;

            }
            return new Result();
        }

        public CameraControlItem[] GetControlItems()
        {
            return _CameraControlItemList.ToArray();
        }
        public CameraControlItem GetControlItem(CameraControlItemType type)
        {
            return _CameraControlItemList.SingleOrDefault(c => c.ControlItemType == type);
        }
        public Result SetControItemValue(CameraControlItemType type, int value)
        {
            var controlItem = _CameraControlItemList.SingleOrDefault(c => c.ControlItemType == type);
            if (controlItem == null)
                return new Result(ErrorCode.Error, "No Such Control Item");

            //TODO:if newValue = oldValue, do nothing?
            var oldValue = controlItem.Value;

            ASI_ERROR_CODE asi_error_code = ASI_ERROR_CODE.ASI_SUCCESS;
            Exception exception = null;
            try
            {
                asi_error_code = ASISetControlValue(_CameraID, ((ASI_CONTROL_CAPS)controlItem.NativeItem).ControlType, value);
                controlItem.Value = value; //TODO: We should get value from device 
            }
            catch (Exception e)
            {
                exception = e;
            }

            //TODO: Log the description the control item include min and max value, etc.
            return GetOperationResult(String.Format("SetControlValue, ItemName {0}, ItemType {1}, OldValue {2}, NewValue {3}", 
                controlItem.Description, controlItem.ControlItemType, oldValue, value), 
                asi_error_code, exception);
        }

        public Result StartCapture(int millisecond, bool isDark)
        {
            ASI_ERROR_CODE asi_error_code = ASI_ERROR_CODE.ASI_SUCCESS;
            Exception exception = null;
            Result result;

            if (_LastExposureTime != millisecond)
            {
                try
                {
                    asi_error_code = ASISetControlValue(_CameraID, ASI_CONTROL_TYPE.ASI_EXPOSURE, millisecond);
                }
                catch (Exception e)
                {
                    exception = e;
                }
                result = GetOperationResult("ASISetControlValue_ASI_EXPOSURE", asi_error_code, exception);
                if (result.Code != ErrorCode.OK) return result;

                _LastExposureTime = millisecond;
            }

            try
            {
                asi_error_code = ASIStartExposure(_CameraID, (isDark ? ASI_BOOL.ASI_TRUE : ASI_BOOL.ASI_FALSE));
            }
            catch (Exception e)
            {
                exception = e;
            }
            return GetOperationResult("ASIStartExposure_" + (isDark ? "Dark" : "NoDark"), asi_error_code, exception);
        }
        public Result GetCaputreStat(out CameraStatus captureStatus, bool logSuccessResult = false)
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
            result = GetOperationResult("ASIGetExpStatus", asi_error_code, exception, logSuccessResult);

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

            //TODO: DebugLog
            //long value = ASIGetControlValue(_CameraID, ASI_CONTROL_TYPE.ASI_EXPOSURE);
            //Log.InfoFormat("{0}", value);

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
        private CameraControlItem ConvertToCameraControlItem(ASI_CONTROL_CAPS asiControlCaps)
        {
            return new CameraControlItem
            {
                ControlItemName = asiControlCaps.Name,
                ControlItemType = ConvertToCameraControlItemType(asiControlCaps.ControlType),
                DefaultValue = asiControlCaps.DefaultValue,
                Description = asiControlCaps.Description,
                IsAutoSupported = asiControlCaps.IsAutoSupported == ASI_BOOL.ASI_TRUE,
                IsWritable = asiControlCaps.IsWritable == ASI_BOOL.ASI_TRUE,
                MaxValue = asiControlCaps.MaxValue,
                MinValue = asiControlCaps.MinValue,
                Value = asiControlCaps.DefaultValue,
                NativeItem = asiControlCaps,
            };
        }
        private CameraControlItemType ConvertToCameraControlItemType(ASI_CONTROL_TYPE controlType)
        {
            switch (controlType)
            {
                case ASI_CONTROL_TYPE.ASI_GAIN:
                    return CameraControlItemType.Gain;
                case ASI_CONTROL_TYPE.ASI_EXPOSURE:
                    return CameraControlItemType.Exposure;
                case ASI_CONTROL_TYPE.ASI_GAMMA:
                    return CameraControlItemType.Gamma;
                case ASI_CONTROL_TYPE.ASI_WB_R:
                    return CameraControlItemType.WhiteBalance_Red;
                case ASI_CONTROL_TYPE.ASI_WB_B:
                    return CameraControlItemType.WhileBalance_Blue;
                case ASI_CONTROL_TYPE.ASI_BRIGHTNESS:
                    return CameraControlItemType.Brightness;
                case ASI_CONTROL_TYPE.ASI_BANDWIDTHOVERLOAD:
                    return CameraControlItemType.BandwidthHoverLoad;
                case ASI_CONTROL_TYPE.ASI_OVERCLOCK:
                    return CameraControlItemType.OverClock;
                case ASI_CONTROL_TYPE.ASI_TEMPERATURE:
                    return CameraControlItemType.Temperature;
                case ASI_CONTROL_TYPE.ASI_FLIP:
                    return CameraControlItemType.Flip;
                case ASI_CONTROL_TYPE.ASI_AUTO_MAX_GAIN:
                    return CameraControlItemType.Gain;
                case ASI_CONTROL_TYPE.ASI_AUTO_MAX_EXP:
                    return CameraControlItemType.AutoMaxExp;
                case ASI_CONTROL_TYPE.ASI_AUTO_MAX_BRIGHTNESS:
                    return CameraControlItemType.AutoMaxBrightness;
                case ASI_CONTROL_TYPE.ASI_HARDWARE_BIN:
                    return CameraControlItemType.HadrwareBin;
                case ASI_CONTROL_TYPE.ASI_HIGH_SPEED_MODE:
                    return CameraControlItemType.HighSpeedMode;
                case ASI_CONTROL_TYPE.ASI_COOLER_POWER_PERC:
                    return CameraControlItemType.CoolerPowerPercent;
                case ASI_CONTROL_TYPE.ASI_TARGET_TEMP:
                    return CameraControlItemType.TargetTemperature;
                case ASI_CONTROL_TYPE.ASI_COOLER_ON:
                    return CameraControlItemType.CoolerOn;
                case ASI_CONTROL_TYPE.ASI_MONO_BIN:
                    return CameraControlItemType.MonoBin;
                case ASI_CONTROL_TYPE.ASI_FAN_ON:
                    return CameraControlItemType.FanOn;
                case ASI_CONTROL_TYPE.ASI_PATTERN_ADJUST:
                    return CameraControlItemType.PatternAdjust;
                case ASI_CONTROL_TYPE.ASI_ANTI_DEW_HEATER:
                    return CameraControlItemType.AntiDewDeater;
                case ASI_CONTROL_TYPE.ASI_HUMIDITY:
                    return CameraControlItemType.Humdity;
                case ASI_CONTROL_TYPE.ASI_ENABLE_DDR:
                    return CameraControlItemType.EnableDDR;
                default:
                    return CameraControlItemType.Unknown;
            };
        }

        private Result GetOperationResult(string operationName, ASI_ERROR_CODE asi_error_code, Exception exception, bool logSuccess = true)
        {
            if (exception == null && asi_error_code == ASI_ERROR_CODE.ASI_SUCCESS && logSuccess)
            {
                Log.InfoFormat("{0} Success, CameraID {1}, CameraName {2}", operationName, _CameraID, CameraInfo.DisplayName);
                return new Result(String.Format("{0} Success, CameraID {1}, CameraName {2}", operationName, _CameraID, CameraInfo.DisplayName));
            }

            string message = String.Format("{0} Failed, CameraID {1}, CameraName {2}, ASI_ERROR_CODE {3}",
                operationName, _CameraID, CameraInfo.DisplayName, asi_error_code.ToString());

            Log.Error(message, exception);

            return new Result(ErrorCode.OperationFailed, message, exception);
        }
    }
}
