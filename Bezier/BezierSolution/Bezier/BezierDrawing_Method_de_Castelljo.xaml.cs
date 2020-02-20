using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bezier
{
    /// <summary>
    /// Логика взаимодействия для BezierDrawing_Method_de_Castelljo.xaml
    /// </summary>
    public partial class BezierDrawing_Method_de_Castelljo : Window
    {
        public BezierDrawing_Method_de_Castelljo()
        {
            InitializeComponent();
        }
        public static MainWindow ParentWnd { get; set; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ParentWnd.isVisibleBDMC = false;
        }

        /// <summary>
        /// Метод для получения списка координат точек.
        /// </summary>
        /// <returns>Список координат точек, полученный из строки.</returns>
        private List<int> parseCoords()
        {
            List<Int32> result = new List<Int32>();

            String[] coords = this.textBox.Text.Split(',');
            foreach (var xy in coords)
            {
                String[] xy_coord = xy.Trim().Split(' ');

                result.Add(Int32.Parse(xy_coord[0]));
                result.Add(Int32.Parse(xy_coord[1]));
            }

            return result;
        }

        private void buttonB2SaveAnimation_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= 1000; i++)
            {
                // Загружаем координаты точек.
                List<Int32> coords = parseCoords();

                // Уровень сглаживания зубцов.
                int xSSAA = 16;

                // Толщина кривой Безье.
                int width = 2;

                /* Рисуем кривую */

                DrawingVisual drawingVisual = new DrawingVisual();

                // Открываем для рисования контекст.
                DrawingContext drawingContext = drawingVisual.RenderOpen();

                // Рисуем фон
                DrawBackground(xSSAA, drawingContext);

                // Рисуем кривую Безье.
                DrawBezierCurve2_2(coords, xSSAA, width, drawingContext, i);

                // Рисуем опорные точки.
                DrawPoints(xSSAA, coords, 3, drawingContext);

                // Закрываем для рисования контекст рисования.
                drawingContext.Close();

                RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                    200, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

                rtb.Render(drawingVisual);

                var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 400, 200));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));
                using (var fs = System.IO.File.OpenWrite("b2dc_" + i.ToString("D4") + ".png"))
                {
                    pngEncoder.Save(fs);
                }
            }
        }

        private void buttonB3SaveAnimation_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= 1000; i++)
            {
                // Загружаем координаты точек.
                List<Int32> coords = parseCoords();

                // Уровень сглаживания зубцов.
                int xSSAA = 16;

                // Толщина кривой Безье.
                int width = 2;

                /* Рисуем кривую */

                DrawingVisual drawingVisual = new DrawingVisual();

                // Открываем для рисования контекст.
                DrawingContext drawingContext = drawingVisual.RenderOpen();

                // Рисуем фон
                DrawBackground(xSSAA, drawingContext);

                // Рисуем кривую Безье.
                DrawBezierCurve3_2(coords, xSSAA, width, drawingContext, i);

                // Рисуем опорные точки.
                DrawPoints(xSSAA, coords, 4, drawingContext);

                // Закрываем для рисования контекст рисования.
                drawingContext.Close();

                RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                    200, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

                rtb.Render(drawingVisual);

                var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 400, 200));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));
                using (var fs = System.IO.File.OpenWrite("b3dc_" + i.ToString("D4") + ".png"))
                {
                    pngEncoder.Save(fs);
                }
            }
        }

        private void buttonB4SaveAnimation_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= 1000; i++)
            {
                // Загружаем координаты точек.
                List<Int32> coords = parseCoords();

                // Уровень сглаживания зубцов.
                int xSSAA = 16;

                // Толщина кривой Безье.
                int width = 2;

                /* Рисуем кривую */

                DrawingVisual drawingVisual = new DrawingVisual();

                // Открываем для рисования контекст.
                DrawingContext drawingContext = drawingVisual.RenderOpen();

                // Рисуем фон
                DrawBackground(xSSAA, drawingContext);

                // Рисуем кривую Безье.
                DrawBezierCurve4_2(coords, xSSAA, width, drawingContext, i);

                // Рисуем опорные точки.
                DrawPoints(xSSAA, coords, 5, drawingContext);

                // Закрываем для рисования контекст рисования.
                drawingContext.Close();

                RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                    200, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

                rtb.Render(drawingVisual);

                var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 400, 200));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));
                using (var fs = System.IO.File.OpenWrite("b4dc_" + i.ToString("D4") + ".png"))
                {
                    pngEncoder.Save(fs);
                }
            }
        }


        [Obsolete]
        private void DrawBezierCurve2_2(List<int> coords, int xSSAA, int width, DrawingContext drawingContext, int step)
        {
            double t = 0;
            double xPrev = coords[0]/*P0*/;
            double yPrev = coords[1]/*P0*/;

            double x, y;
            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * (1 - t) * coords[0]/*P0*/ + 2 * t * (1 - t) * coords[2]/*P1*/+ t * t * coords[4]/*P2*/;
                y = (1 - t) * (1 - t) * coords[1]/*P0*/ + 2 * t * (1 - t) * coords[3]/*P1*/+ t * t * coords[5]/*P2*/;

                Pen pen = new Pen(Brushes.Gray, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }

            t = 0;
            xPrev = coords[0]/*P0*/;
            yPrev = coords[1]/*P0*/;

            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[0]/*P0*/ + t * coords[2]/*P1*/;
                y = (1 - t) * coords[1]/*P0*/ + t * coords[3]/*P1*/;

                Pen pen = new Pen(Brushes.Orange, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }

            t = 0;
            xPrev = coords[2]/*P0*/;
            yPrev = coords[3]/*P0*/;

            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[2]/*P0*/ + t * coords[4]/*P1*/;
                y = (1 - t) * coords[3]/*P0*/ + t * coords[5]/*P1*/;

                Pen pen = new Pen(Brushes.Orange, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }

            t = 0;
            double tt = 0;
            if (step > 0)
            {

                xPrev = coords[0]/*P0*/;
                yPrev = coords[1]/*P0*/;

                int i = 1;

                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * (1 - t) * coords[0]/*P0*/ + 2 * t * (1 - t) * coords[2]/*P1*/+ t * t * coords[4]/*P2*/;
                    y = (1 - t) * (1 - t) * coords[1]/*P0*/ + 2 * t * (1 - t) * coords[3]/*P1*/+ t * t * coords[5]/*P2*/;

                    Pen pen = new Pen(Brushes.Red, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev = x;
                    yPrev = y;
                    tt = t;
                }

                t = 0;
                double xPrev1 = coords[0]/*P0*/;
                double yPrev1 = coords[1]/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * coords[0]/*P0*/ + t * coords[2]/*P1*/;
                    y = (1 - t) * coords[1]/*P0*/ + t * coords[3]/*P1*/;


                    xPrev1 = x;
                    yPrev1 = y;
                }
                t = 0;
                double xPrev2 = coords[2]/*P0*/;
                double yPrev2 = coords[3]/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * coords[2]/*P0*/ + t * coords[4]/*P1*/;
                    y = (1 - t) * coords[3]/*P0*/ + t * coords[5]/*P1*/;


                    xPrev2 = x;
                    yPrev2 = y;
                }

                t = 0;
                double xPrev12 = xPrev1/*P0*/;
                double yPrev12 = yPrev1/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev1/*P0*/ + t * xPrev2/*P1*/;
                    y = (1 - t) * yPrev1/*P0*/ + t * yPrev2/*P1*/;

                    Pen pen = new Pen(Brushes.LightBlue, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev12 * xSSAA, (200 - yPrev12) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev12 = x;
                    yPrev12 = y;
                }

                t = 0;
                xPrev12 = xPrev1/*P0*/;
                yPrev12 = yPrev1/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev1/*P0*/ + t * xPrev2/*P1*/;
                    y = (1 - t) * yPrev1/*P0*/ + t * yPrev2/*P1*/;


                    xPrev12 = x;
                    yPrev12 = y;
                }
            }

            if (step == 1000) tt = 1;
            if (step == 0) tt = 0;

            drawingContext.DrawEllipse(Brushes.Blue, new Pen(Brushes.DarkViolet, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 2 * xSSAA, 2 * xSSAA);
            drawingContext.DrawEllipse(Brushes.White, new Pen(Brushes.White, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 1 * xSSAA, 1 * xSSAA);
            drawingContext.DrawText(new FormattedText("t = " + tt.ToString("N3"), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 14 * xSSAA, Brushes.Black), new Point(20 * xSSAA, 20 * xSSAA));
        }

        [Obsolete]
        private void DrawBezierCurve3_2(List<int> coords, int xSSAA, int width, DrawingContext drawingContext, int step)
        {
            double t = 0;
            double xPrev = coords[0]/*P0*/;
            double yPrev = coords[1]/*P0*/;

            double x, y;
            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * (1 - t) * (1 - t) * coords[0]/*P0*/ + 3 * t * (1 - t) * (1 - t) * coords[2]/*P1*/+ 3 * t * t * (1 - t) * coords[4]/*P2*/+ t * t * t * coords[6]/*P3*/;
                y = (1 - t) * (1 - t) * (1 - t) * coords[1]/*P0*/ + 3 * t * (1 - t) * (1 - t) * coords[3]/*P1*/+ 3 * t * t * (1 - t) * coords[5]/*P2*/+ t * t * t * coords[7]/*P3*/;

                Pen pen = new Pen(Brushes.Gray, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }

            t = 0;
            xPrev = coords[0]/*P0*/;
            yPrev = coords[1]/*P0*/;

            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[0]/*P0*/ + t * coords[2]/*P1*/;
                y = (1 - t) * coords[1]/*P0*/ + t * coords[3]/*P1*/;

                Pen pen = new Pen(Brushes.Orange, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }

            t = 0;
            xPrev = coords[2]/*P0*/;
            yPrev = coords[3]/*P0*/;

            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[2]/*P0*/ + t * coords[4]/*P1*/;
                y = (1 - t) * coords[3]/*P0*/ + t * coords[5]/*P1*/;

                Pen pen = new Pen(Brushes.Orange, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }
            t = 0;
            xPrev = coords[4]/*P0*/;
            yPrev = coords[5]/*P0*/;

            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[4]/*P0*/ + t * coords[6]/*P1*/;
                y = (1 - t) * coords[5]/*P0*/ + t * coords[7]/*P1*/;

                Pen pen = new Pen(Brushes.Orange, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }

            t = 0;
            double tt = 0;
            if (step > 0)
            {

                xPrev = coords[0]/*P0*/;
                yPrev = coords[1]/*P0*/;

                int i = 1;

                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * (1 - t) * (1 - t) * coords[0]/*P0*/ + 3 * t * (1 - t) * (1 - t) * coords[2]/*P1*/+ 3 * t * t * (1 - t) * coords[4]/*P2*/+ t * t * t * coords[6]/*P3*/;
                    y = (1 - t) * (1 - t) * (1 - t) * coords[1]/*P0*/ + 3 * t * (1 - t) * (1 - t) * coords[3]/*P1*/+ 3 * t * t * (1 - t) * coords[5]/*P2*/+ t * t * t * coords[7]/*P3*/;

                    Pen pen = new Pen(Brushes.Red, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev = x;
                    yPrev = y;
                    tt = t;
                }

                t = 0;
                double xPrev1 = coords[0]/*P0*/;
                double yPrev1 = coords[1]/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * coords[0]/*P0*/ + t * coords[2]/*P1*/;
                    y = (1 - t) * coords[1]/*P0*/ + t * coords[3]/*P1*/;


                    xPrev1 = x;
                    yPrev1 = y;
                }
                t = 0;
                double xPrev2 = coords[2]/*P0*/;
                double yPrev2 = coords[3]/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * coords[2]/*P0*/ + t * coords[4]/*P1*/;
                    y = (1 - t) * coords[3]/*P0*/ + t * coords[5]/*P1*/;


                    xPrev2 = x;
                    yPrev2 = y;
                }
                t = 0;
                double xPrev3 = coords[4]/*P0*/;
                double yPrev3 = coords[5]/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * coords[4]/*P0*/ + t * coords[6]/*P1*/;
                    y = (1 - t) * coords[5]/*P0*/ + t * coords[7]/*P1*/;


                    xPrev3 = x;
                    yPrev3 = y;
                }

                t = 0;
                double xPrev12 = xPrev1/*P0*/;
                double yPrev12 = yPrev1/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev1/*P0*/ + t * xPrev2/*P1*/;
                    y = (1 - t) * yPrev1/*P0*/ + t * yPrev2/*P1*/;

                    Pen pen = new Pen(Brushes.LightBlue, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev12 * xSSAA, (200 - yPrev12) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev12 = x;
                    yPrev12 = y;
                }
                t = 0;
                double xPrev22 = xPrev2/*P0*/;
                double yPrev22 = yPrev2/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev2/*P0*/ + t * xPrev3/*P1*/;
                    y = (1 - t) * yPrev2/*P0*/ + t * yPrev3/*P1*/;

                    Pen pen = new Pen(Brushes.LightBlue, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev22 * xSSAA, (200 - yPrev22) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev22 = x;
                    yPrev22 = y;
                }

                t = 0;
                xPrev12 = xPrev1/*P0*/;
                yPrev12 = yPrev1/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev1/*P0*/ + t * xPrev2/*P1*/;
                    y = (1 - t) * yPrev1/*P0*/ + t * yPrev2/*P1*/;


                    xPrev12 = x;
                    yPrev12 = y;
                }
                t = 0;
                xPrev22 = xPrev2/*P0*/;
                yPrev22 = yPrev2/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev2/*P0*/ + t * xPrev3/*P1*/;
                    y = (1 - t) * yPrev2/*P0*/ + t * yPrev3/*P1*/;

                    xPrev22 = x;
                    yPrev22 = y;
                }


                /***********/
                t = 0;
                double xPrev123 = xPrev12/*P0*/;
                double yPrev123 = yPrev12/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev12/*P0*/ + t * xPrev22/*P1*/;
                    y = (1 - t) * yPrev12/*P0*/ + t * yPrev22/*P1*/;

                    Pen pen = new Pen(Brushes.Coral, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev123 * xSSAA, (200 - yPrev123) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev123 = x;
                    yPrev123 = y;
                }
                t = 0;
                xPrev123 = xPrev12/*P0*/;
                yPrev123 = yPrev12/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev12/*P0*/ + t * xPrev22/*P1*/;
                    y = (1 - t) * yPrev12/*P0*/ + t * yPrev22/*P1*/;

                    xPrev123 = x;
                    yPrev123 = y;
                }
            }

            if (step == 1000) tt = 1;
            if (step == 0) tt = 0;

            drawingContext.DrawEllipse(Brushes.Blue, new Pen(Brushes.DarkViolet, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 2 * xSSAA, 2 * xSSAA);
            drawingContext.DrawEllipse(Brushes.White, new Pen(Brushes.White, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 1 * xSSAA, 1 * xSSAA);
            drawingContext.DrawText(new FormattedText("t = " + tt.ToString("N3"), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 14 * xSSAA, Brushes.Black), new Point(20 * xSSAA, 20 * xSSAA));
        }

        [Obsolete]
        private void DrawBezierCurve4_2(List<int> coords, int xSSAA, int width, DrawingContext drawingContext, int step)
        {
            double t = 0;
            double xPrev = coords[0]/*P0*/;
            double yPrev = coords[1]/*P0*/;

            double x, y;
            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * (1 - t) * (1 - t) * (1 - t) * coords[0] + 4 * t * (1 - t) * (1 - t) * (1 - t) * coords[2] + 6 * t * t * (1 - t) * (1 - t) * coords[4] + 4 * t * t * t * (1 - t) * coords[6] + t * t * t * t * coords[8];
                y = (1 - t) * (1 - t) * (1 - t) * (1 - t) * coords[1] + 4 * t * (1 - t) * (1 - t) * (1 - t) * coords[3] + 6 * t * t * (1 - t) * (1 - t) * coords[5] + 4 * t * t * t * (1 - t) * coords[7] + t * t * t * t * coords[9];

                Pen pen = new Pen(Brushes.Gray, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }

            t = 0;
            xPrev = coords[0]/*P0*/;
            yPrev = coords[1]/*P0*/;

            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[0]/*P0*/ + t * coords[2]/*P1*/;
                y = (1 - t) * coords[1]/*P0*/ + t * coords[3]/*P1*/;

                Pen pen = new Pen(Brushes.Orange, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }
            t = 0;
            xPrev = coords[2]/*P0*/;
            yPrev = coords[3]/*P0*/;

            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[2]/*P0*/ + t * coords[4]/*P1*/;
                y = (1 - t) * coords[3]/*P0*/ + t * coords[5]/*P1*/;

                Pen pen = new Pen(Brushes.Orange, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }
            t = 0;
            xPrev = coords[4]/*P0*/;
            yPrev = coords[5]/*P0*/;

            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[4]/*P0*/ + t * coords[6]/*P1*/;
                y = (1 - t) * coords[5]/*P0*/ + t * coords[7]/*P1*/;

                Pen pen = new Pen(Brushes.Orange, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }
            t = 0;
            xPrev = coords[6]/*P0*/;
            yPrev = coords[7]/*P0*/;
            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[6]/*P0*/ + t * coords[8]/*P1*/;
                y = (1 - t) * coords[7]/*P0*/ + t * coords[9]/*P1*/;

                Pen pen = new Pen(Brushes.Orange, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }




            t = 0;
            double tt = 0;
            if (step > 0)
            {

                xPrev = coords[0]/*P0*/;
                yPrev = coords[1]/*P0*/;

                int i = 1;
                
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * (1 - t) * (1 - t) * (1 - t) * coords[0] + 4 * t * (1 - t) * (1 - t) * (1 - t) * coords[2] + 6 * t * t * (1 - t) * (1 - t) * coords[4] + 4 * t * t * t * (1 - t) * coords[6] + t * t * t * t * coords[8];
                    y = (1 - t) * (1 - t) * (1 - t) * (1 - t) * coords[1] + 4 * t * (1 - t) * (1 - t) * (1 - t) * coords[3] + 6 * t * t * (1 - t) * (1 - t) * coords[5] + 4 * t * t * t * (1 - t) * coords[7] + t * t * t * t * coords[9];

                    Pen pen = new Pen(Brushes.Red, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev = x;
                    yPrev = y;
                    tt = t;
                }

                t = 0;
                double xPrev1 = coords[0]/*P0*/;
                double yPrev1 = coords[1]/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * coords[0]/*P0*/ + t * coords[2]/*P1*/;
                    y = (1 - t) * coords[1]/*P0*/ + t * coords[3]/*P1*/;

                 
                    xPrev1 = x;
                    yPrev1 = y;
                }
                t = 0;
                double xPrev2 = coords[2]/*P0*/;
                double yPrev2 = coords[3]/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * coords[2]/*P0*/ + t * coords[4]/*P1*/;
                    y = (1 - t) * coords[3]/*P0*/ + t * coords[5]/*P1*/;


                    xPrev2 = x;
                    yPrev2 = y;
                }
                t = 0;
                double xPrev3 = coords[4]/*P0*/;
                double yPrev3 = coords[5]/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * coords[4]/*P0*/ + t * coords[6]/*P1*/;
                    y = (1 - t) * coords[5]/*P0*/ + t * coords[7]/*P1*/;

  
                    xPrev3 = x;
                    yPrev3 = y;
                }
                t = 0;
                i = 1;
                double xPrev4 = coords[6]/*P0*/;
                double yPrev4 = coords[7]/*P0*/;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * coords[6]/*P0*/ + t * coords[8]/*P1*/;
                    y = (1 - t) * coords[7]/*P0*/ + t * coords[9]/*P1*/;

                    xPrev4 = x;
                    yPrev4 = y;
                }

                t = 0;
                double xPrev12 = xPrev1/*P0*/;
                double yPrev12 = yPrev1/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev1/*P0*/ + t * xPrev2/*P1*/;
                    y = (1 - t) * yPrev1/*P0*/ + t * yPrev2/*P1*/;

                    Pen pen = new Pen(Brushes.LightBlue, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev12 * xSSAA, (200 - yPrev12) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev12 = x;
                    yPrev12 = y;
                }
                t = 0;
                double xPrev22 = xPrev2/*P0*/;
                double yPrev22 = yPrev2/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev2/*P0*/ + t * xPrev3/*P1*/;
                    y = (1 - t) * yPrev2/*P0*/ + t * yPrev3/*P1*/;

                    Pen pen = new Pen(Brushes.LightBlue, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev22 * xSSAA, (200 - yPrev22) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev22 = x;
                    yPrev22 = y;
                }
                t = 0;
                double xPrev32 = xPrev3/*P0*/;
                double yPrev32 = yPrev3/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001)
                {
                    x = (1 - t) * xPrev3/*P0*/ + t * xPrev4/*P1*/;
                    y = (1 - t) * yPrev3/*P0*/ + t * yPrev4/*P1*/;

                    Pen pen = new Pen(Brushes.LightBlue, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev32 * xSSAA, (200 - yPrev32) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev32 = x;
                    yPrev32 = y;
                }

                t = 0;
                xPrev12 = xPrev1/*P0*/;
                yPrev12 = yPrev1/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev1/*P0*/ + t * xPrev2/*P1*/;
                    y = (1 - t) * yPrev1/*P0*/ + t * yPrev2/*P1*/;

       
                    xPrev12 = x;
                    yPrev12 = y;
                }
                t = 0;
                xPrev22 = xPrev2/*P0*/;
                yPrev22 = yPrev2/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev2/*P0*/ + t * xPrev3/*P1*/;
                    y = (1 - t) * yPrev2/*P0*/ + t * yPrev3/*P1*/;

                    xPrev22 = x;
                    yPrev22 = y;
                }
                t = 0;
                xPrev32 = xPrev3/*P0*/;
                yPrev32 = yPrev3/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev3/*P0*/ + t * xPrev4/*P1*/;
                    y = (1 - t) * yPrev3/*P0*/ + t * yPrev4/*P1*/;

                    xPrev32 = x;
                    yPrev32 = y;
                }

                /***********/
                t = 0;
                double xPrev123 = xPrev12/*P0*/;
                double yPrev123 = yPrev12/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev12/*P0*/ + t * xPrev22/*P1*/;
                    y = (1 - t) * yPrev12/*P0*/ + t * yPrev22/*P1*/;

                    Pen pen = new Pen(Brushes.Coral, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev123 * xSSAA, (200 - yPrev123) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev123 = x;
                    yPrev123 = y;
                }
                t = 0;
                double xPrev223 = xPrev22/*P0*/;
                double yPrev223 = yPrev22/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev22/*P0*/ + t * xPrev32/*P1*/;
                    y = (1 - t) * yPrev22/*P0*/ + t * yPrev32/*P1*/;

                    Pen pen = new Pen(Brushes.Coral, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev223 * xSSAA, (200 - yPrev223) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev223 = x;
                    yPrev223 = y;
                }

                t = 0;
                xPrev123 = xPrev12/*P0*/;
                yPrev123 = yPrev12/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev12/*P0*/ + t * xPrev22/*P1*/;
                    y = (1 - t) * yPrev12/*P0*/ + t * yPrev22/*P1*/;

                    xPrev123 = x;
                    yPrev123 = y;
                }
                t = 0;
                xPrev223 = xPrev22/*P0*/;
                yPrev223 = yPrev22/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev22/*P0*/ + t * xPrev32/*P1*/;
                    y = (1 - t) * yPrev22/*P0*/ + t * yPrev32/*P1*/;

                    xPrev223 = x;
                    yPrev223 = y;
                }
                /********************************------------*/
                t = 0;
                double xPrev1234 = xPrev123/*P0*/;
                double yPrev1234 = yPrev123/*P0*/;
                i = 1;
                for (t = 0.001; t <= 1; t += 0.001, i++)
                {
                    x = (1 - t) * xPrev123/*P0*/ + t * xPrev223/*P1*/;
                    y = (1 - t) * yPrev123/*P0*/ + t * yPrev223/*P1*/;

                    Pen pen = new Pen(Brushes.DarkMagenta, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev1234 * xSSAA, (200 - yPrev1234) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev1234 = x;
                    yPrev1234 = y;
                }




                t -= 0.001;
            }

            if (step == 1000) tt = 1;
            if (step == 0) tt = 0;

            drawingContext.DrawEllipse(Brushes.Blue, new Pen(Brushes.DarkViolet, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 2 * xSSAA, 2 * xSSAA);
            drawingContext.DrawEllipse(Brushes.White, new Pen(Brushes.White, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 1 * xSSAA, 1 * xSSAA);
            drawingContext.DrawText(new FormattedText("t = " + tt.ToString("N3"), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 14 * xSSAA, Brushes.Black), new Point(20 * xSSAA, 20 * xSSAA));

        }

        private static void DrawBackground(int xSSAA, DrawingContext drawingContext)
        {
            // Create a rectangle and draw it in the DrawingContext.
            Rect border = new Rect(0, 0, 400 * xSSAA, 200 * xSSAA);
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.Red, (System.Windows.Media.Pen)null, border);

            // Create a rectangle and draw it in the DrawingContext.
            Rect background = new Rect(1 * xSSAA, 1 * xSSAA, 398 * xSSAA, 198 * xSSAA);
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.White, (System.Windows.Media.Pen)null, background);
        }

        [Obsolete]
        private void DrawPoints(int xSSAA, List<int> coords, int numOfPoints, DrawingContext drawingContext)
        {
            for (int i = 0; i < numOfPoints; i++)
            {
                drawingContext.DrawEllipse(Brushes.Blue, new Pen(Brushes.Blue, 1 * xSSAA), new Point((coords[i * 2] * xSSAA), (200 - coords[i * 2 + 1]) * xSSAA), 3 * xSSAA, 3 * xSSAA);
                drawingContext.DrawEllipse(Brushes.White, new Pen(Brushes.White, 1 * xSSAA), new Point((coords[i * 2] * xSSAA), (200 - coords[i * 2 + 1]) * xSSAA), 2 * xSSAA, 2 * xSSAA);
                drawingContext.DrawText(new FormattedText("P", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 14 * xSSAA, Brushes.Black), new Point((coords[i * 2] * xSSAA) - 14 * xSSAA, (200 - coords[i * 2 + 1] - 14) * xSSAA));
                drawingContext.DrawText(new FormattedText(i.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 8 * xSSAA, Brushes.Black), new Point((coords[i * 2] * xSSAA) - 8 * xSSAA, (200 - coords[i * 2 + 1] - 4) * xSSAA));
            }
        }
    }
}
