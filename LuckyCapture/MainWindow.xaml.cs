using LuckyCatpure.Engine.Common;
using LuckyCatpure.Engine.Device.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Xceed.Wpf.Toolkit;
using ZWOptical.ASISDK;
using static ZWOptical.ASISDK.ASICameraDll2;

namespace LuckyCapture
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CameraConnect();
        }

        private async void CameraConnect()
        {
            Result result;

            Log.Info("Test started");

            List<ICamera> cameraList = new List<ICamera>();
            result = ASICamera.ScanASICameras(cameraList);
            if (Log.Result(result).Code != ErrorCode.OK) return;

            if (cameraList.Count == 0)
            {
                Log.Info("No Camera Found");
                return;
            }

            var camera = cameraList[0];
            result = camera.Connect();
            if (Log.Result(result).Code != ErrorCode.OK) return;

            ushort[] buffer = new ushort[camera.CameraInfo.MaxPixelCount];

            var startime = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                camera.StartCapture(1, false);
                //camera.StartCapture(i, false);

                CameraStatus status;
                do
                {
                    result = camera.GetCaputreStat(out status);
                } while (result.Code == ErrorCode.OK && status == CameraStatus.Working);

                if (Log.Result(result).Code != ErrorCode.OK) return;
                if (result.Code != ErrorCode.OK)
                {
                    Log.Result(result);
                    return;
                }
                if (status != CameraStatus.CaptureSuccess)
                {
                    Log.ErrorFormat("Capture Failed, CameraName {0}, ExposureTime {1}ms", camera.CameraInfo.DisplayName, i);
                    return;
                }

                result = camera.GetCaputreData(buffer);
                if (result.Code == ErrorCode.OK)
                {
                    Log.InfoFormat("{0}, ExposureTime {1}ms", result.Message, i);
                }
                else
                {
                    Log.ErrorFormat("{0}, ExposureTime {1}ms", result.Message, i);
                    return;
                }
            }

            var timecost = DateTime.Now - startime;
            Log.InfoFormat("Total Time Cost: {0}s", timecost.TotalSeconds);
        }
    }
}
