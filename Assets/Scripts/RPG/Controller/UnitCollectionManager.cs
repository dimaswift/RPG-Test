using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class UnitCollectionManager
    {
        readonly List<UnitConfig> _collection;
        readonly UnitFactory _unitFactory;
        
        public UnitCollectionManager(IEnumerable<UnitConfig> collection, 
            UnitFactory unitFactory)
        {
            _unitFactory = unitFactory;
            _collection = new List<UnitConfig>(collection);
        }

        public UnitConfig GetConfig(string id)
        {
            foreach (var config in _collection)
            {
                if (config.Id == id)
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

        public List<T> CreateDeck<T>(int size) where T : UnitState, new()
        {
            var deck = new List<T>();
            for (int i = 0; i < size; i++)
            {
   
                if (i >= _collection.Count)
                {
                    break;
                }
                var config = _collection[i];
                var state = new T();
                state.Id = config.Id;
                state.Attributes = _unitFactory.GetLeveledAttributes(config, 1);
                deck.Add(state);
            }

            return deck;
        }
    }
}