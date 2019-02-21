using System.Collections.Generic;
using RPG.Model;
using RPG.View;

namespace RPG.Controller
{
    public class GameController
    {
        public UnitCollectionManager HeroCollectionManager { get; private set; }
        public DeckManager DeckManager { get; private set; }

      
        readonly UnitFactory _unitFactory;
        readonly GameConfig _config;
        readonly PlayerProfile _playerProfile;
        readonly IProfileProvider _profileProvider;

        BattleController _battleController;
        
        public GameController(GameConfig config, IProfileProvider profileProvider, IRandomRange randomRange)
        {
            _profileProvider = profileProvider;
            _config = config;
            _playerProfile = profileProvider.LoadProfile();
            _unitFactory = new UnitFactory(config, randomRange);
            SetUpHeroCollection();
            SetUpDeck();
            SaveProfile();
        }

        public void StartBattle(IBattleView battleView)
        {
            var battleSnapshot = new BattleSnapshot(1);
            battleSnapshot.Heroes = new List<HeroState>(_playerProfile.Deck);
            battleSnapshot.Enemies = new List<UnitConfig>(_config.EnemiesAmount);
            var enemyConfig = _unitFactory.CreateRandomUnitConfig(1);
            for (int i = 0; i < _config.EnemiesAmount; i++)
            {
                battleSnapshot.Enemies.Add(_unitFactory.CreateRandomUnitConfig(1));
            }
            StartBattle(battleSnapshot, battleView);
        }
        
        public void StartBattle(BattleSnapshot battleSnapshot, IBattleView battleView)
        {
            var heroes = CreateHeroes(battleSnapshot.Heroes);
            var enemies = CreateEnemies(battleSnapshot.Enemies, battleSnapshot.Level);
            _battleController = new BattleController(this, heroes, enemies, battleSnapshot.Level);
            _battleController.SetView(battleView);
            _battleController.PrepareBattle();
        }
        
        public void ProcessBattleEnd(bool victory)
        {
            _playerProfile.BattlesPlayed++;
            if (_playerProfile.BattlesPlayed % _config.FreeHeroPrizeFrequency == 0)
            {
                DeckManager.AddNewHeroToTheDeck();
                _playerProfile.Deck = new List<HeroState>(DeckManager.GetDeck());
                SaveProfile();
            }
        }

        public void SaveBattleSnapshotToProfile()
        {
            _playerProfile.LastBattleSnapshot = new BattleSnapshot(_battleController);
            SaveProfile();
        }

        void SetUpDeck()
        {
            if (_playerProfile.Deck.Count == 0)
            {
                _playerProfile.Deck = HeroCollectionManager.CreateDeck<HeroState>(_config.InitialDeckSize);
            }
            DeckManager = new DeckManager(_playerProfile.Deck, HeroCollectionManager, _unitFactory);
        }
        
        void SetUpHeroCollection()
        {
            if (_playerProfile.HeroCollection.Count == 0)
            {
                _playerProfile.HeroCollection = _unitFactory.CreateRandomCollection(_config.HeroGeneratorConfig,
                    _config.MaxHeroesCollectionSize,
                    _config.UnitVisualsAmount, 
                    "Hero");
            }
            HeroCollectionManager = new UnitCollectionManager(_playerProfile.HeroCollection, _unitFactory);
        }
     
        void SaveProfile()
        {
            _profileProvider.SaveProfile(_playerProfile);
        }

      

        List<HeroController> CreateHeroes(ICollection<HeroState> heroStates)
        {
            var heroes = new List<HeroController>(heroStates.Count);
            foreach (var heroState in heroStates)
            {
                var heroConfig = HeroCollectionManager.GetConfig(heroState.Id);
                var heroController = new HeroController(heroConfig, heroState, _config);
                heroes.Add(heroController);
            }
            return heroes;
        }

        List<UnitController> CreateEnemies(ICollection<UnitConfig> enemyConfigs, int level)
        {
            var enemies = new List<UnitController>(enemyConfigs.Count);
            foreach (var config in enemyConfigs)
            {
                var enemyController = new UnitController(config, _unitFactory.CreateState(level, config));
                enemies.Add(enemyController);
            }
            return enemies;
        }

    }    
}