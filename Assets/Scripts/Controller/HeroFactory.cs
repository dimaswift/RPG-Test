using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Controller
{
	public class HeroFactory : UnitFactory
	{
		readonly Dictionary<string, UnitConfig> _availableHeroes = new Dictionary<string, UnitConfig>(10);

		readonly GameConfig _gameConfig;
		
		public HeroFactory(GameConfig config, params UnitConfig[] heroes) : base (config.HeroGenerator)
		{
			_gameConfig = config;
			foreach (var hero in heroes)
			{
				AddHeroToCollection(hero);
			}
			
			while (_availableHeroes.Count < config.MaxHeroesCollectionSize)
			{
				var randomHero = CreateRandomHeroConfig(config);
				AddHeroToCollection(randomHero);
			}
		}
		
		public HeroController CreateHeroController(HeroState heroState)
		{
			var config = GetHeroConfig(heroState.Name);
			return new HeroController(config, heroState);
		}

		UnitConfig GetHeroConfig(string name)
		{
			UnitConfig hero;
			if(_availableHeroes.TryGetValue(name, out hero))
				return hero;
			hero = CreateRandomHeroConfig(_gameConfig);
			AddHeroToCollection(hero);
			return hero;
		}

		void AddHeroToCollection(UnitConfig heroConfig)
		{
			while (_availableHeroes.ContainsKey(heroConfig.Name))
			{
				heroConfig.Name = GetRandomHeroName();
			}
			_availableHeroes.Add(heroConfig.Name, heroConfig);
		}
		
		string GetRandomHeroName()
		{
			return "Hero " + Random.Range(0, 1000);
		}
		
		UnitConfig CreateRandomHeroConfig(GameConfig config)
		{
			var heroConfig = new UnitConfig()
			{
				Name = GetRandomHeroName()
			};
			config.HeroGenerator.SetStats(heroConfig, 1);
			return heroConfig;
		}
	}

}

