using RPG.Model;

namespace RPG.Controller
{
	public class UnitFactory
	{
		readonly GameConfig _gameConfig;
		readonly IRandomRange _random;
		
		public UnitFactory(IRandomRange randomRange, GameConfig config)
		{
			_gameConfig = config;
			_random = randomRange;
		}

		public UnitAttributes GetRandomAttributes(RandomUnitGeneratorConfig generatorConfig, int multiplier)
		{
			return new UnitAttributes()
			{
				Attack = _random.Range(generatorConfig.MinAttack, generatorConfig.MaxAttack) * multiplier,
				Hp = _random.Range(generatorConfig.MinHp, generatorConfig.MaxHp) * multiplier,
			};
		}

		public UnitAttributes GetLeveledAttributes(UnitConfig config, int level)
		{
			return new UnitAttributes()
			{
				Attack =  config.Attributes.Attack + level * _gameConfig.AttackLevelUpMultiplier,
				Hp = config.Attributes.Hp + level * _gameConfig.AttackLevelUpMultiplier
			};
		}
		
		public UnitConfig CreateRandomEnemyConfig(int level)
		{
			var enemyConfig = new UnitConfig();
			enemyConfig.Attributes = GetRandomAttributes(_gameConfig.EnemyGeneratorConfig, level);
			enemyConfig.VisualIndex = _random.Range(0, _gameConfig.UnitVisualsAmount);
			return enemyConfig;
		}
		
		public UnitState CreateState(int level, UnitConfig config)
		{
			var state = new UnitState();
			state.Attributes = GetLeveledAttributes(config, level);
			state.Name = "Enemy LVL " + (level + 1);
			return state;
		}

	}

}

