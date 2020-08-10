# LuckyCapture

LuckyCapture是一个开源的天文摄影软件，主要用于深空摄影。类似SGP Pro、NINA、ACP等。

LuckyCapture主要服务于远程台，旨在充分实现自动化拍摄，降低拍摄过程中的人工干预，以便尽可能充分利用可拍摄的时间，提高拍摄效率。

LuckyCapture设计为对于给定的目标序列，在满足条件的时候自动开始拍摄，包括设备自动上电，自动启动拍摄流程，自动预处理拍摄结果和自动停机等，全程无需人工干预。

LuckyCapture采用平台化的思路，提供所有功能的外部接口，包括用于控制的HTTP访问接口和用于通知的回调接口，以便与其它系统联动。

LuckCapture设计实现为一个服务，即7\*24小时。通常要求运行在可7\*24小时运行的硬件，例如服务器，工控机上。

LuckyCapture将专门为Lucky Imaging进行优化，包括

- 尽可能的发挥出相机的拍摄速度
- 初步筛选功能，以便节省磁盘空间
- 灵活的终止条件

## 特性

- 全多个目标、多个通道的自动化拍摄
- 为Lucky Imaging流程优化
- 平台化Platform：提供Http访问接口和事件回调
- 校检帧的自动拍摄与合成
- 自动化预处理（暗场，平场，Bias）
- 设备电源的自动管理
- 自动启动拍摄与自动终止拍摄
- 拍摄计划的辅助制订
- 统计报表

## 版本

- v0.1：最小可运行功能集合
- v0.2：面向Lucky Imaging的优化

### v0.1：最小可运行集合

- 设备：相机，赤道仪，调焦座
- 模块：解析，导星
- 功能：序列，多目标

## 软件设计

![image](http://github.com/yuandong1222/LuckyCapture/Doc/Architecture.png)
