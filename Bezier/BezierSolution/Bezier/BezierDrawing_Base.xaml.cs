using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace Bezier
{
    /// <summary>
    /// Логика взаимодействия для BezierDrawing_Base.xaml
    /// </summary>
    public partial class BezierDrawing_Base : Window
    {
        public BezierDrawing_Base()
        {
            InitializeComponent();
        }

        public static MainWindow ParentWnd { get; set; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ParentWnd.isVisibleBDB = false;
        }

        private void buttonB1Draw_Click(object sender, RoutedEventArgs e)
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
            DrawBezierCurve1(coords, xSSAA, width, drawingContext);

            // Рисуем опорные точки.
            DrawPoints(xSSAA, coords, 2, drawingContext);


            // Закрываем для рисования контекст рисования.
            drawingContext.Close();

            // Выполняем уменьшение изображения.
            RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                200, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

            // Применяем уменьшение изображения.
            rtb.Render(drawingVisual);

            // Преобразуем изображение в кисточку.
            ImageBrush image = new ImageBrush(rtb);

            // Отображаем рисунок, как фон холста.
            this.Canvas.Background = image;

        }

        private void DrawBezierCurve1(List<int> coords, int xSSAA, int width, DrawingContext drawingContext)
        {
            double t = 0;
            double xPrev = coords[0]/*P0*/;
            double yPrev = coords[1]/*P0*/;

            double x, y;
            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[0]/*P0*/ + t * coords[2]/*P1*/;
                y = (1 - t) * coords[1]/*P0*/ + t * coords[3]/*P1*/;

                Pen pen = new Pen(Brushes.Red, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }
        }

        [Obsolete]
        private void DrawBezierCurve1_2(List<int> coords, int xSSAA, int width, DrawingContext drawingContext, int step)
        {
            double t = 0;
            double xPrev = coords[0]/*P0*/;
            double yPrev = coords[1]/*P0*/;

            double x, y;
            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * coords[0]/*P0*/ + t * coords[2]/*P1*/;
                y = (1 - t) * coords[1]/*P0*/ + +t * coords[3]/*P1*/;

                Pen pen = new Pen(Brushes.Gray, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }
            t = 0; double tt = 0.001;
            if (step > 0)
            {
               
                xPrev = coords[0]/*P0*/;
                yPrev = coords[1]/*P0*/;
                
                int i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i ++)
                {
                    x = (1 - t) * coords[0]/*P0*/ + t * coords[2]/*P1*/;
                    y = (1 - t) * coords[1]/*P0*/ + t * coords[3]/*P1*/;

                    Pen pen = new Pen(Brushes.Red, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev = x;
                    yPrev = y;

                    tt = t;
                }
            }
            if (step == 1000) tt = 1;
            if (step == 0) tt = 0;
            drawingContext.DrawEllipse(Brushes.Blue, new Pen(Brushes.DarkViolet, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 2 * xSSAA, 2 * xSSAA);
            drawingContext.DrawEllipse(Brushes.White, new Pen(Brushes.White, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 1 * xSSAA, 1 * xSSAA);
            drawingContext.DrawText(new FormattedText("t = " + tt.ToString("N3"), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 14 * xSSAA, Brushes.Black), new Point(20 * xSSAA, 20 * xSSAA));

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
            t = 0; double tt = 0.001;
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
            t = 0; double tt = 0.001;
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
            double tt = 0.001;
            if (step > 0)
            {
                xPrev = coords[0]/*P0*/;
                yPrev = coords[1]/*P0*/;

                int i = 1;
                for (t = 0.001; t <= 1 && i <= step; t += 0.001, i++)
                {
                    x = (1 - t) * (1 - t) * (1 - t) * (1 - t) * coords[0] + 4* t * (1 - t) * (1 - t) * (1 - t) * coords[2] + 6 * t * t * (1 - t) * (1 - t) * coords[4] + 4 * t * t * t * (1 - t) * coords[6] + t * t * t * t * coords[8];
                    y = (1 - t) * (1 - t) * (1 - t) * (1 - t) * coords[1] + 4 * t * (1 - t) * (1 - t) * (1 - t) * coords[3] + 6 * t * t * (1 - t) * (1 - t) * coords[5] + 4 * t * t * t * (1 - t) * coords[7] + t * t * t * t * coords[9];

                    Pen pen = new Pen(Brushes.Red, width * xSSAA);
                    drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                    xPrev = x;
                    yPrev = y;
                    tt = t;
                }
            }
            if (step == 1000) tt = 1;
            if (step == 0) tt = 0;
            drawingContext.DrawEllipse(Brushes.Blue, new Pen(Brushes.DarkViolet, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 2 * xSSAA, 2 * xSSAA);
            drawingContext.DrawEllipse(Brushes.White, new Pen(Brushes.White, 1 * xSSAA), new Point((xPrev * xSSAA), (200 - yPrev) * xSSAA), 1 * xSSAA, 1 * xSSAA);
            drawingContext.DrawText(new FormattedText("t = " + tt.ToString("N3"), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 14 * xSSAA, Brushes.Black), new Point(20 * xSSAA, 20 * xSSAA));

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

        private static void DrawBackground(int xSSAA, DrawingContext drawingContext)
        {
            // Create a rectangle and draw it in the DrawingContext.
            Rect border = new Rect(0, 0, 400 * xSSAA, 200 * xSSAA);
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.Red, (System.Windows.Media.Pen)null, border);

            // Create a rectangle and draw it in the DrawingContext.
            Rect background = new Rect(1 * xSSAA, 1 * xSSAA, 398 * xSSAA, 198 * xSSAA);
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.White, (System.Windows.Media.Pen)null, background);
        }

        private void buttonB1Save_Click(object sender, RoutedEventArgs e)
        {

            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            // Create a rectangle and draw it in the DrawingContext.
            Rect rect = new Rect(0, 0, 400, 200);
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.LightBlue, (System.Windows.Media.Pen)null, rect);

            // Persist the drawing content.
            drawingContext.Close();


            RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                200, 96, 96, PixelFormats.Default);

            rtb.Render(drawingVisual);

            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 400, 200));

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));
            using (var fs = System.IO.File.OpenWrite("logo.png"))
            {
                pngEncoder.Save(fs);
            }
        }

        /// <summary>
        /// Метод рисует кривую Бизье 3-го порядка.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonB3Draw_Click(object sender, RoutedEventArgs e)
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
            DrawBezierCurve3(coords, xSSAA, width, drawingContext);

            // Рисуем опорные точки.
            DrawPoints(xSSAA, coords, 4, drawingContext);

            // Закрываем для рисования контекст рисования.
            drawingContext.Close();

            // Выполняем уменьшение изображения.
            RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                200, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

            // Применяем уменьшение изображения.
            rtb.Render(drawingVisual);

            // Преобразуем изображение в кисточку.
            ImageBrush image = new ImageBrush(rtb);

            // Отображаем рисунок, как фон холста.
            this.Canvas.Background = image;
        }

        private static void DrawBezierСurve2(List<int> coords, int xSSAA, int width, DrawingContext drawingContext)
        {
            int iterationCount = 1000;

            double t = 0;
            double xPrev = coords[0]/*P0*/;
            double yPrev = coords[1]/*P0*/;

            double x, y;
            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * (1 - t) * coords[0]/*P0*/ + 2 * t * (1 - t) * coords[2]/*P1*/+ t * t * coords[4]/*P2*/;
                y = (1 - t) * (1 - t) * coords[1]/*P0*/ + 2 * t * (1 - t) * coords[3]/*P1*/+ t * t * coords[5]/*P2*/;

                Pen pen = new Pen(Brushes.Red, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, 200 * xSSAA - yPrev * xSSAA), new Point(x * xSSAA, 200 * xSSAA - y * xSSAA));

                xPrev = x;
                yPrev = y;
            }


        }

        private void buttonB4Draw_Click(object sender, RoutedEventArgs e)
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
            DrawBezierCurve4(coords, xSSAA, width, drawingContext);

            // Рисуем опорные точки.
            DrawPoints(xSSAA, coords, 5, drawingContext);

            // Закрываем для рисования контекст рисования.
            drawingContext.Close();

            // Выполняем уменьшение изображения.
            RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                200, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

            // Применяем уменьшение изображения.
            rtb.Render(drawingVisual);

            // Преобразуем изображение в кисточку.
            ImageBrush image = new ImageBrush(rtb);

            // Отображаем рисунок, как фон холста.
            this.Canvas.Background = image;
        }

        private void DrawBezierCurve4(List<int> coords, int xSSAA, int width, DrawingContext drawingContext)
        {
            double t = 0;
            double xPrev = coords[0]/*P0*/;
            double yPrev = coords[1]/*P0*/;

            double x, y;
            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * (1 - t) * (1 - t) * (1 - t) * coords[0] + 4 * t * (1 - t) * (1 - t) * (1 - t) * coords[2] + 6 * t * t * (1 - t) * (1 - t) * coords[4] + 4 * t * t * t * (1 - t) * coords[6] + t * t * t * t * coords[8];
                y = (1 - t) * (1 - t) * (1 - t) * (1 - t) * coords[1] + 4 * t * (1 - t) * (1 - t) * (1 - t) * coords[3] + 6* t * t * (1 - t) * (1 - t) * coords[5] + 4 * t * t * t * (1 - t) * coords[7] + t * t * t * t * coords[9];

                Pen pen = new Pen(Brushes.Red, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200 - yPrev) * xSSAA), new Point(x * xSSAA, (200 - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }
        }

        private static void DrawBezierCurve3(List<int> coords, int xSSAA, int width, DrawingContext drawingContext)
        {

            double t = 0;
            double xPrev = coords[0]/*P0*/;
            double yPrev = coords[1]/*P0*/;

            double x, y;
            for (t = 0.001; t <= 1; t += 0.001)
            {
                x = (1 - t) * (1 - t) * (1 - t) * coords[0]/*P0*/ + 3 * t * (1 - t) * (1 - t) * coords[2]/*P1*/+ 3 * t * t * (1 - t) * coords[4]/*P2*/+ t * t * t * coords[6]/*P3*/;
                y = (1 - t) * (1 - t) * (1 - t) * coords[1]/*P0*/ + 3 * t * (1 - t) * (1 - t) * coords[3]/*P1*/+ 3 * t * t * (1 - t) * coords[5]/*P2*/+ t * t * t * coords[7]/*P3*/;

                Pen pen = new Pen(Brushes.Red, width * xSSAA);
                drawingContext.DrawLine(pen, new Point(xPrev * xSSAA, (200  - yPrev) * xSSAA), new Point(x * xSSAA, (200  - y) * xSSAA));

                xPrev = x;
                yPrev = y;
            }
        }

        private void buttonB2Draw_Click(object sender, RoutedEventArgs e)
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
            DrawBezierСurve2(coords, xSSAA, width, drawingContext);

            // Рисуем опорные точки.
            DrawPoints(xSSAA, coords, 3, drawingContext);
           
            // Закрываем для рисования контекст рисования.
            drawingContext.Close();

            // Выполняем уменьшение изображения.
            RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                200, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

            // Применяем уменьшение изображения.
            rtb.Render(drawingVisual);

            // Преобразуем изображение в кисточку.
            ImageBrush image = new ImageBrush(rtb);

            // Отображаем рисунок, как фон холста.
            this.Canvas.Background = image;
        }

        [Obsolete]
        private void DrawPoints(int xSSAA, List<int> coords, int numOfPoints, DrawingContext drawingContext)
        {
            for(int i = 0; i < numOfPoints; i++)
            {
                drawingContext.DrawEllipse(Brushes.Blue, new Pen(Brushes.Blue, 1 * xSSAA), new Point((coords[i * 2] * xSSAA), (200-coords[i * 2 + 1]) * xSSAA), 3 * xSSAA, 3 * xSSAA);
                drawingContext.DrawEllipse(Brushes.White, new Pen(Brushes.White, 1 * xSSAA), new Point((coords[i * 2] * xSSAA), (200 - coords[i * 2 + 1]) * xSSAA), 2 * xSSAA, 2 * xSSAA);
                drawingContext.DrawText(new FormattedText("P", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 14 * xSSAA, Brushes.Black), new Point((coords[i * 2] * xSSAA) - 14 * xSSAA, (200 - coords[i * 2 + 1] -14) * xSSAA));
                drawingContext.DrawText(new FormattedText(i.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 8 * xSSAA, Brushes.Black), new Point((coords[i * 2] * xSSAA) - 8 * xSSAA, (200 - coords[i * 2 + 1] - 4) * xSSAA));
            }
        }

        private void buttonB1Save_Click_1(object sender, RoutedEventArgs e)
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
            DrawBezierCurve1(coords, xSSAA, width, drawingContext);

            // Рисуем опорные точки.
            DrawPoints(xSSAA, coords, 2, drawingContext);


            // Закрываем для рисования контекст рисования.
            drawingContext.Close();

            RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                200, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

            rtb.Render(drawingVisual);

            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 400, 200));

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));
            using (var fs = System.IO.File.OpenWrite("b1.png"))
            {
                pngEncoder.Save(fs);
            }
        }

        private void buttonB2Save_Click(object sender, RoutedEventArgs e)
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
            DrawBezierСurve2(coords, xSSAA, width, drawingContext);

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
            using (var fs = System.IO.File.OpenWrite("b2.png"))
            {
                pngEncoder.Save(fs);
            }
        }

        private void buttonB3Save_Click(object sender, RoutedEventArgs e)
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
            DrawBezierCurve3(coords, xSSAA, width, drawingContext);

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
            using (var fs = System.IO.File.OpenWrite("b3.png"))
            {
                pngEncoder.Save(fs);
            }
        }

        private void buttonB4Save_Click(object sender, RoutedEventArgs e)
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
            DrawBezierCurve4(coords, xSSAA, width, drawingContext);

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
            using (var fs = System.IO.File.OpenWrite("b4.png"))
            {
                pngEncoder.Save(fs);
            }
        }

        private void buttonB4SaveAnimation_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i<=1000; i++)
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
                using (var fs = System.IO.File.OpenWrite("b4_" + i.ToString("D4") + ".png"))
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
                using (var fs = System.IO.File.OpenWrite("b3_" + i.ToString("D4") + ".png"))
                {
                    pngEncoder.Save(fs);
                }
            }
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
                using (var fs = System.IO.File.OpenWrite("b2_" + i.ToString("D4") + ".png"))
                {
                    pngEncoder.Save(fs);
                }
            }
        }

        private void buttonB1SaveAnimation_Click(object sender, RoutedEventArgs e)
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
                DrawBezierCurve1_2(coords, xSSAA, width, drawingContext, i);

                // Рисуем опорные точки.
                DrawPoints(xSSAA, coords, 2, drawingContext);

                // Закрываем для рисования контекст рисования.
                drawingContext.Close();

                RenderTargetBitmap rtb = new RenderTargetBitmap(400,
                    200, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

                rtb.Render(drawingVisual);

                var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 400, 200));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));
                using (var fs = System.IO.File.OpenWrite("b1_" + i.ToString("D4") + ".png"))
                {
                    pngEncoder.Save(fs);
                }
            }
        }
    }
}
