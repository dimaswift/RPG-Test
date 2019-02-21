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
                if (config.Id == name)
                    return config;
            }

            return null;
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
                state.Id = config.Id;
                state.Attributes = GetRandomAttributes(_config.HeroGeneratorConfig, _randomRange, 1);
                deck.Add(state);
            }

            return deck;
        }
    }
}