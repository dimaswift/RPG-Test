using System;

namespace RPG.Model
{
	[Serializable]
	public class GameConfig
	{
		public float LevelUpStatsMultiplier = .1f;
		public int MaxHeroesCollectionSize = 10;
		public int InitialDeckSize = 3;
		public int BattleDeckSize = 3;
		public int EnemiesAmount = 1;
		public float EnemyLevelUpMultiplier = .1f;
		public int UnitVisualsAmount = 10;
		public int FreeHeroPrizeFrequency = 5;
		
		public RandomUnitGeneratorConfig HeroGeneratorConfig;
		public RandomUnitGeneratorConfig EnemyGeneratorConfig;
		
		public int XpPerLevel = 5;
	}
}

