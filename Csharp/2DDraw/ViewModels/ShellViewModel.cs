using Caliburn.Micro;

namespace _2DDraw.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        protected async override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await ShowCanvas();
        }

        public async Task ShowCanvas()
        {
            CanvasViewModel canvasViewModel = IoC.Get<CanvasViewModel>();
            await ActivateItemAsync(canvasViewModel, new CancellationToken());
        }
    }
}
