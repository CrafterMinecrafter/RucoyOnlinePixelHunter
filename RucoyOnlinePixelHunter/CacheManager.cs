using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RucoyOnline.PixelHunter
{
    public enum Cache
    {
        Target,
        Mummy,
    }
    public static class CacheManager
    {

        static CacheManager()
        {
            img_text.Add(Cache.Target, Image.FromFile("./res/ImAttack.png"));
            img_text.Add(Cache.Mummy, Image.FromFile("./res/mummy.png"));
        }
        public static Dictionary<Cache, Image> img_text = new Dictionary<Cache, Image>();

        public static int bitmapHeight
        {
            get
            {
                return _bitmapHeight;
            }
            set
            {
                if (_bitmapHeight != value)
                {
                    _bitmapHeight = value;
                    Bot.sourceResize = true;
                }
            }
        }
        public static int bitmapWidth
        {
            get
            {
                return _bitmapWidth;
            }
            set
            {
                if (_bitmapWidth != value)
                {
                    _bitmapWidth = value;
                    Bot.sourceResize = true;
                }
            }
        }

        private static int _bitmapHeight = 4;
        private static int _bitmapWidth = 4;

    }
}
