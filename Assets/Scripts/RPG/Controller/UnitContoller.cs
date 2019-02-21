using RPG.Model;
using RPG.View;

namespace RPG.Controller
{
	public class UnitController : Controller<IUnitView>
	{
		public UnitState State
		{
			get { return _state; }
			set { _state = value; }
		}
		
		public UnitConfig Config
		{
			get { return _config; }
		}
		
		UnitState _state;

		readonly UnitConfig _config;

		public UnitController(UnitConfig config, UnitState state)
		{
			_state = state;
			_config = config;
		}

		public void SetHp(float hp)
		{
			if (hp < 0)
				hp = 0;
			var prevHp = _state.Attributes.Hp;
			
			_state.Attributes.Hp = hp;
			View.OnHpAmountChanged(prevHp, hp);

			if(_state.Attributes.Hp <= 0)
				Kill();
		}

		public void TakeDamage(float amount)
		{
			if(_state.Attributes.Hp <= 0)
				return;
			SetHp(_state.Attributes.Hp - amount);
		}

		public void Kill()
		{
			_state.Attributes.Hp = 0;
			View.OnDeath();
		}
	}
}
