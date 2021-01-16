using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace StereoCanvasSamples
{
    /// <summary>
    /// Внимательно!: При хранении объекта в дискретном виде программа требует много памяти.
    /// </summary>
    public interface World
    {
        Dictionary<TDPoint, CubeDot> Matrix { get; set; }
    }
}
