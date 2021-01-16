using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace StereoCanvasSamples
{
    public class Telescoper
    {
        public Camera Camera { get; set; }
        public World World { get; set; }
        public Color GetRenderedPixel(long x, long y)
        {
            Color result;

            if (Camera.Projection == Projection.Orthogonal)
            {
                result = GetOrthogonalProjectionPixelColor(x, y);
            }
            else
            {
                result = GetOpticProjectionPixelColor(x, y);
            }

            return result;
        }

        private Color GetOpticProjectionPixelColor(long x, long y)
        {
            throw new NotImplementedException();
        }



        private Color GetOrthogonalProjectionPixelColor(long x, long y)
        {
            Color result = Colors.Transparent;

            TDPointD start = Camera.GetOffsetStartCoordinates(x, y); // Координаты текущей точки, при step = 0, это координаты точки на приёмной матрице.

            TDPoint current;

            for (long step = 1; step <= Camera.MaxDistance; step++)
            {
                current = GetCurrentPoint(start, step);

                //  Если текущая точка луча ударилась, то определяем цвет от ударившуюся поверность и её окружение по пути:
                if (IsObjectInCurrentPoint(current))
                {
                    result = GetColor(current);
                    break;
                }
            }

            return result;
        }

        private bool IsObjectInCurrentPoint(TDPoint current)
        {
            return World.Matrix.Keys.Where((k) => (k.X == current.X && k.Y == current.Y && k.Z == current.Z)).Count() > 0;
        }

        private TDPoint GetCurrentPoint(TDPointD start, long step)
        {
            if (Camera.Direction == Direction.Front)
                return new TDPoint() { X = (long)Math.Ceiling(start.X - step * Camera.NormalSingleVector.X), Y = (long)Math.Ceiling(start.Y - step * Camera.NormalSingleVector.Y), Z = (long)Math.Ceiling(start.Z - step * Camera.NormalSingleVector.Z) };
            else
                return new TDPoint() { X = (long)Math.Ceiling(start.X + step * Camera.NormalSingleVector.X), Y = (long)Math.Ceiling(start.Y + step * Camera.NormalSingleVector.Y), Z = (long)Math.Ceiling(start.Z + step * Camera.NormalSingleVector.Z) };
        }

        public Color GetColor(
            TDPoint
            /* Текущая позиция врезания */ current)
        {
            CubeDot dot = null;
            TDPoint key = World.Matrix.Keys.Where((k) => k.X == current.X && k.Y == current.Y && k.Z == current.Z).First();
            if (World.Matrix.TryGetValue(key, out dot))
                return dot.Color;
            else return Colors.Transparent;
        }
    }
}
