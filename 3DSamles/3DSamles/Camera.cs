using System;

namespace StereoCanvasSamples
{
    public class Camera
    {
        public Projection Projection { get; set; }
        // Расстояние до точки фокусировки.
        public long FocusDotDistance { get; set; }
        // Координаты верхней левой точки.
        public TDPoint TopLeft { get; set; }

        // Координаты точки сверху справа.
        public TDPoint TopRight { get; set; }

        // Координаты точки снизу слева.
        public TDPoint BottomLeft { get; set; }

        // Максимальная дальность дискретизации.
        public long MaxDistance { get; set; }

        public Direction Direction { get; set; }


        /// <summary>
        /// Ширина камеры
        /// </summary>
        public long Width
        {
            get
            {
                long Lx, Ly, Lz, L;

                Lx = TopLeft.X - TopRight.X;
                Ly = TopLeft.Y - TopRight.Y;
                Lz = TopLeft.Z - TopRight.Z;

                L = (long)Math.Round(Math.Sqrt(Lx * Lx + Ly * Ly + Lz * Lz));

                return L;
            }
        }

        /// <summary>
        /// Высота камеры
        /// </summary>
        public long Height
        {
            get
            {
                long Lx, Ly, Lz, L;

                Lx = TopLeft.X - BottomLeft.X;
                Ly = TopLeft.Y - BottomLeft.Y;
                Lz = TopLeft.Z - BottomLeft.Z;

                L = (long)Math.Round(Math.Sqrt(Lx * Lx + Ly * Ly + Lz * Lz));

                return L;
            }
        }

        // Координаты правой точки снизу.
        public TDPoint BottomRight
        {
            get
            {
                int signX, signY, signZ;
                signX = TopLeft.X > BottomLeft.X ? -1 : 1;
                signY = TopLeft.Y > BottomLeft.Y ? -1 : 1;
                signZ = TopLeft.Z > BottomLeft.Z ? -1 : 1;
                return new TDPoint() { X = TopRight.X + signX * (BottomLeft.X - TopLeft.X), Y = TopRight.Y + signX * (BottomLeft.Y - TopLeft.Y), Z = TopRight.Z + signX * (BottomLeft.Z - TopLeft.Z) };
            }
        }

        // Координаты центра плоскости камеры.
        public TDPoint MiddlePoint
        {
            get => new TDPoint() { X = (TopLeft.X + TopRight.X + BottomLeft.X + BottomRight.X) / 4, Y = (TopLeft.Y + TopRight.Y + BottomLeft.Y + BottomRight.Y) / 4, Z = (TopLeft.Z + TopRight.Z + BottomLeft.Z + BottomRight.Z) / 4 };
        }


        // Косинусы модификации прямой под углом 90 градусов к плоскости камеры.
        public TDPointD NormalSingleVector
        {
            get
            {
                // Переменные уравнения плоскости.
                // A = y1(z2 - z3) + y2(z3 - z1) + y3(z1 - z2)
                // B = z1(x2 - x3) + z2(x3 - x1) + z3(x1 - x2)
                // C = x1(y2 - y3) + x2(y3 - y1) + x3(y1 - y2)
                // TopLeft = 1
                // TopRight = 2
                // BottomLeft = 3

                double A, B, C, lenght;
                A = TopLeft.Y * (TopRight.Z - BottomLeft.Z) + TopRight.Y * (BottomLeft.Z - TopLeft.Z) + BottomLeft.Y * (TopLeft.Z - TopRight.Z);
                B = TopLeft.Z * (TopRight.X - BottomLeft.X) + TopRight.Z * (BottomLeft.X - TopLeft.X) + BottomLeft.Z * (TopLeft.X - TopRight.X);
                C = TopLeft.X * (TopRight.Y - BottomLeft.Y) + TopRight.X * (BottomLeft.Y - TopLeft.Y) + BottomLeft.X * (TopLeft.Y - TopRight.Y);
                lenght = Math.Sqrt(A * A + B * B + C * C);

                return new TDPointD() { X = A / lenght, Y = B / lenght, Z = C / lenght };
            }
        }

        // Координаты точки фокуса.
        public TDPoint FocusPoint
        {
            get
            {

                TDPoint middlePoint = MiddlePoint;

                double dx = FocusDotDistance * NormalSingleVector.X;
                double dy = FocusDotDistance * NormalSingleVector.Y;
                double dz = FocusDotDistance * NormalSingleVector.Z;

                return new TDPoint() { X = (long)Math.Round(middlePoint.X + dx), Y = (long)Math.Round(middlePoint.Y + dy), Z = (long)Math.Round(middlePoint.Z + dz) };
            }
        }


        public TDPointD GetOffsetStartCoordinates(long x, long y)
        {
            TDPointD result = new TDPointD();

            TDPointD top = new TDPointD(), bottom = new TDPointD();

            double tx = x / (double)(Width - 1);
            double ty = y / (double)(Height - 1);

            top.X = TopLeft.X * (1 - tx) + TopRight.X * tx;
            top.Y = TopLeft.Y * (1 - tx) + TopRight.Y * tx;
            top.Z = TopLeft.Z * (1 - tx) + TopRight.Z * tx;

            bottom.X = BottomLeft.X * (1 - tx) + BottomRight.X * tx;
            bottom.Y = BottomLeft.Y * (1 - tx) + BottomRight.Y * tx;
            bottom.Z = BottomLeft.Z * (1 - tx) + BottomRight.Z * tx;

            result.X = top.X * (1 - ty) + bottom.X * ty;
            result.Y = top.Y * (1 - ty) + bottom.Y * ty;
            result.Z = top.Z * (1 - ty) + bottom.Z * ty;

            return result;
        }
    }
}

