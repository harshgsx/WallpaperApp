using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace WallpaperApp
{
    public sealed class Wallpaper
    {
        Wallpaper() { }

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto )]
        static extern int SystemParametersInfo(int uAction, int uParamm , String lpvparamram, int fuWinIni);

        public enum Style : int
        {
            Tiled,
            Centered,
            Stretched,
            Fill,
            Fit,
            Span
        }

        public static void Set(String FilePath, Style style)
        {
            System.IO.Stream s = File.OpenRead(FilePath);           
            System.Drawing.Image img = System.Drawing.Image.FromStream(s);
            
            string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);
            s.Close();
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == Style.Stretched)
            {
                key.SetValue(@"WallpaperStyle", "2");
                key.SetValue(@"TileWallpaper", "0");
            }

            if (style == Style.Centered)
            {
                key.SetValue(@"WallpaperStyle", "3");
                key.SetValue(@"TileWallpaper", "0");
            }

            if (style == Style.Tiled)
            {
                key.SetValue(@"WallpaperStyle", "1");
                key.SetValue(@"TileWallpaper", "1");
            }
            if(style == Style.Fit)
            {
                key.SetValue(@"WallpaperStyle", "6");
                key.SetValue(@"TileWallpaper", "0");
            }
            if (style == Style.Fill)
            {
                key.SetValue(@"WallpaperStyle", "10");
                key.SetValue(@"TileWallpaper", "0");
            }
            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                tempPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

    }

}

