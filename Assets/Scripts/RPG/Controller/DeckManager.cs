using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class DeckManager
    {
        readonly List<HeroState> _deck;
        readonly UnitCollectionManager _collectionManager;
        readonly UnitFactory _unitFactory;
        
        public DeckManager(IEnumerable<HeroState> deck, UnitCollectionManager collectionManager, UnitFactory unitFactory)
        {
            _deck = new List<HeroState>(deck);
            _collectionManager = collectionManager;
            _unitFactory = unitFactory;
        }

        public IEnumerable<HeroState> GetDeck()
        {
            foreach (var heroState in _deck)
            {
                yield return heroState;
            }
        }

        public void AddNewHeroToTheDeck()
        {
            foreach (var config in _collectionManager.GetCollection())
            {
                if (_deck.Find(c => c.Id == config.Id) == null)
                {
                    var state = new HeroState();
                    state.Id = config.Id;
                    state.Attributes = _unitFactory.GetLeveledAttributes(config, 1);
                    _deck.Add(state);
                }
            }
        }
    }
}