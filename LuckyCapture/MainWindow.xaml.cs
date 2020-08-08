using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void CameraConnect()
        {
            Button1.Content = ASICameraDll2.ASIGetNumOfConnectedCameras();
            Content1.Content = ASICameraDll2.ASIGetNumOfConnectedCameras();
        }
    }
}
