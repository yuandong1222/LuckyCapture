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
            var logBuilder = new StringBuilder();

            ASI_ERROR_CODE asi_result = 0;
            ASI_EXPOSURE_STATUS asi_exposure_status;

            //find camera
            int camera_count = ASICameraDll2.ASIGetNumOfConnectedCameras();
            logBuilder.AppendFormat("Camera Count: {0} \n", camera_count);

            if (camera_count == 0)
            {
                logBuilder.AppendLine("No Camera Found");
                Content1.Content = logBuilder.ToString();
                return;
            }

            //Open
            asi_result = ASICameraDll2.ASIOpenCamera(0);

            //Init
            asi_result = ASICameraDll2.ASIInitCamera(0);

            //ASIStartExposure
            asi_result = ASICameraDll2.ASIStartExposure(0, ASI_BOOL.ASI_FALSE);


            //ASIGetExpStatus
            Thread.Sleep(1000);


            //if(status ==ASI_EXP_SUCCESS) ASIGetDataAfterExp
            int size = 10000 * 10000;
            IntPtr a = new IntPtr(size);
            ushort[] arr = new ushort[size];
            int buffersize = size * 2;
            asi_result = ASICameraDll2.ASIGetDataAfterExp(0, arr, buffersize);
            logBuilder.AppendFormat("Result {0}\n", asi_result);
            Content1.Content = logBuilder.ToString();
        }
    }
}
