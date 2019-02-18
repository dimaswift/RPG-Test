using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class HeroesCollectionManager
    {
        readonly Dictionary<string, UnitConfig> _collection;
        readonly List<HeroState> _deck;
        readonly GameConfig _config;
        readonly UnitFactory _unitFactory;
        
        public HeroesCollectionManager(GameConfig config, 
            IEnumerable<HeroState> deck, 
            IEnumerable<UnitConfig> collection, 
            UnitFactory unitFactory)
        {
            _unitFactory = unitFactory;
            _config = config;
            _deck = new List<HeroState>(deck);
            _collection = new Dictionary<string, UnitConfig>();
            foreach (var unitConfig in collection)
            {
                if(_collection.ContainsKey(unitConfig.Name))
                    continue;
                _collection.Add(unitConfig.Name, unitConfig);
            }
        }

        public UnitConfig GetConfig(string name)
        {
            return _collection.ContainsKey(name) ? _collection[name] : null;
        }

        public void GenerateRandomCollection(IRandomRange randomRange)
        {
            
            for (int i = 0; i < _config.MaxHeroesCollectionSize; i++)
            {
                var hero = new UnitConfig();
                while (_collection.ContainsKey(hero.Name))
                {
                    hero.Name = GetRandomHeroName(randomRange);
                }
                hero.Attributes = _unitFactory.GetRandomAttributes(_config.HeroGeneratorConfig, 1);
                hero.VisualIndex = randomRange.Range(0, _config.UnitVisualsAmount);
                _collection.Add(hero.Name, hero);
            }
            
        }

        string GetRandomHeroName(IRandomRange randomRange)
        {
            return "Hero " + randomRange.Range(0, 1000);
        }

        public void AddNewHeroToTheDeck()
        {
            UnitConfig config = null;
            foreach (var unitConfig in _collection)
            {
                if (_deck.Find(u => u.Name == unitConfig.Key) == null)
                {
                    config = unitConfig.Value;
                    break;
                }
            }

            if (config == null)
            {
                return;
            }
            var state = new HeroState();
            state.Attributes = config.Attributes;
            _deck.Add(state);
        }

        public IEnumerable<UnitConfig> GetCollection()
        {
            foreach (var config in _collection)
            {
                yield return config.Value;
            }
        }
        
        public List<HeroState> GetDeck()
        {
            return _deck;
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
        
    }
}