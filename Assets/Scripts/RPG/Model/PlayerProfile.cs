using System;
using System.Collections.Generic;

namespace RPG.Model
{
	[Serializable]
	public class PlayerProfile
	{
		public List<HeroData> Deck = new List<HeroData>();
		public int BattlesPlayed;
	}
}

