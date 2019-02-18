using System.Collections.Generic;
using RPG.Model;
using UnityEngine;

namespace RPG.View
{
    public class HeroCollectionView : View
    {

        [SerializeField] HeroInfoPanel _infoPanel;

        readonly List<HeroState> _selectedHeroes = new List<HeroState>();
        
        readonly List<UnitCardView> _cards = new List<UnitCardView>();

        protected override void OnInit(Game game)
        {
            base.OnInit(game);
            GetComponentsInChildren(true, _cards);
            _infoPanel.Hide();
        }

        public override void Render()
        {
            foreach (var card in _cards)
            {
                card.Render();
            }
        }

        public void SetUp(IEnumerable<HeroState> heroes)
        {
            foreach (var card in _cards)
            {
                card.SetUpAsEmpty();
            }
            var i = 0;
            foreach (var heroState in heroes)
            {
                var card = i < _cards.Count ? _cards[i] : AddNewCard();
                var heroConfig = Game.Controller.HeroesCollectionManager.GetConfig(heroState.Name);
                card.SetUp(heroConfig, heroState, this);
                i++;
            }
        }

        public IEnumerable<HeroState> GetSelectedHeroes()
        {
            foreach (var selectedHero in _selectedHeroes)
            {
                yield return selectedHero;
            }
        }

        public void OnSelectionChanged()
        {
            _selectedHeroes.Clear();
            foreach (var card in _cards)
            {
                if(card.Selected)
                    _selectedHeroes.Add(card.HeroState);
            }
        }        
        
        public void ShowHeroInfo(UnitCardView cardView)
        {
            _infoPanel.transform.position = cardView.transform.position;
            _infoPanel.SetUp(cardView.HeroState);
            _infoPanel.Show();
        }
       
        
        UnitCardView AddNewCard()
        {
            var card = Instantiate(_cards[0]);
            card.Init(Game);
            card.transform.SetParent(_cards[0].transform.parent);
            card.gameObject.SetActive(true);
            _cards.Add(card);
            return card;
        }
    }
}