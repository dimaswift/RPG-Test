using RPG.Controller;
using RPG.Model;
using UnityEngine;

namespace RPG.View
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField] GameConfig _gameConfig;
        [SerializeField] PlayerProfile _defaultProfile;

        public GameController GameController { get; private set; }
        
        void Awake()
        {
            Init();
        }

        public void Init()
        {
            GameController = new GameController(_gameConfig, new PlayerPrefsProfileProvider("profile", _defaultProfile));
            GameController.StartBattle();
        }

    }
}