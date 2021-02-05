using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace StereoCanvasSamples.Objects
{
    public class Cube : World
    {
        public Dictionary<TDPoint, CubeDot> Matrix { get; set; }
        public int xSSAA { get; set; }
        public Cube(long cubeWidth, int xSSAA)
        {
            int cubeHalf;
            this.xSSAA = xSSAA;
            Dictionary<TDPoint, CubeDot> pixels;
            Color color, colorLight, colorShadow;
            Init(cubeWidth, out cubeHalf, out pixels, out color, out colorLight, out colorShadow);
            MakeCube(cubeHalf, pixels, ref color, ref colorLight, ref colorShadow);
            Matrix = pixels;
        }

        private void MakeCube(int _cubeHalf, Dictionary<TDPoint, CubeDot> _pixels, ref Color color, ref Color colorLight, ref Color colorShadow)
        {
            for (int x = -_cubeHalf; x <= _cubeHalf; x++)
                for (int y = -_cubeHalf; y <= _cubeHalf; y++)
                    for (int z = -_cubeHalf; z <= _cubeHalf; z++)
                    {
                        if (IsInnerPoint(_cubeHalf, x, y, z))
                        {
                            MakeInnerPoint(_cubeHalf, _pixels, ref color, ref colorLight, ref colorShadow, x, y, z);
                        }
                        else
                        {
                            MakeEdgePoint(_pixels, x, y, z);
                        }
                    }
        }

        private void MakeEdgePoint(Dictionary<TDPoint, CubeDot> pixels, int x, int y, int z)
        {
            Color finalColor = Colors.Black;
            pixels.Add(new TDPoint() { X = x, Y = y, Z = z }, new CubeDot() { Color = finalColor });
        }

        private void MakeInnerPoint(int _cubeHalf, Dictionary<TDPoint, CubeDot> pixels, ref Color color, ref Color colorLight, ref Color colorShadow, int x, int y, int z)
        {
            double deltaLight, deltaShadow;
            InitDeltaColor(_cubeHalf, x, y, z, out deltaLight, out deltaShadow);

            byte r, g, b;
            MakeCurrentColor(ref color, ref colorLight, ref colorShadow, deltaLight, deltaShadow, out r, out g, out b);

            Color finalColor = new Color() { A = 255, R = r, G = g, B = b };
            pixels.Add(new TDPoint() { X = x, Y = y, Z = z }, new CubeDot() { Color = finalColor });
        }

        private void MakeCurrentColor(ref Color color, ref Color colorLight, ref Color colorShadow, double deltaLight, double deltaShadow, out byte r, out byte g, out byte b)
        {
            if (deltaLight > 0)
            {
                r = (byte)Math.Round(color.R * (deltaLight) + colorLight.R * (1 - deltaLight));
                g = (byte)Math.Round(color.G * (deltaLight) + colorLight.R * (1 - deltaLight));
                b = (byte)Math.Round(color.B * (deltaLight) + colorLight.R * (1 - deltaLight));
            }
            else if (deltaShadow > 0)
            {
                r = (byte)Math.Round(color.R * (deltaShadow) + colorShadow.R * (1 - deltaShadow));
                g = (byte)Math.Round(color.G * (deltaShadow) + colorShadow.G * (1 - deltaShadow));
                b = (byte)Math.Round(color.B * (deltaShadow) + colorShadow.B * (1 - deltaShadow));
            }
            else
            {
                r = color.R;
                g = color.G;
                b = color.B;
            }
        }

        private void Init(long cubeWidth, out int cubeHalf, out Dictionary<TDPoint, CubeDot> pixels, out Color color, out Color colorLight, out Color colorShadow)
        {
            cubeHalf = (int)Math.Round((double)cubeWidth / 2);
            pixels = new Dictionary<TDPoint, CubeDot>();
            color = Colors.Blue;
            colorLight = Colors.White;
            colorShadow = Colors.Black;
        }

        private void InitDeltaColor(int cubeHalf, int x, int y, int z, out double deltaLight, out double deltaShadow)
        {
            deltaLight = Math.Sqrt((cubeHalf - x) * (cubeHalf - x) + (cubeHalf - y) * (cubeHalf - y) + (cubeHalf - z) * (cubeHalf - z)) / Math.Sqrt(cubeHalf * cubeHalf + cubeHalf * cubeHalf + cubeHalf * cubeHalf);
            if (deltaLight > 1) deltaLight = 0;
            deltaShadow = Math.Sqrt((-cubeHalf - x) * (-cubeHalf - x) + (-cubeHalf - y) * (-cubeHalf - y) + (-cubeHalf - z) * (-cubeHalf - z)) / Math.Sqrt(cubeHalf * cubeHalf + cubeHalf * cubeHalf + cubeHalf * cubeHalf);
            if (deltaShadow > 1) deltaShadow = 0;
        }

        private bool IsInnerPoint(int cubeHalf, int x, int y, int z)
        {
            return !(
                                        (isCubeHalf(x, cubeHalf) && isCubeHalf(y, cubeHalf)) ||
                                        (isCubeHalf(x, cubeHalf) && isCubeHalf(z, cubeHalf)) ||
                                        (isCubeHalf(x, -cubeHalf) && isCubeHalf(y, cubeHalf)) ||
                                        (isCubeHalf(x, -cubeHalf) && isCubeHalf(z, cubeHalf)) ||
                                        (isCubeHalf(x, cubeHalf) && isCubeHalf(y, -cubeHalf)) ||
                                        (isCubeHalf(x, cubeHalf) && isCubeHalf(z, -cubeHalf)) ||
                                        (isCubeHalf(x, -cubeHalf) && isCubeHalf(y, -cubeHalf)) ||
                                        (isCubeHalf(x, -cubeHalf) && isCubeHalf(z, -cubeHalf)) ||
                                        (isCubeHalf(y, cubeHalf) && isCubeHalf(z, cubeHalf)) ||
                                        (isCubeHalf(y, -cubeHalf) && isCubeHalf(z, cubeHalf)) ||
                                        (isCubeHalf(y, cubeHalf) && isCubeHalf(z, -cubeHalf)) ||
                                        (isCubeHalf(y, -cubeHalf) && isCubeHalf(z, -cubeHalf))
                                        );
        }
        private bool isCubeHalf(long coord, int cubeHalf)
        {
            return (coord >= (cubeHalf - xSSAA/2) && coord <= (cubeHalf - xSSAA/2));
        }
    }
}
