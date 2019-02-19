using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class GameController : IBattleListener
    {
        public BattleController BattleController { get; private set; }
        
        public readonly GameConfig Config;
        public readonly PlayerProfile PlayerProfile;
        public readonly HeroesCollectionManager CollectionManager;
        public readonly DeckManager DeckManager;
        readonly IProfileProvider _profileProvider;
        
        public GameController(GameConfig config, IProfileProvider profileProvider, IRandomRange randomRange)
        {
            _profileProvider = profileProvider;
            Config = config;
            PlayerProfile = profileProvider.LoadProfile();
            if (PlayerProfile.Collection.Count == 0)
            {
                PlayerProfile.Collection = HeroesCollectionManager.CreateRandomCollection(config, randomRange);
            }
            CollectionManager = new HeroesCollectionManager(Config, PlayerProfile.Collection, randomRange);
            if (PlayerProfile.Deck.Count == 0)
            {
                PlayerProfile.Deck = CollectionManager.CreateDeck(config.InitialDeckSize);
            }
            DeckManager = new DeckManager(PlayerProfile.Deck);
            SaveProfile();
        }

        public void SaveProfile()
        {
            _profileProvider.SaveProfile(PlayerProfile);
        }

        public void SaveBattleSnapshotToProfile()
        {
            PlayerProfile.LastBattleSnapshot = new BattleSnapshot(BattleController);
            SaveProfile();
        }
        
        public void StartBattle()
        {
            var battleSnapshot = new BattleSnapshot(1);
            battleSnapshot.Heroes = new List<HeroState>(PlayerProfile.Deck);
            battleSnapshot.Enemies = new List<UnitState>(Config.EnemiesAmount);
            var enemyConfig = CollectionManager.CreateRandomEnemyConfig(1);
            for (int i = 0; i < Config.EnemiesAmount; i++)
            {
                battleSnapshot.Enemies.Add(CollectionManager.CreateState(1, enemyConfig));
            }
            StartBattle(battleSnapshot);
        }

        public void OnDefeat()
        {
            ProcessBattleEnd();
        }

        public void OnVictory()
        {
            ProcessBattleEnd();
        }

        public void StartBattle(BattleSnapshot battleSnapshot)
        {
            var heroes = CreateHeroes(battleSnapshot.Heroes);
            var enemies = CreateEnemies(battleSnapshot.Enemies, battleSnapshot.Level);
            BattleController = new BattleController(heroes, enemies, battleSnapshot.Level);
            BattleController.AddListener(this);
        }

        List<HeroController> CreateHeroes(ICollection<HeroState> heroStates)
        {
            var heroes = new List<HeroController>(heroStates.Count);
            foreach (var heroState in heroStates)
            {
                var heroConfig = CollectionManager.GetConfig(heroState.Name);
                var heroController = new HeroController(heroConfig, heroState, Config);
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
            PlayerProfile.BattlesPlayed++;
            if (PlayerProfile.BattlesPlayed % Config.FreeHeroPrizeFrequency == 0)
            {
                DeckManager.AddNewHeroToTheDeck(CollectionManager);
                PlayerProfile.Deck = new List<HeroState>(DeckManager.GetDeck());
                SaveProfile();
            }
        }
    }    
}