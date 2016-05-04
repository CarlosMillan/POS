using System.Windows.Media;

namespace Gestionix.POS.GUI
{
    public static class Functions
    {
        public static Brush IncreasedColor(SolidColorBrush basecolor)
        {
            float IncrementPercentage = .20f;
            int[] RGB = new int[3];
            RGB[0] = basecolor.Color.R;
            RGB[1] = basecolor.Color.G;
            RGB[2] = basecolor.Color.B;

            RGB[0] += (int)(RGB[0] * IncrementPercentage);
            RGB[1] += (int)(RGB[1] * IncrementPercentage);
            RGB[2] += (int)(RGB[2] * IncrementPercentage);

            RGB[0] = RGB[0] > 255 ? 255 : RGB[0];
            RGB[1] = RGB[1] > 255 ? 255 : RGB[1];
            RGB[2] = RGB[2] > 255 ? 255 : RGB[2];

            return new SolidColorBrush(Color.FromRgb((byte)RGB[0], (byte)RGB[1], (byte)RGB[2]));
        }
    }
}
