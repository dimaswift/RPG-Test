using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class UnitFactory
    {
        readonly IRandomRange _randomRange;
        readonly GameConfig _config;
        readonly List<UnitConfig> _enemyCollection;
        
        
        public UnitFactory(GameConfig config, IRandomRange randomRange, IEnumerable<UnitConfig> enemies)
        {
            _enemyCollection = new List<UnitConfig>(enemies);
            _config = config;
            _randomRange = randomRange;
        }
        
        public static UnitAttributes GetRandomAttributes(RandomUnitGeneratorConfig generatorConfig, IRandomRange randomRange, int multiplier)
        {
            return new UnitAttributes()
            {
                Attack = randomRange.Range(generatorConfig.MinAttack, generatorConfig.MaxAttack) * multiplier,
                Hp = randomRange.Range(generatorConfig.MinHp, generatorConfig.MaxHp) * multiplier,
            };
        }
        
        public static List<UnitConfig> CreateRandomUnitCollection(RandomUnitGeneratorConfig generatorConfig,
            int size,
            int visualsAmount, 
            IRandomRange randomRange, 
            string prefix = "Unit")
        {
            var collection = new List<UnitConfig>();
            var visualIndexList = new List<int>();
            for (int i = 0; i < visualsAmount; i++)
            {
                visualIndexList.Add(i);
            }

            var visualIndex = 0;
            visualIndexList.Sort((v1, v2) => randomRange.Range(-1, 2));
            for (int i = 0; i < size; i++)
            {
                var hero = new UnitConfig();
                hero.Id = GetRandomUnitName(prefix, randomRange);
                while (collection.Find(h => hero.Id == h.Id) != null)
                {
                    hero.Id = GetRandomUnitName(prefix, randomRange);
                }
                
                hero.Attributes = GetRandomAttributes(generatorConfig, randomRange, 1);
                visualIndex++;
                if (visualIndex >= visualIndexList.Count)
                    visualIndex = 0;
                hero.VisualIndex = visualIndexList[visualIndex];
                
                collection.Add(hero);
            }

            return collection;
        }
        
              
  
        static string GetRandomUnitName(string prefix, IRandomRange randomRange)
        {
            return string.Format("{0} {1}", prefix, randomRange.Range(0, 1000));
        }


        public UnitAttributes GetLeveledAttributes(UnitConfig config, int level)
        {
            return new UnitAttributes()
            {
                Attack =  config.Attributes.Attack + level * _config.AttackLevelUpMultiplier,
                Hp = config.Attributes.Hp + level * _config.AttackLevelUpMultiplier
            };
        }
		
        public UnitConfig CreateRandomUnitConfig(int level)
        {
            var enemyConfig = new UnitConfig();
            enemyConfig.Attributes = GetRandomAttributes(_config.EnemyGeneratorConfig, _randomRange, level);
            enemyConfig.VisualIndex = _randomRange.Range(0, _config.UnitVisualsAmount);
            return enemyConfig;
        }
		
        public UnitState CreateState(int level, UnitConfig config)
        {
            var state = new UnitState();
            state.Attributes = GetLeveledAttributes(config, level);
            state.Id = config.Id;
            return state;
        }
    }
}