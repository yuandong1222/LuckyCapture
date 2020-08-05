# LuckyCapture

LuckCapture是一个开源的天文摄影软件，主要用于深空摄影。类似SGP Pro、NINA等。

LuckCapture将专门为Lucky Imaging进行优化，包括

- 尽可能的发挥出相机的拍摄速度
- 初步筛选功能，以便节省磁盘空间
- 灵活的终止条件

## 特性

- 全多个目标、多个通道的自动化拍摄
- 为Lucky Imaging流程优化

## 设计

- 设备接口：ICamera、ITelescope
- 组件接口：IGuider、IPlateSolver

## 实现

- Milestore 1：界面原型
- MileStore 2：ASI6000MM Pro高速读取（2FPS/s）

