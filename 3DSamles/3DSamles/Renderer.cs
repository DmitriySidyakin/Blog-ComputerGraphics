using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StereoCanvasSamples
{
    public class Renderer
    {
        public Telescoper Telescoper { get; set; }

        public void Render(ref DrawingContext drawingContext, int startDx, int startDy)
        {
            Color[,] img = new Color[Telescoper.Camera.Width, Telescoper.Camera.Height];
            for (long i = 0; i < Telescoper.Camera.Width; i++)
            {
                for (long j = 0; j < Telescoper.Camera.Height; j++)
                {
                    DrawOn(i, j, ref img);
                }
            }
            DrawTo(ref drawingContext, in img, startDx, startDy);
        }

        private void DrawOn(long i, long j, ref Color[,] img)
        {
            img[i, j] = Telescoper.GetRenderedPixel(i, j);
        }

        private void DrawTo(ref DrawingContext drawingContext, in Color[,] img, int startDx, int startDy)
        {
            if (Telescoper.Camera.Width <= int.MaxValue && Telescoper.Camera.Height <= int.MaxValue)
            {
                for (int x = 0; x < Telescoper.Camera.Width; x++)
                {
                    for (int y = 0; y < Telescoper.Camera.Height; y++)
                    {
                        DrawPoint(ref drawingContext, in img, x, y, startDx, startDy);
                    }
                }
            }
            else
            {
                throw new Exception("Size of image were overflowed!");
            }
        }

        private void DrawPoint(ref DrawingContext drawingContext, in Color[,] img, int x, int y, int startDx, int startDy)
        {
            Brush brush = new SolidColorBrush(Color.FromArgb(img[x, y].A, img[x, y].R, img[x, y].G, img[x, y].B));
            Pen pen = new Pen(brush, 1);
            Rect rect = new Rect(
                    (int)x + startDx,
                    (int)y + startDy,
                    1,
                    1);
            drawingContext.DrawRectangle(brush, pen, rect);
        }
    }
}
