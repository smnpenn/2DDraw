using System.Windows;
using System.Windows.Controls;
using _2DDraw.ViewModels;
using Caliburn.Micro;

namespace _2DDraw.Views
{
    /// <summary>
    /// Interaktionslogik für Canvas.xaml
    /// </summary>
    public partial class CanvasView : UserControl
    {
        public CanvasView()
        {
            InitializeComponent();

            this.Loaded += CanvasView_Loaded;
        }

        private void CanvasView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is CanvasViewModel viewModel)
            {
                viewModel.DrawingCanvas = this.DrawingCanvas;
            } 
        }
    }
}
