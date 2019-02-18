using System.Collections.Generic;
using RPG.Model;
using UnityEngine;

namespace RPG.View
{
    public class HeroCollectionView : View
    {
        [SerializeField] int _size;
        [SerializeField] UnitCardView _unitCardViewPrefab;
        
        readonly List<HeroState> _allHeroes = new List<HeroState>();
        readonly List<HeroState> _selectedHeroes = new List<HeroState>();
        
        readonly List<UnitCardView> _cards = new List<UnitCardView>();
        
        public override void Render()
        {
            
        }

        public void SetUp(IEnumerable<HeroState> heroes)
        {
            var i = 0;
            foreach (var heroState in heroes)
            {
                var card = i < _cards.Count ? _cards[i] : AddNewCard();
                card.SetUp(heroState);
                i++;
            }
        }

        UnitCardView AddNewCard()
        {
            var card = Instantiate(_unitCardViewPrefab);
            card.Init(Game);
            card.transform.SetParent(_unitCardViewPrefab.transform.parent);
            card.gameObject.SetActive(true);
            _cards.Add(card);
            return card;
        }
    }
}