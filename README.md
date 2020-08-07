# LuckyCapture

LuckCapture是一个开源的天文摄影软件，主要用于深空摄影。类似SGP Pro、NINA等。

LuckCapture将专门为Lucky Imaging进行优化，包括

- 尽可能的发挥出相机的拍摄速度
- 初步筛选功能，以便节省磁盘空间
- 灵活的终止条件

## 特性

- 全多个目标、多个通道的自动化拍摄
- 为Lucky Imaging流程优化
- 作为平台Platform：提供Http访问接口和事件回调

## 设计

- 设备接口：ICamera、ITelescope
- 组件接口：IGuider、IPlateSolver

## 版本

- v0.1：最小可运行功能集合

## Milestore 1 - 最小可运行集合

- 设备：相机，赤道仪，调焦座
- 模块：解析，导星
- 功能：序列，多目标

