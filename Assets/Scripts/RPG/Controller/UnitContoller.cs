using RPG.Model;
using RPG.View;

namespace RPG.Controller
{
	public class UnitController : Controller<IUnitView>
	{
		public float Hp { get; private set; }
		public float Attack { get; private set; }
		
		public UnitData Data
		{
			get { return _data; }
		}
	
		readonly UnitData _data;
		
		public UnitController(UnitData data)
		{
			Hp = data.Hp;
			Attack = data.Attack;
			_data = data;
		}
		
		public void TakeDamage(float amount)
		{
			if(Hp <= 0)
				return;
			SetHp(Hp - amount);
		}

		void SetHp(float hp)
		{
			if (hp < 0)
				hp = 0;
			
			var prevHp = Hp;
			Hp = hp;
			if(View != null)
				View.OnHpAmountChanged(prevHp, hp);

			if(Hp <= 0)
				Kill();
		}

		void Kill()
		{
			Hp = 0;
			if(View != null)
				View.OnDeath();
		}
	}
}
