using RPG.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.View
{
    public class UnitView : ControlledView<UnitController>, IUnitListener
    {
        [SerializeField] HealthBarView _healthBar;
        [SerializeField] Image _bodyImage;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            Controller.AddListener(this);
            Render();
        }

        public void OnHpAmountChanged(float oldHp, float newHp)
        {
            _healthBar.SetHp(newHp / Controller.Config.Attributes.Hp);
        }
        
        public void OnDeath()
        {
            gameObject.SetActive(false);
        }

        public override void Render()
        {
            _healthBar.SetHp(Controller.State.Attributes.Hp / (float)Controller.Config.Attributes.Hp);
            _bodyImage.color = Game.GetUnitColor(Controller.Config.VisualIndex);
        }

    }
}