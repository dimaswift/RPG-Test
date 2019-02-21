﻿using System;
using System.Collections.Generic;

namespace RPG.Model
{
	[Serializable]
	public class PlayerProfile
	{
		public BattleSnapshot LastBattleSnapshot;
		public List<HeroState> Deck = new List<HeroState>();
		public List<UnitConfig> HeroCollection = new List<UnitConfig>();
		public int BattlesPlayed;
	}
}

