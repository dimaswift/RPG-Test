using UnityEngine;

namespace RPG.View
{
    public abstract class View : MonoBehaviour
    {
        public abstract void Render();
        
        protected Game Game { get; private set; }
        
        public void Init(Game game)
        {
            Game = game;
            OnInit(game);
        }

        protected virtual void OnInit(Game game)
        {
            
        }
    }
}