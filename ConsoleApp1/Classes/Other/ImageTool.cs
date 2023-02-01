using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CloudLibrary
{
    public class ImageTool
    {
        public static byte[] ImageFromUrl(string url)
        {
            using (WebClient wc = new WebClient())
            {
                byte[] originalData = wc.DownloadData(url);
                return originalData;
            }
        }
        //public static Bitmap BitmapFromBytes(byte[] bytes)
        //{
        //    using (var ms = new MemoryStream(bytes))
        //    {
        //        return new Bitmap(ms);
        //    }
        //}
    }
}
