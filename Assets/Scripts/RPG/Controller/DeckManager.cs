using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class DeckManager
    {
        readonly List<HeroState> _deck;
        
        public DeckManager(IEnumerable<HeroState> deck)
        {
            _deck = new List<HeroState>(deck);
        }

        public IEnumerable<HeroState> GetDeck()
        {
            foreach (var heroState in _deck)
            {
                yield return heroState;
            }
        }

        public void AddNewHeroToTheDeck(HeroesCollectionManager collectionManager)
        {
            foreach (var config in collectionManager.GetCollection())
            {
                if (_deck.Find(c => c.Name == config.Name) == null)
                {
                    var state = new HeroState();
                    state.Name = config.Name;
                    state.Attributes = collectionManager.GetLeveledAttributes(config, 1);
                    _deck.Add(state);
                }
            }
        }
    }
}