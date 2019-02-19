using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class HeroesCollectionManager
    {
        readonly List<UnitConfig> _collection;
        readonly GameConfig _config;
        readonly IRandomRange _randomRange;
        
        public HeroesCollectionManager(GameConfig config, 
            IEnumerable<UnitConfig> collection, 
            IRandomRange randomRange)
        {
            _randomRange = randomRange;
            _config = config;
            _collection = new List<UnitConfig>(collection);
        }

        public UnitConfig GetConfig(string name)
        {
            foreach (var config in _collection)
            {
                if (config.Name == name)
                    return config;
            }

            return null;
        }

        public static List<UnitConfig> CreateRandomCollection(GameConfig config, IRandomRange randomRange)
        {
            var collection = new List<UnitConfig>();
            var visualIndexList = new List<int>();
            for (int i = 0; i < config.UnitVisualsAmount; i++)
            {
                visualIndexList.Add(i);
            }

            var visualIndex = 0;
            visualIndexList.Sort((v1, v2) => randomRange.Range(-1, 2));
            for (int i = 0; i < config.MaxHeroesCollectionSize; i++)
            {
                var hero = new UnitConfig();
                hero.Name = GetRandomHeroName(randomRange);
                while (collection.Find(h => hero.Name == h.Name) != null)
                {
                    hero.Name = GetRandomHeroName(randomRange);
                }
                
                hero.Attributes = GetRandomAttributes(config.HeroGeneratorConfig, randomRange, 1);
                visualIndex++;
                if (visualIndex >= visualIndexList.Count)
                    visualIndex = 0;
                hero.VisualIndex = visualIndexList[visualIndex];
                
                collection.Add(hero);
            }

            return collection;
        }
        

        public static UnitAttributes GetRandomAttributes(RandomUnitGeneratorConfig generatorConfig, IRandomRange randomRange, int multiplier)
        {
            return new UnitAttributes()
            {
                Attack = randomRange.Range(generatorConfig.MinAttack, generatorConfig.MaxAttack) * multiplier,
                Hp = randomRange.Range(generatorConfig.MinHp, generatorConfig.MaxHp) * multiplier,
            };
        }

        public UnitAttributes GetLeveledAttributes(UnitConfig config, int level)
        {
            return new UnitAttributes()
            {
                Attack =  config.Attributes.Attack + level * _config.AttackLevelUpMultiplier,
                Hp = config.Attributes.Hp + level * _config.AttackLevelUpMultiplier
            };
        }
		
        public UnitConfig CreateRandomEnemyConfig(int level)
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
            state.Name = "Enemy LVL " + (level + 1);
            return state;
        }

        static string GetRandomHeroName(IRandomRange randomRange)
        {
            return "Hero " + randomRange.Range(0, 1000);
        }

        public IEnumerable<UnitConfig> GetCollection()
        {
            foreach (var config in _collection)
            {
                yield return config;
            }
        }

        public HeroState AddExperienceToHero(HeroState state)
        {
            state.Experience++;

            while (state.Experience >= _config.ExperiencePointsPerLevel)
            {
                state.Experience -= _config.ExperiencePointsPerLevel;
                state.Level++;
                state.Attributes.Hp += _config.HpLevelUpMultiplier *  state.Attributes.Hp;
                state.Attributes.Attack += _config.AttackLevelUpMultiplier * state.Attributes.Attack;
            }

            return state;
        }

        public List<HeroState> CreateDeck(int size)
        {
            var deck = new List<HeroState>();
            for (int i = 0; i < size; i++)
            {
   
                if (i >= _collection.Count)
                {
                    break;
                }
                var config = _collection[i];
                var state = new HeroState();
                state.Name = config.Name;
                state.Attributes = GetRandomAttributes(_config.HeroGeneratorConfig, _randomRange, 1);
                deck.Add(state);
            }

            return deck;
        }
    }
}