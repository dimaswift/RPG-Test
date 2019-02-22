using RPG.Controller;
using RPG.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UnityImplementation
{
    public class UnitView : ControlledView<UnitController>, IUnitView, IPointerClickHandler
    {
        [SerializeField] HealthBarView _healthBar;
        [SerializeField] Image _bodyImage;

        HeroInfoTrigger _infoTrigger;
        HeroController _heroController;

        protected override void OnInit(Game game)
        {
            base.OnInit(game);
            _infoTrigger = GetComponent<HeroInfoTrigger>();
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            Controller.SetView(this);
            _heroController = Controller as HeroController;
            if (_infoTrigger != null && _heroController != null && _infoTrigger)
            {
                _infoTrigger.SetHeroData(_heroController.HeroData);
                _heroController.OnLevelUp += OnHeroLevelUp;
            }
            Render();
        }

        void OnHeroLevelUp(float extraHp, float extraAttack)
        {
            Game.TextDropController.DropText(string.Format(@"Hp + {0:0.0} 
Attack + {1:0.0}", extraHp, extraAttack), transform.position, Color.green);
        }

        public void OnHpAmountChanged(float oldHp, float newHp)
        {
            _healthBar.SetHp(newHp / Controller.Data.Hp);
        }
        
        public void OnDeath()
        {
            if (_heroController != null)
                _heroController.OnLevelUp -= OnHeroLevelUp;
            gameObject.SetActive(false);
        }

        public override void Render()
        {
            _healthBar.SetHp(Controller.Hp / Controller.Data.Hp);
            _bodyImage.color = Game.GetUnitColor(Controller.Data.VisualIndex);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_infoTrigger != null && _infoTrigger.IsHeroInfoPanelActive())
                return;
            Game.BattleView.TapOnUnit(this);
        }
        
        
    }
}