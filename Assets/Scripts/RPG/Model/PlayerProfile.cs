using System.Collections.Generic;

namespace RPG.Model
{
	public class PlayerProfile
	{
		public BattleSnapshot LastBattleSnapshot;
		public List<HeroState> Deck = new List<HeroState>();
		public List<UnitConfig> Collection = new List<UnitConfig>();
		public int BattlesPlayed;
	}
}

