using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class GameController
    {
        public BattleController BattleController { get; private set; }
        public HeroesCollectionManager CollectionManager { get; private set; }
        public DeckManager DeckManager { get; private set; }

        
        
        readonly GameConfig _config;
        readonly PlayerProfile _playerProfile;
        readonly IProfileProvider _profileProvider;
        readonly IRandomRange _randomRange;
        
        public GameController(GameConfig config, IProfileProvider profileProvider, IRandomRange randomRange)
        {
            _randomRange = randomRange;
            _profileProvider = profileProvider;
            _config = config;
            _playerProfile = profileProvider.LoadProfile();
           
            SetUpCollection();
            SetUpDeck();
            SaveProfile();
        }

        void SetUpDeck()
        {
            if (_playerProfile.Deck.Count == 0)
            {
                _playerProfile.Deck = CollectionManager.CreateDeck(_config.InitialDeckSize);
            }
            DeckManager = new DeckManager(_playerProfile.Deck);
        }
        
        void SetUpCollection()
        {
            if (_playerProfile.Collection.Count == 0)
            {
                _playerProfile.Collection = HeroesCollectionManager.CreateRandomUnitCollection(_config, _randomRange);
            }
            CollectionManager = new HeroesCollectionManager(_config, _playerProfile.Collection, _randomRange);
        }
        

        void SaveProfile()
        {
            _profileProvider.SaveProfile(_playerProfile);
        }

        public void SaveBattleSnapshotToProfile()
        {
            _playerProfile.LastBattleSnapshot = new BattleSnapshot(BattleController);
            SaveProfile();
        }
        
        public void StartBattle(IBattleView battleView)
        {
            var battleSnapshot = new BattleSnapshot(1);
            battleSnapshot.Heroes = new List<HeroState>(_playerProfile.Deck);
            battleSnapshot.Enemies = new List<UnitState>(_config.EnemiesAmount);
            var enemyConfig = CollectionManager.CreateRandomEnemyConfig(1);
            for (int i = 0; i < _config.EnemiesAmount; i++)
            {
                battleSnapshot.Enemies.Add(CollectionManager.CreateState(1, enemyConfig));
            }
            StartBattle(battleSnapshot, battleView);
        }

        public void StartBattle(BattleSnapshot battleSnapshot, IBattleView battleView)
        {
            var heroes = CreateHeroes(battleSnapshot.Heroes);
            var enemies = CreateEnemies(battleSnapshot.Enemies, battleSnapshot.Level);
            BattleController = new BattleController(heroes, enemies, battleView, battleSnapshot.Level);
            BattleController.OnDefeat += ProcessBattleEnd;
            BattleController.OnVictory += ProcessBattleEnd;
        }

        List<HeroController> CreateHeroes(ICollection<HeroState> heroStates)
        {
            var heroes = new List<HeroController>(heroStates.Count);
            foreach (var heroState in heroStates)
            {
                var heroConfig = CollectionManager.GetConfig(heroState.Id);
                var heroController = new HeroController(heroConfig, heroState, _config);
                heroes.Add(heroController);
            }
            return heroes;
        }

        List<UnitController> CreateEnemies(ICollection<UnitState> enemyStates, int level)
        {
            var enemies = new List<UnitController>(enemyStates.Count);
            var config = CollectionManager.CreateRandomEnemyConfig(level);
            foreach (var enemyState in enemyStates)
            {
                
                var enemyController = new EnemyController(config, enemyState);
                enemies.Add(enemyController);
            }
            return enemies;
        }

        void ProcessBattleEnd()
        {
            _playerProfile.BattlesPlayed++;
            if (_playerProfile.BattlesPlayed % _config.FreeHeroPrizeFrequency == 0)
            {
                DeckManager.AddNewHeroToTheDeck(CollectionManager);
                _playerProfile.Deck = new List<HeroState>(DeckManager.GetDeck());
                SaveProfile();
            }
        }
    }    
}