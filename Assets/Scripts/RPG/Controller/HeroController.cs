using RPG.Model;

namespace RPG.Controller
{
	public delegate void LevelUpHandler(float extraHp, float extraAttack);
	
	public class HeroController : UnitController
	{
		public event LevelUpHandler OnLevelUp;
		
		public HeroData HeroData
		{
			get
			{
				return Data as HeroData;
			}
		}

		readonly int _xpPerLevel;
		readonly float _levelUpStatMultiplier;
		
		public HeroController(HeroData data, int xpPerLevel, float levelUpStatMultiplier) : base(data)
		{
			_levelUpStatMultiplier = levelUpStatMultiplier;
			_xpPerLevel = xpPerLevel;
		}

		public void AddExperience()
		{
			HeroData.Experience++;

			while (HeroData.Experience >= _xpPerLevel)
			{
				HeroData.Experience -= _xpPerLevel;
				var oldAttack = HeroData.Attack;
				var oldHp = HeroData.Hp;
				HeroData.Attack += HeroData.Attack * _levelUpStatMultiplier;
				HeroData.Hp += HeroData.Hp * _levelUpStatMultiplier;
				HeroData.Level++;
				if (OnLevelUp != null)
					OnLevelUp(HeroData.Hp - oldHp, HeroData.Attack - oldAttack);
			}
		}
	}
}

