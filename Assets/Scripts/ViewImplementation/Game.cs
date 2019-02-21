using System.Collections.Generic;
using RPG.Controller;
using RPG.Model;
using RPG.View;
using UnityEngine;

namespace ViewImplementation
{
    public class Game : MonoBehaviour
    {
        public GameConfig Config
        {
            get { return _config; }
        }
        
        public GameController Controller { get; private set; }

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

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            var profileProvider = new PlayerPrefsProfileProvider("profile");
            profileProvider.Delete();
            Controller = new GameController(_config, profileProvider, new UnityRandom());

            var views = FindObjectsOfType<View>();
            
            foreach (var view in views)
            {
                view.Init(this);
                view.Hide();
            }
            
            _heroCollectionView.SetUp(Controller.DeckManager.GetDeck());
            
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
            Controller.StartBattle(_battleView);
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
            _heroCollectionView.SetUp(Controller.DeckManager.GetDeck());
            _heroCollectionView.Show();
        }
    }
}