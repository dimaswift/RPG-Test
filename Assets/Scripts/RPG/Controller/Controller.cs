using RPG.View;

namespace RPG.Controller
{
    public abstract class Controller<TView> where TView : IView
    {
        protected TView View { get; private set; }

        public void SetView(TView view)
        {
            View = view;
            OnViewSetUp();
        }

        protected virtual void OnViewSetUp()
        {
            
        }
    }
}