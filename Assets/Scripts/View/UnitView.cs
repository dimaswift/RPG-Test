using RPG.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.View
{
    public class UnitView : View<UnitController>
    {
        [SerializeField] HealthBarView _healthBar;
        [SerializeField] Text _nameText;
        [SerializeField] Image _bodyImage;
        
        
        
        public override void Render()
        {
            _healthBar.SetHp(Controller.State.CurrentHp / (float)Controller.Config.BaseHp);
            _nameText.text = Controller.Config.Name;
        }
    }
}