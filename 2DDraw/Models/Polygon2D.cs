using System.Windows.Shapes;

namespace _2DDraw.Models
{
    public sealed class Polygon2D
    {
        public List<Line> Lines { get; set; } = new List<Line>();

        public bool Finished { get; set; }
    }
}
