using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Linq;

namespace Helper.Extensions
{
    public static class ColorExtensions
    {
        public static Color GetContrastingColor(this Color value)
        {
            // Counting the perceptive luminance - human eye favors green color...
            var a = 1 - (0.299 * value.R + 0.587 * value.G + 0.114 * value.B) / 255;

            var d = a < 0.5 ? 0 : 255;

            return Color.FromArgb(d, d, d);
        }

        public static string GetApproximateColorName(this Color value)
        {
            var minDistance = int.MaxValue;
            var minColor = Color.Black.Name;

            foreach (var colorProperty in ColorProperties)
            {
                var colorPropertyValue = (Color)colorProperty.GetValue(null, null);

                if (colorPropertyValue.R == value.R && colorPropertyValue.G == value.G && colorPropertyValue.B == value.B)
                    return colorPropertyValue.Name;

                var distance = Math.Abs(colorPropertyValue.R - value.R) + Math.Abs(colorPropertyValue.G - value.G) + Math.Abs(colorPropertyValue.B - value.B);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    minColor = colorPropertyValue.Name;
                }
            }

            return minColor;
        }

        private static IEnumerable<PropertyInfo> ColorProperties => typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static)
                                                                                 .Where(p => p.PropertyType == typeof(Color));
    }
}
