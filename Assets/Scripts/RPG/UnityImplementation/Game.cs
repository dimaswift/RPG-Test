using RPG.Controller;
using RPG.Model;
using UnityEngine;

namespace RPG.UnityImplementation
{
    public class Game : MonoBehaviour
    {
        public GameConfig Config
        {
            get { return _config; }
        }

        public TextDropController TextDropController
        {
            get { return _textDropController; }
        }
        
        public HeroInfoPanel HeroInfoPanel
        {
            get { return _heroInfoPanel; }
        }
        
        public BattleView BattleView
        {
            get { return _battleView; }
        }

        [SerializeField] GameConfig _config;
        
        [SerializeField] Color[] _unitColors;

        [SerializeField] BattleView _battleView;
        
        [SerializeField] HeroInfoPanel _heroInfoPanel;

        [SerializeField] HeroCollectionView _heroCollectionView;

        [SerializeField] TextDropController _textDropController;
        
        GameController _controller;

        void Awake()
        {
            Init();
        }
        
        void Init()
        {
            var profileProvider = new PlayerPrefsProfileProvider("profile");

            _controller = new GameController(_config, profileProvider, new UnityRandom());

            var views = GetComponentsInChildren<View>(true);
            
            foreach (var view in views)
            {
                view.Init(this);
            }

            _heroCollectionView.SetUp(_controller.GetPlayerDeck());
            
            _heroCollectionView.Show();
        }

        public Color GetUnitColor(int index)
        {
            if(index < 0 || index >= _unitColors.Length)
                return Color.black;
            return _unitColors[index];
        }

        public void StartNextBattle()
        {
            _heroCollectionView.Hide();
            _controller.StartBattle(_battleView);
        }

        void OnApplicationQuit()
        {
            if (_controller != null)
                _controller.SaveProfile();
        }

        public void ShowCollectionMenu()
        {
            _battleView.Hide();
            _heroCollectionView.SetUp(_controller.GetPlayerDeck());
            _heroCollectionView.Show();
        }
    }
}