using System.Collections.Generic;
using RPG.Model;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UnityImplementation
{
    public class HeroCollectionView : View
    {
        [SerializeField] Button _fightButton;
        [SerializeField] Text _selectionAmountText;
        [SerializeField] RectTransform _viewport;
        
        readonly List<UnitCardView> _selectedCards = new List<UnitCardView>();
        
        readonly List<UnitCardView> _cards = new List<UnitCardView>();
        

        protected override void OnInit(Game game)
        {
            base.OnInit(game);
            GetComponentsInChildren(true, _cards);
            _fightButton.gameObject.SetActive(false);
        
        }

        public override void Render()
        {
            foreach (var card in _cards)
            {
                card.Render();
            }
        }

        public void SetUp(IEnumerable<HeroData> heroes)
        {
            foreach (var card in _cards)
            {
                card.SetUpAsEmpty();
            }
            var i = 0;
            _selectedCards.Clear();
            foreach (var heroState in heroes)
            {
                var card = i < _cards.Count ? _cards[i] : AddNewCard();
                card.SetUp(heroState, this);
                if (heroState.Selected)
                {
                    _selectedCards.Add(card);
                }
                i++;
            }
            OnSelectionChanged();
        }

        public void OnFightClick()
        {
            Game.StartNextBattle();
        }
        
        public void Deselect(UnitCardView cardView)
        {
            _selectedCards.Remove(cardView);
            OnSelectionChanged();
        }        

        
        public void Select(UnitCardView cardView)
        {
            if (_selectedCards.Count >= Game.Config.BattleDeckSize)
            {
                _selectedCards[_selectedCards.Count - 1].Selected = false;
                _selectedCards.RemoveAt(_selectedCards.Count - 1);
            }
             
            _selectedCards.Add(cardView);
            OnSelectionChanged();
        }

        void OnSelectionChanged()
        {
            _selectionAmountText.text = string.Format("{0}/{1}", _selectedCards.Count, Game.Config.BattleDeckSize);
            _fightButton.gameObject.SetActive(_selectedCards.Count == Game.Config.BattleDeckSize);
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