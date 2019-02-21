using System.Collections.Generic;
using RPG.Model;
using RPG.View;
using UnityEngine;
using UnityEngine.UI;

namespace ViewImplementation
{
    public class HeroCollectionView : View
    {

        [SerializeField] HeroInfoPanel _infoPanel;
        [SerializeField] Button _fightButton;
        [SerializeField] Text _selectionAmountText;
        
        readonly List<HeroState> _selectedHeroes = new List<HeroState>();
        
        readonly List<UnitCardView> _cards = new List<UnitCardView>();

        protected override void OnInit(Game game)
        {
            base.OnInit(game);
            GetComponentsInChildren(true, _cards);
            _infoPanel.Hide();
            _fightButton.gameObject.SetActive(false);
            OnSelectionChanged();
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
                var heroConfig = Game.Controller.HeroCollectionManager.GetConfig(heroState.Id);
                if (heroConfig == null)
                {
                    Debug.LogError("no config with name " + heroState.Id);
                    continue;
                }
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

        public void OnFightClick()
        {
            Game.StartNextBattle();
        }
        
        public void OnSelectionChanged()
        {
            _selectedHeroes.Clear();
            foreach (var card in _cards)
            {
                if (card.Selected)
                {
                    if (_selectedHeroes.Count < Game.Config.BattleDeckSize)
                        _selectedHeroes.Add(card.HeroState);
                    else card.Selected = false;
                }
            }

            _selectionAmountText.text = string.Format("{0}/{1}", _selectedHeroes.Count, Game.Config.BattleDeckSize);
            _fightButton.gameObject.SetActive(_selectedHeroes.Count == Game.Config.BattleDeckSize);
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