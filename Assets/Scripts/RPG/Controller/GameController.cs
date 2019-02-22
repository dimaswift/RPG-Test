using System.Collections.Generic;
using RPG.Model;
using RPG.View;

namespace RPG.Controller
{
    public class GameController
    {
        public bool IsBattleActive { get; private set; }
        
        readonly GameConfig _config;

        readonly UnitFactory _unitFactory;

        readonly IProfileProvider _profileProvider;

        readonly IRandomRange _randomRange;
        
        readonly PlayerProfile _playerProfile;

        BattleController _battleController;
        
        public GameController(GameConfig config, IProfileProvider profileProvider, IRandomRange randomRange)
        {
            _config = config;
            _randomRange = randomRange;
            _profileProvider = profileProvider;
            _unitFactory = new UnitFactory(randomRange, _config.UnitVisualsAmount);
            _playerProfile = profileProvider.LoadProfile();
            CheckHeroDeck();
            SaveProfile();
        }

        public void SaveProfile()
        {
            _profileProvider.SaveProfile(_playerProfile);
        }
 
        public IEnumerable<HeroData> GetPlayerDeck()
        {
            foreach (var heroData in _playerProfile.Deck)
            {
                yield return heroData;
            }
        }
        
        public IEnumerable<UnitController> GetAllControllers()
        {
            if (_battleController == null)
                yield break;
            foreach (var controller in _battleController.GetHeroControllers())
            {
                yield return controller;
            }
            foreach (var controller in _battleController.GetEnemyControllers())
            {
                yield return controller;
            }
        }

        public HeroController GetAliveHeroController()
        {
            if (_battleController == null)
                return null;
            foreach (var heroController in _battleController.GetHeroControllers())
            {
                if (heroController.Hp > 0)
                    return heroController;
            }
            return null;
        }
        
        public UnitController GetAliveEnemyController()
        {
            if (_battleController == null)
                return null;
            foreach (var enemyController in _battleController.GetEnemyControllers())
            {
                if (enemyController.Hp > 0)
                    return enemyController;
            }
            return null;
        }
        
        public void StartBattle(IBattleView battleView)
        {
            SaveProfile();
            _battleController = new BattleController(_config, 
                _playerProfile.Deck, 
                _unitFactory, 
                _playerProfile.BattlesPlayed,
                _randomRange);
            _battleController.SetView(battleView);
            _battleController.PrepareBattle();
            _battleController.OnDefeat += ProcessBattleEnd;
            _battleController.OnVictory += ProcessBattleEnd;
            IsBattleActive = true;
        }

        void ProcessBattleEnd()
        {
            _playerProfile.BattlesPlayed++;
            if (_playerProfile.BattlesPlayed % _config.FreeHeroPrizeFrequency == 0
                && _playerProfile.Deck.Count < _config.MaxHeroesCollectionSize)
            {
                var newHeroData = _unitFactory.CreateRandomHeroData(_config.HeroGeneratorConfig);
                _playerProfile.Deck.Add(newHeroData);
            }
        
            _battleController.OnDefeat -= ProcessBattleEnd;
            _battleController.OnVictory -= ProcessBattleEnd;
            
            SaveProfile();
            IsBattleActive = false;
        }

        void CheckHeroDeck()
        {
            if (_playerProfile.Deck.Count == 0)
            {
                _playerProfile.Deck = _unitFactory.CreateHeroDeck(_config.InitialDeckSize, _config.HeroGeneratorConfig);
            }
        }


    }    
}