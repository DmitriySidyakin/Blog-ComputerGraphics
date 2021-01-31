using StereoCanvasSamples.Objects;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StereoCanvasSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonCube_Click(object sender, RoutedEventArgs e)
        {
            int xSSAA = 2;
            DrawingVisual drawingVisual;
            DrawingContext drawingContext;
            NewCanvas(out drawingVisual, out drawingContext);
            DrawCube(xSSAA, drawingContext);
            RenderTargetBitmap rtb = DrawToForm(xSSAA, drawingVisual);
            SaveToFile(rtb);
        }

        private static void SaveToFile(RenderTargetBitmap rtb, string fileName = "cube.png")
        {
            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 52, 52));

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));
            using (var fs = System.IO.File.OpenWrite(fileName))
            {
                pngEncoder.Save(fs);
            }
        }

        private RenderTargetBitmap DrawToForm(int xSSAA, DrawingVisual drawingVisual)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap(52,
                            52, 96 / xSSAA, 96 / xSSAA, PixelFormats.Default);

            rtb.Render(drawingVisual);

            // Преобразуем изображение в кисточку.
            ImageBrush image = new ImageBrush(rtb);

            // Отображаем рисунок, как фон холста.
            this.PreviewCanvas.Background = image;
            return rtb;
        }

        private void DrawCube(int xSSAA, DrawingContext drawingContext)
        {
            // Рисуем фон
            DrawBackground(xSSAA, drawingContext);

            // Рисуем кривую Безье.
            DrawCube(xSSAA, drawingContext, (int)0);

            // Закрываем для рисования контекст рисования.
            drawingContext.Close();
        }

        private static void NewCanvas(out DrawingVisual drawingVisual, out DrawingContext drawingContext)
        {
            drawingVisual = new DrawingVisual();

            // Открываем для рисования контекст.
            drawingContext = drawingVisual.RenderOpen();
        }

        private void DrawCube(int xSSAA, DrawingContext drawingContext, int step)
        {
            long focusDotDistance = 36;

            long renderDistance = 70;

            long cubeWidth = 20 * xSSAA;

            Camera camera = SetCamera(xSSAA, focusDotDistance, renderDistance);

            Renderer renderer = new Renderer() { Telescoper = new Telescoper() { Camera = camera, World = new Cube(cubeWidth, xSSAA)} };

            renderer.Render(ref drawingContext, 3, 3);
        }

        private static Camera SetCamera(int xSSAA, long focusDotDistance, long renderDistance)
        {
            Camera camera = new Camera()
            {
                TopLeft = new TDPoint() { X = (long)(22.67766952966369) * xSSAA, Y = (long)(12.67766952966369) * xSSAA, Z = (long)(42.42640687119285) * xSSAA },
                TopRight = new TDPoint() { X = (long)(-12.67766952966369) * xSSAA, Y = (long)(-22.67766952966369) * xSSAA, Z = (long)(42.42640687119285) * xSSAA },
                BottomLeft = new TDPoint() { X = (long)(47.67766952966369) * xSSAA, Y = (long)(-12.32233047033631) * xSSAA, Z = (long)(7.071067811865478) * xSSAA },
                FocusDotDistance = focusDotDistance * xSSAA,
                MaxDistance = renderDistance * xSSAA,
                Projection = Projection.Orthogonal,
                Direction = Direction.Front,
            };

            return camera;
        }

        private void DrawBackground(int xSSAA, DrawingContext drawingContext)
        {
            Rect border = new Rect(0, 0, 52 * xSSAA, 52 * xSSAA);
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.Red, (System.Windows.Media.Pen)null, border);

            Rect background = new Rect(xSSAA, xSSAA, 50 * xSSAA, 50 * xSSAA);
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.White, (System.Windows.Media.Pen)null, background);
        }

        private void buttonBezierSurfaces_Click(object sender, RoutedEventArgs e)
        {
            int xSSAA = 2;
            DrawingVisual drawingVisual;
            DrawingContext drawingContext;
            NewCanvas(out drawingVisual, out drawingContext);
            DrawBezierSurface(xSSAA, drawingContext);
            RenderTargetBitmap rtb = DrawToForm(xSSAA, drawingVisual);
            SaveToFile(rtb, "BezierSurface.png");
        }

        private void DrawBezierSurface(int xSSAA, DrawingContext drawingContext)
        {
            // Рисуем фон
            DrawBackground(xSSAA, drawingContext);

            // Рисуем кривую Безье.
            DrawBezierSurfaceOnStereoCanvas(xSSAA, drawingContext);

            // Закрываем для рисования контекст рисования.
            drawingContext.Close();
        }

        private void DrawBezierSurfaceOnStereoCanvas(int xSSAA, DrawingContext drawingContext)
        {
            long focusDotDistance = 36;

            long renderDistance = 36;

            long cubeWidth = 20 * xSSAA;

            Camera camera = SetCamera(xSSAA,  focusDotDistance,  renderDistance);

           Renderer renderer = new Renderer() { Telescoper = new Telescoper() { Camera = camera, World = new BezierSurface(xSSAA) { } } };

            renderer.Render(ref drawingContext, 3, 3);
        }

    }
}
