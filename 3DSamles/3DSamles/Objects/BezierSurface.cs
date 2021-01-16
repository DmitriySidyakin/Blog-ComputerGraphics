using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace StereoCanvasSamples.Objects
{
    public class BezierSurface : World
    {
        public Dictionary<TDPoint, CubeDot> Matrix { get; set; }
        public int xSSAA { get; set; }
        public BezierSurface(int xSSAA)
        {
            this.xSSAA = xSSAA;
            Dictionary<TDPoint, CubeDot> pixels;
            Color color, colorLight, colorShadow;
            Init(out pixels, out color, out colorLight, out colorShadow);
            MakeBezierSurface(pixels, ref color, ref colorLight, ref colorShadow);
            Matrix = pixels;
        }

        private void MakeBezierSurface(Dictionary<TDPoint, CubeDot> _pixels, ref Color color, ref Color colorLight, ref Color colorShadow)
        {
           TDPoint[] points1 = new TDPoint[3] {
                new TDPoint() { X = -10*xSSAA, Y = -10*xSSAA, Z = 0*xSSAA },
                new TDPoint() { X = -10*xSSAA, Y = 0*xSSAA,   Z = 20*xSSAA },
                new TDPoint() { X = -10*xSSAA, Y = 10*xSSAA,  Z = 0*xSSAA },
            };
            /*
            TDPoint[] points2 = new TDPoint[3] {
                new TDPoint() { X = -10*xSSAA, Y = 10*xSSAA,  Z = 0*xSSAA },
                new TDPoint() { X = 0*xSSAA,   Y = 10*xSSAA,  Z = -20*xSSAA },
                new TDPoint() { X = 10*xSSAA,  Y = 10*xSSAA,  Z = 0*xSSAA },
            };*/

            TDPoint[] points3 = new TDPoint[3] {
                new TDPoint() { X = 10*xSSAA,  Y = -10*xSSAA, Z = 0*xSSAA },
                new TDPoint() { X = 10*xSSAA,  Y = 0*xSSAA,   Z = 20*xSSAA },
                new TDPoint() { X = 10*xSSAA,  Y = 10*xSSAA,  Z = 0*xSSAA },
            };
            /*
            TDPoint[] points4 = new TDPoint[3] {
                new TDPoint() { X = -10*xSSAA, Y = -10*xSSAA, Z = 0*xSSAA },
                new TDPoint() { X = 0*xSSAA,   Y = -10*xSSAA, Z = -20*xSSAA },
                new TDPoint() { X = 10*xSSAA,  Y = -10*xSSAA, Z = 0*xSSAA },
            };*/
            
            double t = 0;
            while(t <= 1)
            {
                //AddPointIfNoExists(_pixels, new TDPoint() { X = (long)Sum(points1.Select(p => p.X), t), Y = (long)Sum(points1.Select(p => p.Y), t), Z = (long)Sum(points1.Select(p => p.Z), t) }, ref color, ref color, ref color);
                TDPoint p1 = new TDPoint() { X = (long)Sum(points1.Select(p => p.X), t), Y = (long)Sum(points1.Select(p => p.Y), t), Z = (long)Sum(points1.Select(p => p.Z), t) };
                TDPoint p3 = new TDPoint() { X = (long)Sum(points3.Select(p => p.X), t), Y = (long)Sum(points3.Select(p => p.Y), t), Z = (long)Sum(points3.Select(p => p.Z), t) };
                TDPoint[] points13 = new TDPoint[2] {
                        p1, p3
                    };

                double t2 = 0;
                while ( t2 <= 1)
                {


                    TDPoint p13 = new TDPoint() { X = (long)Sum(points13.Select(p => p.X), t2), Y = (long)Sum(points13.Select(p => p.Y), t2), Z = (long)Sum(points13.Select(p => p.Z), t2) };
                    AddPointIfNoExists(_pixels, p13, ref color, ref colorLight, ref colorShadow);
                    t2 += 0.01;
                }
                t += 0.01;
            }

            // Добавляем направляющие точки:
            /*
            Color bezierPointColor = Colors.Red;
            AddPointIfNoExists(_pixels, points1[0],ref bezierPointColor, ref bezierPointColor, ref bezierPointColor);
            AddPointIfNoExists(_pixels, points1[1], ref bezierPointColor, ref bezierPointColor, ref bezierPointColor);
            AddPointIfNoExists(_pixels, points2[0], ref bezierPointColor, ref bezierPointColor, ref bezierPointColor);
            AddPointIfNoExists(_pixels, points2[1], ref bezierPointColor, ref bezierPointColor, ref bezierPointColor);
            AddPointIfNoExists(_pixels, points3[0], ref bezierPointColor, ref bezierPointColor, ref bezierPointColor);
            AddPointIfNoExists(_pixels, points3[1], ref bezierPointColor, ref bezierPointColor, ref bezierPointColor);
            AddPointIfNoExists(_pixels, points4[0], ref bezierPointColor, ref bezierPointColor, ref bezierPointColor);
            AddPointIfNoExists(_pixels, points4[1], ref bezierPointColor, ref bezierPointColor, ref bezierPointColor);*/
        }

        private decimal Sum(IEnumerable<long> points, double t)
        {
            byte n = (byte) (points.Count() - 1);
            byte k = 0;
            decimal result = 0;
            foreach(var p in points)
            {
                result += p * CalculateBezierMultiplier(t, k++, n);
            }
            return result;
        }

        private void AddPointIfNoExists(Dictionary<TDPoint, CubeDot> pixels, TDPoint coords, ref Color color, ref Color colorLight, ref Color colorShadow)
        {
            if (!IsExists(pixels, coords))
                if (IsInnerPoint(coords))
                {
                    MakeInnerPoint(pixels, ref color, ref colorLight, ref colorShadow, coords);
                }
                else
                {
                    MakeEdgePoint(pixels, coords);
                }
        }

        private bool IsInnerPoint(TDPoint coords)
        {
            return Math.Abs(coords.X) <= 10*xSSAA - xSSAA || Math.Abs(coords.Y) <= 10 * xSSAA - xSSAA || Math.Abs(coords.Z) <= 10 * xSSAA - xSSAA;
        }

        private bool IsExists(Dictionary<TDPoint, CubeDot> pixels, TDPoint coords)
        {
            return pixels.Keys.Where(k => k.X == coords.X && k.Y == coords.Y && k.Z == coords.Z).Count() > 0;
        }

        private decimal CalculateBezierMultiplier(double t, uint k, uint n)
        {
            return (Factorial(n)/(Factorial(k)*Factorial(n-k)))*((decimal)(Math.Pow(t,k)* Math.Pow(1-t, n-k)));
        }

        public static uint Factorial(uint num)
        {
            uint fact = 1;
            while (num > 1) fact *= num--;
            return fact;
        }

        private void MakeEdgePoint(Dictionary<TDPoint, CubeDot> pixels, TDPoint coords)
        {
            Color finalColor = Colors.Black;
            pixels.Add(new TDPoint() { X = coords.X, Y = coords.Y, Z = coords.Z }, new CubeDot() { Color = finalColor });
        }

        private void MakeInnerPoint(Dictionary<TDPoint, CubeDot> pixels, ref Color color, ref Color colorLight, ref Color colorShadow, TDPoint coords)
        {
            double deltaLight, deltaShadow;
            InitDeltaColor( coords.X, coords.Y, coords.Z, out deltaLight, out deltaShadow);

            byte r, g, b;
            MakeCurrentColor(ref color, ref colorLight, ref colorShadow, deltaLight, deltaShadow, out r, out g, out b);

            Color finalColor = new Color() { A = 255, R = r, G = g, B = b };
            pixels.Add(coords, new CubeDot() { Color = finalColor });
        }

        private void MakeCurrentColor(ref Color color, ref Color colorLight, ref Color colorShadow, double deltaLight, double deltaShadow, out byte r, out byte g, out byte b)
        {
            if (deltaLight > 0)
            {
                r = (byte)Math.Round(color.R * (1 - deltaLight) + colorLight.R * (deltaLight));
                g = (byte)Math.Round(color.G * (1 - deltaLight) + colorLight.G * (deltaLight));
                b = (byte)Math.Round(color.B * (1 - deltaLight) + colorLight.B * (deltaLight));
            }
            else if (deltaShadow > 0)
            {
                r = (byte)Math.Round(color.R * (1 - deltaShadow) + colorShadow.R * (deltaShadow));
                g = (byte)Math.Round(color.G * (1 - deltaShadow) + colorShadow.G * (deltaShadow));
                b = (byte)Math.Round(color.B * (1 - deltaShadow) + colorShadow.B * (deltaShadow));
            }
            else
            {
                r = color.R;
                g = color.G;
                b = color.B;
            }
        }

        private void Init(out Dictionary<TDPoint, CubeDot> pixels, out Color color, out Color colorLight, out Color colorShadow)
        {
            pixels = new Dictionary<TDPoint, CubeDot>();
            color = Colors.Blue;
            colorLight = Colors.White;
            colorShadow = Colors.Black;
        }

        private void InitDeltaColor(long x, long y, long z, out double deltaLight, out double deltaShadow)
        {
            if (z <= 0) deltaLight = 0;
            else deltaLight = (double) z / (10*xSSAA);

            if (z >= 0) deltaShadow = 0;
            else deltaShadow = (double) -z / (10 * xSSAA);
        }

    }
}
