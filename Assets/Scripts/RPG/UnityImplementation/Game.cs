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

        public HeroCollectionView HeroCollectionView
        {
            get { return _heroCollectionView; }
        }
        
        public BattleView BattleView
        {
            get { return _battleView; }
        }

        [SerializeField] GameConfig _config;
        
        [SerializeField] Color[] _unitColors;

        [SerializeField] BattleView _battleView;

        [SerializeField] HeroCollectionView _heroCollectionView;

        [SerializeField] TextDropController _textDropController;
        
        GameController _controller;

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            var profileProvider = new PlayerPrefsProfileProvider("profile");
            profileProvider.Delete();
            _controller = new GameController(_config, profileProvider, new UnityRandom());

            var views = FindObjectsOfType<View>();
            
            foreach (var view in views)
            {
                view.Init(this);
                view.Hide();
            }

            _heroCollectionView.SetUp(_controller.PlayerProfile.Deck);
            
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

        public class UnityRandom : IRandomRange
        {
            public int Range(int min, int max)
            {
                return Random.Range(min, max);
            }
        }

        public void ShowCollectionMenu()
        {
            _battleView.Hide();
            _heroCollectionView.SetUp(_controller.PlayerProfile.Deck);
            _heroCollectionView.Show();
        }
    }
}