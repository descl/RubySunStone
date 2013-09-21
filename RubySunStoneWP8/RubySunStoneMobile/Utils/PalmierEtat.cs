
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RubySunStoneMobile.Utils
{
    class PalmierEtat
    {
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public SolidColorBrush Brush { get; private set; }
        public BitmapImage Image { get; private set; }

        public PalmierEtat()
        {  }

        public PalmierEtat(string name, string desc, string colorHex)
        {
            Name = name;
            Desc = desc;
            Brush = new SolidColorBrush(Color.FromArgb(
                    255,
                    Convert.ToByte(colorHex.Substring(0, 2), 16),
                    Convert.ToByte(colorHex.Substring(2, 2), 16),
                    Convert.ToByte(colorHex.Substring(4, 2), 16)));
            Image = new BitmapImage();
            Image.CreateOptions = BitmapCreateOptions.None;
            Image.UriSource = new Uri("/Assets/" + name + ".png", UriKind.Relative);
            Image.ImageOpened += (a, b) =>
            {
                Console.WriteLine(Image.UriSource + " chargé...");
            };
        }
    }
}
