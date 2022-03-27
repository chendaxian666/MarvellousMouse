using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarvellousMouse
{
    public class ElementProvider
    {
        public static IList<T>  GetElements<T>()
        {
            var list = new List<T>();
            if (typeof(T) == typeof(MyImage))
            {
                return GetMyImage() as List<T>;
            }
            else
            {
                return null;
            }
        }

        private static IList<MyImage> GetMyImage()
        {
            var list = new List<MyImage>();
            list.Add(new MyImage("/Flowers/1.png"));
            list.Add(new MyImage("/Flowers/2.png"));
            list.Add(new MyImage("/Flowers/4.png"));
            list.Add(new MyImage("/Flowers/5.png"));
            list.Add(new MyImage("/Flowers/6.png"));
            list.Add(new MyImage("/Flowers/7.png"));
            return list;
        }
    }
}
