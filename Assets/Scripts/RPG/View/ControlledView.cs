namespace RPG.View
{
    public abstract class ControlledView<TController> : View where TController : Controller.Controller
    {
        public TController Controller { get; private set; }
        
        public void SetUp(TController controller)
        {
            Controller = controller;
            OnSetUp();
        }

        protected virtual void OnSetUp()
        {
            
        }
    }
}