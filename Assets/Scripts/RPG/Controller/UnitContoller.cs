using RPG.Model;

namespace RPG.Controller
{
	public class UnitController : ObservableController<IUnitListener>
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

			foreach (var unitListener in GetListeners())
			{
				unitListener.OnHpAmountChanged(prevHp, hp);
			}
			
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
			if(_state.Attributes.Hp <= 0)
				return;
			_state.Attributes.Hp = 0;

			foreach (var unitListener in GetListeners())
			{
				unitListener.OnDeath();
			}
		}
	}
}
