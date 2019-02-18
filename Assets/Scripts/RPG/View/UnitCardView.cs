using RPG.Model;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.View
{
    public class UnitCardView : View
    {
        [SerializeField] Image _unitBodyImage;
        [SerializeField] GameObject _selectionFrame;

        UnitConfig _unitConfig;
        HeroState _state;

        bool _selected;
        
        public void SetUp(UnitConfig hero, HeroState state)
        {
            _state = state;
            _unitConfig = hero;
            Render();
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; _selectionFrame.SetActive(value); }
        }
        
        public override void Render()
        {
            _unitBodyImage.color = Game.GetUnitColor(_unitConfig.VisualIndex);
            Selected = _state.Selected;
        }
    }
}