using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components
{
    public class BaseComponent<T> : ComponentBase, IDisposable where T : ComponentBase
    {
        private readonly CancellationTokenSource _componentDisposeTokenSource = new();
        protected CancellationToken cancellationToken => _componentDisposeTokenSource.Token;

        [Inject] 
        protected NavigationManager navigationManager { get; set; }

        public virtual void Dispose()
        {
            _componentDisposeTokenSource?.Cancel();
            _componentDisposeTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
