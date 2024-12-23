using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using _2DDraw.Models;

namespace _2DDraw.ViewModels
{
    public sealed class CanvasViewModel : Screen
    {
        public void MouseMove_Canvas(Canvas canvas, MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(canvas);

            if (lastMousePosition == currentPos)
            {
                return;
            }

            if (isCurrentlyDrawing)
            {
                ShowPreview(canvas, e.GetPosition(canvas));
                lastMousePosition = currentPos;
            }
        }

        public void LeftMouseDown_Canvas(Canvas canvas, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                MouseDoubleClick_Canvas(canvas, e);
                e.Handled = true;
                return;
            }

            if (!isCurrentlyDrawing)
            {
                position1 = e.GetPosition(canvas);
                isCurrentlyDrawing = true;
                currentPolygon = new Polygon2D();
            }
            else
            {
                position2 = e.GetPosition(canvas);
                currentPolygon?.Lines.Add(DrawLine(canvas, position1, position2));
                isCurrentlyDrawing = false;
            }
        }

        public void RightMouseDown_Canvas(Canvas canvas, MouseEventArgs e)
        {
            if (isCurrentlyDrawing)
            {
                ClearPreview(canvas);
                isCurrentlyDrawing = false;
            }
        }

        public void MouseDoubleClick_Canvas(Canvas canvas, MouseEventArgs e)
        {
            if (currentPolygon != null)
            {
                currentPolygon.Finished = true;
                polygons.Add(currentPolygon);

                currentPolygon = new Polygon2D();
            }
        }

        private Line DrawLine(Canvas canvas, Point pos1, Point pos2)
        {
            Line line = new Line
            {
                X1 = pos1.X,
                Y1 = pos1.Y,
                X2 = pos2.X,
                Y2 = pos2.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            canvas?.Children.Add(line);
            Refresh();

            return line;
        }

        private void ShowPreview(Canvas canvas, Point currentMousePosition)
        {
            ClearPreview(canvas);
            previewLine = DrawLine(canvas, position1, currentMousePosition);
        }

        private void ClearPreview(Canvas canvas)
        {
            if (previewLine != null)
            {
                canvas.Children.Remove(previewLine);
            }
        }

        private Point position1;
        private Point position2;
        private Point lastMousePosition;
        private bool isCurrentlyDrawing = false;
        private Line? previewLine;
        private Polygon2D? currentPolygon;
        private List<Polygon2D> polygons = new List<Polygon2D>();
    }
}
