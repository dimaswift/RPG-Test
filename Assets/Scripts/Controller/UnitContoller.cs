using System;
using System.Collections.Generic;
using RPG.Model;


namespace RPG.Controller
{
	public class UnitController : Controller
	{
		public event Action OnDeath;
		public event Action<int, int> OnHpAmountChanged;
		public event Action<int> OnDamageTaken;
		
		public int CurrentAttack { get; private set; }

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
			CalculateStats();
		}

		void CalculateStats()
		{
			CurrentAttack = CalculateAttack();
		}

		public void SetHp(int hp)
		{
			var prevHp = _state.CurrentHp;
			_state.CurrentHp = hp;
			var delta = hp - prevHp;
			if(OnHpAmountChanged != null)
				OnHpAmountChanged(prevHp, _state.CurrentHp);
			if(_state.CurrentHp <= 0)
				Kill();
		}

		public void Damage(int amount)
		{
			if(_state.CurrentHp <= 0)
				return;
			SetHp(_state.CurrentHp - amount);
			if (OnDamageTaken != null)
				OnDamageTaken(amount);
		}

		public void Kill()
		{
			if(_state.CurrentHp <= 0)
				return;
			_state.CurrentHp = 0;
			if(OnDeath != null)
				OnDeath();
		}

		protected virtual int CalculateAttack()
		{
			return _config.BaseAttack;
		}

		protected virtual int CalculateHp()
		{
			return _config.BaseHp;
		}

		protected virtual void HpAmountChanged(int oldHp, int newHp)
		{
			
		}
		
		protected virtual void DamageTaken(int amount)
		{
			
		}

		protected virtual void Killed()
		{
			
		}
	}

}
