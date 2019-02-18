using System.Collections.Generic;

namespace RPG.Controller
{
    public abstract class ObservableController<T> : Controller
    { 
        readonly List<T> _listeners;

        protected ObservableController(params T[] listeners)
        {
            _listeners = new List<T>(listeners);
        }

        public void AddListener(T listener)
        {
            if(_listeners.Contains(listener))
                return;
            _listeners.Add(listener);
        }

        public void RemoveListener(T listener)
        {
            _listeners.Remove(listener);
        }

        protected IEnumerable<T> GetListeners()
        {
            foreach (var listener in _listeners)
            {
                yield return listener;
            }
        }
    }
}