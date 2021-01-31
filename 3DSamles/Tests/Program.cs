using System;
using System.Windows.Media;
using StereoCanvasSamples;
using StereoCanvasSamples.Objects;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Test_Factorial();
        }

        private static void Test_NormalSingleVector(ref Camera camera)
        {
            long newPointX_MyForm = (long) Math.Round(camera.TopLeft.X + camera.NormalSingleVector.X);
        }

        public static void Test_Factorial()
        {
            Console.WriteLine(BezierSurface.Factorial(3));
        }
    }
}
