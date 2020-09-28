using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Device.Camera
{
    public enum CameraControlItemType
    {
        Unknown = 0,
        Gain,
        Exposure,
        Gamma,
        WhiteBalance_Red,
        WhileBalance_Blue,
        Brightness,
        BandwidthHoverLoad,
        OverClock,
        Temperature,
        TargetTemperature,
        Flip,
        AutoMaxGain,
        AutoMaxExp,
        AutoMaxBrightness,
        HadrwareBin,
        MonoBin,
        HighSpeedMode,
        FanOn,
        CoolerOn,
        CoolerPowerPercent,
        PatternAdjust,
        AntiDewDeater,
        Humdity,
        EnableDDR,
    }
    public class CameraControlItem
    {
        public string ControlItemName { get; set; } //the name of the Control like Exposure, Gain etc..
        public string Description { get; set; } //description of this control

        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public int DefaultValue { get; set; }
        public int Value { get; set; }
        public bool IsAutoSupported { get; set; } //support auto set 1, don't support 0
        public bool IsWritable { get; set; } //some control like temperature can only be read by some cameras
        public CameraControlItemType ControlItemType { get; set; } //this is used to get value and set value of the control

        public object NativeItem { get; set; }
    }
}
