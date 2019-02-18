using RPG.Model;

namespace RPG.Controller
{
	public class HeroController : UnitController
	{
		readonly GameConfig _gameConfig;
		
		public HeroState HeroState
		{
			get
			{
				return State as HeroState;
			}
			set { State = value; }
		}
		
		public HeroController(UnitConfig config, HeroState state, GameConfig gameConfig) : base(config, state)
		{
			_gameConfig = gameConfig;
		}

		public void AddExperience()
		{
			HeroState.Experience++;

			while (HeroState.Experience >= _gameConfig.ExperiencePointsPerLevel)
			{
				HeroState.Experience -= _gameConfig.ExperiencePointsPerLevel;
				HeroState.Level++;
				HeroState.Attributes.Hp += _gameConfig.HpLevelUpMultiplier *  HeroState.Attributes.Hp;
				HeroState.Attributes.Attack += _gameConfig.AttackLevelUpMultiplier * HeroState.Attributes.Attack;
			}
		}
	
	}

}

