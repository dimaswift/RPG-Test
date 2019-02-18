using System;

namespace RPG.Model
{
	[Serializable]
	public class HeroState : UnitState
	{
		public int Experience;
		public int Level;
		public bool Selected;
	}

}
