using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace _2DDraw.Models
{
    public sealed class DrawingOperation
    {
        public Line? Line { get; set; }

        public Polygon2D? Polygon { get; set; }

        public DrawingAction Action { get; set; }
    }

    public enum DrawingAction
    {
        DrawLine,
        FinishPolygon
    }
}

