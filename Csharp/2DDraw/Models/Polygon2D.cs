using System.Windows.Shapes;

namespace _2DDraw.Models
{
    public sealed class Polygon2D
    {
        public List<Line> Lines { get; set; } = new List<Line>();

        public Line? LastLine
        {
            get => lastLine;
            set
            {
                lastLine = value;
                if (lastLine != null)
                {
                    IsFinished = true;
                }
            }
        }

        public bool IsFinished { get; set; } = false;

        private Line? lastLine;
    }
}
