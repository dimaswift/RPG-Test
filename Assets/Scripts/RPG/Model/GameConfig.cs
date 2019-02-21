using System;

namespace RPG.Model
{
	[Serializable]
	public class GameConfig
	{
		public float HpLevelUpMultiplier = .1f;
		public float AttackLevelUpMultiplier = .1f;
		public int MaxHeroesCollectionSize = 10;
		public int InitialDeckSize = 3;
		public int BattleDeckSize = 3;
		public int EnemiesAmount = 1;
		public int UnitVisualsAmount = 10;
		public int FreeHeroPrizeFrequency = 5;
		
		public RandomUnitGeneratorConfig HeroGeneratorConfig;
		public RandomUnitGeneratorConfig EnemyGeneratorConfig;
		
		public int ExperiencePointsPerLevel = 5;
	}
}

