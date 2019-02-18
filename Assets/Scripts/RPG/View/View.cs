using UnityEngine;

namespace RPG.View
{
    public abstract class View : MonoBehaviour
    {
        public abstract void Render();
        
        protected Game Game { get; private set; }
        
        public void Init(Game game)
        {
            if(Game != null)
                return;
            Game = game;
            OnInit(game);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        protected virtual void OnInit(Game game)
        {
            
        }
    }
}