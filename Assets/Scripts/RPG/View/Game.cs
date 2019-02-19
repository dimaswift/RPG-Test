using System.Collections.Generic;
using RPG.Controller;
using RPG.Model;
using UnityEngine;

namespace RPG.View
{
    public class Game : MonoBehaviour
    {
        public GameConfig Config
        {
            get { return _config; }
        }
        
        public GameController Controller { get; private set; }

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
         
            Controller = new GameController(_config, profileProvider, new UnityRandom());

            var views = FindObjectsOfType<View>();
            
            foreach (var view in views)
            {
                view.Init(this);
                view.Hide();
            }
            
            _heroCollectionView.SetUp(Controller.DeckManager.GetDeck());
            
            _heroCollectionView.Show();
            
            StartNextBattle();

        }

        public Color GetUnitColor(int index)
        {
            if(index < 0 || index >= _unitColors.Length)
                return Color.black;
            return _unitColors[index];
        }

        public void StartNextBattle()
        {
            Controller.StartBattle();
            _battleView.SetUp(Controller.BattleController);
        }

        class UnityRandom : IRandomRange
        {
            public int Range(int min, int max)
            {
                return Random.Range(min, max);
            }
        }
        
    }
}