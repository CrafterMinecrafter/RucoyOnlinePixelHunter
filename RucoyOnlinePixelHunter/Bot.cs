using ImageFinderNS;
using SendInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RucoyOnline.PixelHunter
{
    public class Bot
    {
        public static bool sourceResize = false;
        public Bot()
        {
            application_source = new Bitmap(1, 1);
        }
        public void DoSourceResize()
        {
            application_source = new Bitmap(CacheManager.bitmapWidth, CacheManager.bitmapHeight);
            ImageFinder.SetSource(application_source);
        }

        public Bitmap application_source;

        public void Update()
        {
            if (Form1.emulator == null) return;
            //capture
            IntPtr hwnd = Form1.emulator.MainWindowHandle;
            CaptureWindow(hwnd);
            //work
            DoWork();
        }
        #region copy paste

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr handle, ref Rectangle rect);


        public void CaptureWindow(IntPtr handle)
        {
            // Get the size of the window to capture
            Rectangle rect = new Rectangle();
            GetWindowRect(handle, ref rect);

            // GetWindowRect returns Top/Left and Bottom/Right, so fix it
            rect.Width = rect.Width - rect.X;
            rect.Height = rect.Height - rect.Y;
            // Create a bitmap to draw the capture into
            CacheManager.bitmapWidth = rect.Width;
            CacheManager.bitmapHeight = rect.Height;
            if (sourceResize)
            {
                DoSourceResize();
                sourceResize = false;
            }
            // Use PrintWindow to draw the window into our bitmap
            Graphics g = Graphics.FromImage(application_source);


            IntPtr hdc = g.GetHdc();
            if (!PrintWindow(handle, hdc, 0))
            {
                int error = Marshal.GetLastWin32Error();
                var exception = new System.ComponentModel.Win32Exception(error);
                Debug.WriteLine("ERROR: " + error + ": " + exception.Message);
                // TODO: Throw the exception?
            }
            g.ReleaseHdc(hdc);
        }
        #endregion
        public void DoWork()
        {
            var target = TryFind(Cache.Target, 0.01f);
            var enemies = TryFind(Cache.Mummy);

            bool TargetedNow = target.Count != 0;
            Graphics g = Graphics.FromImage(application_source);
            foreach (var e in enemies)
            {
                Rectangle rect = Rectangle.Empty;
                GetWindowRect(Form1.emulator.MainWindowHandle, ref rect);
                g.DrawRectangle(new Pen(Color.Red), e.Zone);
                if (!TargetedNow)
                    MouseInput.LeftClick(e.Zone.X, e.Zone.Y);
            }
            foreach (var t in target)
            {
                g.DrawRectangle(new Pen(Color.DarkRed), t.Zone);
            }
        }
        private List<ImageFinder.Match> TryFind(Cache target, float accuracy = 0.8f)
        {
            Image img;
            if (CacheManager.img_text.TryGetValue(target, out img))
                return ImageFinder.Find(img, accuracy);
            else
                return null;
        }
    }
}
