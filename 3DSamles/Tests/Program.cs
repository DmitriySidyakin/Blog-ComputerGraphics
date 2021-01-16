using System;
using System.Windows.Media;
using StereoCanvasSamples;
using StereoCanvasSamples.Objects;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {/*
            int xSSAA = 1;

            int focusDotDistance = 350;

            int renderDistance = 200;

            int cubeWidth = 200;

            DrawingVisual drawingVisual = new DrawingVisual();

            DrawingContext drawingContext = drawingVisual.RenderOpen();

            Camera camera = new Camera()
            {
                TopLeft = new TDPoint() { X = (long)Math.Ceiling(45.8) * xSSAA, Y = (long)Math.Ceiling(-92.79) * xSSAA, Z = (long)Math.Ceiling(105.36) * xSSAA },
                TopRight = new TDPoint() { X = (long)Math.Ceiling(-92.79) * xSSAA, Y = (long)Math.Ceiling(45.8) * xSSAA, Z = (long)Math.Ceiling(105.36) * xSSAA },
                BottomLeft = new TDPoint() { X = (long)Math.Ceiling(143.8) * xSSAA, Y = (long)Math.Ceiling(5.2) * xSSAA, Z = (long)Math.Ceiling(-33.23) * xSSAA },
                FocusDotDistance = focusDotDistance * xSSAA,
                MaxDistance = renderDistance * xSSAA,
                Projection = Projection.Orthogonal,
                Direction = Direction.Front
            };

            // Тестирование единичного вектора нормали.
            Test_NormalSingleVector(ref camera);

            Renderer renderer = new Renderer() { Telescoper = new Telescoper() { Camera = camera, World = new Cube(cubeWidth) }, xSSAA = xSSAA };

            renderer.Render(ref drawingContext, 2, 2);*/
            Test_Factorial();
        }

        private static void Test_NormalSingleVector(ref Camera camera)
        {
            long newPointX_MyForm = (long) Math.Ceiling(camera.TopLeft.X + camera.NormalSingleVector.X);
        }

        public static void Test_Factorial()
        {
            Console.WriteLine(BezierSurface.Factorial(3));
        }
    }
}
