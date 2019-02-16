using UnityEngine;

namespace RPG.View
{
    public abstract class View<T> : MonoBehaviour where T : Controller.Controller
    {
        
        public T Controller { get; private set; }
        
        public abstract void Render();

        public void SetUp(T controller)
        {
            Controller = controller;
            OnSetUp();
        }

        protected virtual void OnSetUp()
        {
            
        }
        
    }
}