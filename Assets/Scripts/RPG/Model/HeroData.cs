using System;

namespace RPG.Model
{
	[Serializable]
	public class HeroData : UnitData
	{
		public int Experience;
		public int Level;
		public bool Selected;
	}

}
