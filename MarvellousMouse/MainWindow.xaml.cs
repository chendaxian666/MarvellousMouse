using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarvellousMouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 属性
        private int NumofGrap;
        private const double DurationTime = 1000;
        private const int Radius = 20;                 //小球半径，与DAWdith和DAHeight有关
        private const int RandomRadius = 50;           //随机范围

        public Storyboard MyStoryBoard { get; set; }

        public DoubleAnimation DAWidth { get; set; }

        public DoubleAnimation DAHeight { get; set; }

        public DoubleAnimation DAOpacity { get; set; }

        public double ScaleX { get; set; }

        public double ScaleY { get; set; }

        private Random _random = new Random();

        private SolidColorBrush InvarColor = new SolidColorBrush(Colors.Green);

        public SolidColorBrush RandomColor
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(Convert.ToByte(_random.Next(0, 255)), Convert.ToByte(_random.Next(0, 255)), Convert.ToByte(_random.Next(0, 255)), Convert.ToByte(_random.Next(0, 255))));
            }
        }

        public int RandomDistance
        {
            get
            {
                return _random.Next(-RandomRadius, RandomRadius);
            }
        }

        #endregion 属性
        public MainWindow()
        {
            InitializeComponent();
            InitProperty();
            InitGraph();
            MaxmizeAndTransparenceWindow();
            StartMarvellousMouse();
        }

        #region 方法

        /// <summary>
        /// 设置动画，height，width，opacity变化,对于frameworkelement有效
        /// </summary>
        private void InitProperty()
        {
            MyStoryBoard = new Storyboard();
            //MyStoryBoard.RepeatBehavior = RepeatBehavior.Forever;
            DAWidth = new DoubleAnimation();
            DAWidth.From = 0;
            DAWidth.To = 2 * Radius;
            DAWidth.Duration = new Duration(TimeSpan.FromMilliseconds(DurationTime));
            DAHeight = new DoubleAnimation();
            DAHeight.From = 0;
            DAHeight.To = 2 * Radius;
            DAHeight.Duration = new Duration(TimeSpan.FromMilliseconds(DurationTime));
            DAOpacity = new DoubleAnimation();
            DAOpacity.From = 0;
            DAOpacity.To = 0.7;
            DAOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(DurationTime));
            MyStoryBoard.Children.Add(DAWidth);
            MyStoryBoard.Children.Add(DAHeight);
            MyStoryBoard.Children.Add(DAOpacity);
        }

        /// <summary>
        /// 核心函数 开始画圆
        /// </summary>
        private void StartMarvellousMouse()
        {
            POINT lpPoint;
            var widthProperty = new PropertyPath("Width");
            var heightProperty = new PropertyPath("Height");
            var opacityProperty = new PropertyPath("Opacity");
            new Thread(() =>
            {
                while (true)
                {
                    for (int i = 0; i < NumofGrap; i++)
                    {
                        GetCursorPos(out lpPoint);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (i == NumofGrap) return;           //防止循环i=NumofGrap的时候，ui线程正好访问
                            var item = (FrameworkElement)this.canvas.Children[i];
                            Canvas.SetLeft(item, (lpPoint.X - Radius + RandomDistance) / ScaleX);
                            Canvas.SetTop(item, (lpPoint.Y - Radius + RandomDistance) / ScaleY);
                            //item.Fill = RandomColor;
                            Storyboard.SetTarget(DAWidth, item);
                            Storyboard.SetTargetProperty(DAWidth, widthProperty);
                            Storyboard.SetTarget(DAHeight, item);
                            Storyboard.SetTargetProperty(DAHeight, heightProperty);
                            Storyboard.SetTarget(DAOpacity, item);
                            Storyboard.SetTargetProperty(DAOpacity, opacityProperty);
                            MyStoryBoard.Begin();
                        }));
                        Thread.Sleep((int)DurationTime / NumofGrap);
                    }
                }

            }).Start();
        }

        /// <summary>
        /// 添加显示对象个数，
        /// </summary>
        /// <param name="num"></param>
        private void InitGraph()
        {
            var elements =ElementProvider.GetElements<MyImage>();
            if (elements != null)
            {
                foreach (var item in elements)
                {
                    this.canvas.Children.Add(item);
                }
            }
            NumofGrap = this.canvas.Children.Count;
        }

        /// <summary>
        /// 初始化屏幕，透明最大化等
        /// </summary>
        private void MaxmizeAndTransparenceWindow()
        {
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            var hdc = GetDC(GetDesktopWindow());
            int ResolutionHeight = GetDeviceCaps(hdc, 10);        //高
            int ResolutionWidth = GetDeviceCaps(hdc, 8);       //宽
            ScaleX = (double)(ResolutionWidth / Width);
            ScaleY = (double)(ResolutionHeight / Height);
            this.Top = 0;
            this.Left = 0;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;
        }

        #endregion 方法

        #region 获取鼠标相关
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }


        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);


        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(
            IntPtr hdc, // handle to DC
            int nIndex // index of capability
        );

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        #endregion 获取鼠标相关
    }

}
