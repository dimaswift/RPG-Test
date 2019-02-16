using System.Collections;
using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class GameController
    {
        public BattleController BattleController { get; private set; }
        
        public readonly GameConfig Config;
        public readonly PlayerProfile PlayerProfile;
        public readonly HeroFactory HeroFactory;
        public readonly EnemyFactory EnemyFactory;
\
        readonly IProfileProvider _profileProvider;
        
        public GameController(GameConfig config, IProfileProvider profileProvider)
        {
            _profileProvider = profileProvider;
            Config = config;
            PlayerProfile = profileProvider.LoadProfile();
            HeroFactory = new HeroFactory(config);
            EnemyFactory =  new EnemyFactory(config.EnemyGenerator);
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
            battleSnapshot.Heroes = new List<HeroState>(PlayerProfile.HeroesDeck);
            battleSnapshot.Enemies = new List<UnitState>(Config.EnemiesAmount);
            var enemyConfig = EnemyFactory.CreateRandomEnemyConfig(1);
            for (int i = 0; i < Config.EnemiesAmount; i++)
            {
                battleSnapshot.Enemies.Add(EnemyFactory.CreateEnemyState(1, enemyConfig));
            }
            StartBattle(battleSnapshot);
        }

        public void StartBattle(BattleSnapshot battleSnapshot)
        {
            var heroes = CreateHeroes(battleSnapshot.Heroes);
            var enemies = CreateEnemies(battleSnapshot.Enemies, battleSnapshot.Level);
            BattleController = new BattleController(heroes, enemies, battleSnapshot.Level);
        }

        List<HeroController> CreateHeroes(ICollection<HeroState> heroStates)
        {
            var heroes = new List<HeroController>(heroStates.Count);
            foreach (var heroState in heroStates)
            {
                var heroController = HeroFactory.CreateHeroController(heroState);
                heroes.Add(heroController);
            }
            return heroes;
        }
        
        List<UnitController> CreateEnemies(ICollection<UnitState> enemyStates, int level)
        {
            var enemies = new List<UnitController>(enemyStates.Count);
            foreach (var enemyState in enemyStates)
            {
                var heroController = new EnemyController(e);
                enemies.Add(heroController);
            }
            return enemies;
        }
    }    
}