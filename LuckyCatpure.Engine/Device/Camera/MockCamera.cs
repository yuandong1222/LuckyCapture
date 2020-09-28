using LuckyCatpure.Engine.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Device.Camera
{
    public class MockCamera : ICamera
    {
        private CameraStatus _CameraStatus = CameraStatus.Unknown;

        public CameraInfo CameraInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CameraStatus Status => throw new NotImplementedException();

        public Result Connect()
        {
            _CameraStatus = CameraStatus.Idle;

            return new Result();
        }

        public Result Disconnect()
        {
            throw new NotImplementedException();
        }

        public Result GetCaputreData(ushort[] data)
        {
            throw new NotImplementedException();
        }

        public Result GetCaputreStat(out CameraStatus captureStatus, bool logSuccessResult)
        {
            throw new NotImplementedException();
        }

        public CameraControlItem[] GetControlItems()
        {
            throw new NotImplementedException();
        }

        public Result SetControItemValue(CameraControlItemType type, int value)
        {
            throw new NotImplementedException();
        }

        public Result StartCapture(bool isDark)
        {
            throw new NotImplementedException();
        }
    }
}
