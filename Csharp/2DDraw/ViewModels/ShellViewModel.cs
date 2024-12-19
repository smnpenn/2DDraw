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
            var viewmodel = IoC.Get<CanvasViewModel>();
            await ActivateItemAsync(viewmodel, new CancellationToken());
        }
    }
}
