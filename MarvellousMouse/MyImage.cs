using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MarvellousMouse
{
    public class MyImage : Image
    {
        public MyImage(string path)
        {
            var source = new BitmapImage(new Uri(path,UriKind.Relative));
            Source = source;
            Height = 0;
            Width = 0;
        }
    }
}
