using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Model;
using UnityEngine;

namespace RPG.Controller
{
	public class HeroController : UnitController
	{
		public HeroState HeroState
		{
			get
			{
				return State as HeroState;
			}
		}
		
		public HeroController(UnitConfig config, HeroState state) : base(config, state)
		{
			
		}
		

	}

}

