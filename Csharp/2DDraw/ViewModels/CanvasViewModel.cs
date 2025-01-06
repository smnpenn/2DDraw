using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using _2DDraw.Models;

namespace _2DDraw.ViewModels
{
    public sealed class CanvasViewModel : Screen
    {
        public bool UndoEnabled => undoStack.Count > 0;
        public bool RedoEnabled => redoStack.Count > 0;
        public Canvas DrawingCanvas { get; set; }

        public List<Polygon2D> Polygons = new List<Polygon2D>();

        public void Undo()
        {
            if (undoStack.Count > 0 && DrawingCanvas != null)
            {
                // Pop the last line from the undo stack and push into redo stack
                DrawingOperation op = undoStack.Pop();
                redoStack.Push(op);

                if( op.Action == DrawingAction.FinishPolygon)
                {

                }
                else
                {
                    Line line = op.Line!;

                    // Find the polygon that contains the line
                    Polygon2D? polygon = Polygons.Find(p => p.Lines.Contains(line));

                    if (polygon != null)
                    {
                        polygon.Lines.Remove(line);

                        // if undone line is the last one, mark polygon as unfinished
                        if (line == polygon.LastLine)
                        {
                            polygon.IsFinished = false;
                        }

                        // set pos1 to startpoint of undone line if polygon is not empty (for preview)
                        if (polygon.Lines.Count > 0)
                        {
                            position1 = new Point(line.X1, line.Y1);
                            isDrawing = true;
                        }
                        else
                        {
                            isDrawing = false;
                        }
                    }
                    currentPolygon = polygon;
                }

                // Refresh the canvas
                RedrawCanvas(DrawingCanvas);
                Refresh();
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0 && DrawingCanvas != null)
            {
                // Pop the last line from the redo stack and push into undo stack
                DrawingOperation op = redoStack.Pop();
                undoStack.Push(op);

                if (op.Action == DrawingAction.FinishPolygon)
                {

                }
                else
                {
                    Line line = op.Line!;

                    Polygon2D? polygon = currentPolygon;

                    if (polygon != null)
                    {
                        polygon.Lines.Add(line);

                        // if redone is the last line, mark polygon as finished
                        if (polygon.LastLine == line)
                        {
                            polygon.IsFinished = true;

                            // set currentPolygon to the next one or to null if its the last one
                            if (polygon == Polygons.Last())
                            {
                                currentPolygon = null;
                            }
                            else
                            {
                                currentPolygon = Polygons[Polygons.IndexOf(polygon) + 1];
                            }

                            isDrawing = false;
                        }
                        else
                        {
                            // set pos1 to endpoint of redone line
                            position1 = new Point(line.X2, line.Y2);
                            isDrawing = true;
                        }
                    }
                }

                // Refresh the canvas
                RedrawCanvas(DrawingCanvas);
                Refresh();
            }
        }

        public void MouseMove_Canvas(Canvas canvas, MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(canvas);

            if (lastMousePosition == currentPos)
            {
                return;
            }

            if (isDrawing)
            {
                ShowPreview(canvas, currentPos);
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

            if (!isDrawing)
            {
                position1 = e.GetPosition(canvas);
                currentPolygon = new Polygon2D();
                Polygons.Add(currentPolygon);

                isDrawing = true;
            }
            else
            {
                position2 = e.GetPosition(canvas);
                Line line = DrawLine(canvas, position1, position2, Brushes.Black);
                currentPolygon?.Lines.Add(line);
                undoStack.Push(new() { Line = line, Action = DrawingAction.DrawLine });
                redoStack.Clear();

                position1 = position2;
            }
        }

        public void MouseDoubleClick_Canvas(Canvas canvas, MouseEventArgs e)
        {
            if (currentPolygon != null)
            {
                ClearPreview(canvas);
                currentPolygon.LastLine = currentPolygon.Lines.Last();
                RedrawCanvas(canvas);

                currentPolygon = null;
                isDrawing = false;
            }
        }

        private void RedrawCanvas(Canvas canvas)
        {
            canvas.Children.Clear();

            Polygons.ForEach(p => DrawPolygon(canvas, p));
        }

        private void DrawPolygon(Canvas canvas, Polygon2D polygon)
        {
            Brush brush = polygon.IsFinished ? Brushes.Orange : Brushes.Black;
            foreach (var line in polygon.Lines)
            {
                DrawLine(canvas, line, brush);
            }
        }

        private Line DrawLine(Canvas canvas, Line line, Brush brush)
        {
            line.Stroke = brush;
            line.StrokeThickness = 2;

            canvas?.Children.Add(line);
            Refresh();

            return line;
        }

        private Line DrawLine(Canvas canvas, Point pos1, Point pos2, Brush brush)
        {
            Line line = new Line()
            {
                X1 = pos1.X,
                Y1 = pos1.Y,
                X2 = pos2.X,
                Y2 = pos2.Y
            };

            return DrawLine(canvas, line, brush);
        }

        private void ShowPreview(Canvas canvas, Point currentMousePosition)
        {
            ClearPreview(canvas);
            previewLine = DrawLine(canvas, position1, currentMousePosition, Brushes.Black);
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
        private Line? previewLine;
        private Polygon2D? currentPolygon;
        private bool isDrawing = false;

        private Stack<DrawingOperation> undoStack = new Stack<DrawingOperation>();
        private Stack<DrawingOperation> redoStack = new Stack<DrawingOperation>();
    }
}
