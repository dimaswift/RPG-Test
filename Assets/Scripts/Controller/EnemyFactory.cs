using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Controller
{
	public class EnemyFactory
	{
		readonly RandomUnitGenerator _generator;
		
		public EnemyFactory(RandomUnitGenerator generator)
		{
			_generator = generator;
		}

		public UnitConfig CreateRandomEnemyConfig(int level)
		{
			var enemyConfig = new UnitConfig();
			_generator.SetStats(enemyConfig, level);
			return enemyConfig;
		}
		
		public UnitState CreateEnemyState(int level, UnitConfig config)
		{
			var state = new UnitState();
			state.CurrentHp = config.BaseHp;
			state.Name = "Enemy LVL " + (level + 1);
			return state;
		}

	}

}

