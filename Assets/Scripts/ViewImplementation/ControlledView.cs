namespace ViewImplementation
{
    public abstract class ControlledView<TController> : View
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