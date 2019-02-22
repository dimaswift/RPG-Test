using System.Collections.Generic;
using NUnit.Framework;
using RPG.Controller;
using RPG.Model;

namespace RPG.UnitTests
{
	public class GameTest
	{
		public GameTest(ITestLogger testLogger)
		{
			var profileProvider = new DummyProfileProvider();
			
			var profile = profileProvider.LoadProfile();

			profile.Deck = new List<HeroData>()
			{
				new HeroData()
				{
					Attack = 1,
					Hp = 300000,
					Selected = true,
					Name = "Hero 1"
				},
				new HeroData()
				{
					Attack = 2,
					Hp = 5,
					Selected = true,
					Name = "Hero 2"
				},
				new HeroData()
				{
					Attack = 3,
					Hp = 10,
					Selected = true,
					Name = "Hero 3"
				}
			};
			profileProvider.SaveProfile(profile);
			
			var config = new GameConfig();

			var random = new RandomRange();
			
			var battleView = new DummyBattleView(testLogger);
			
			var gameController = new GameController(config, profileProvider, random);

			TestLevelUp(gameController, config, profileProvider, battleView, testLogger);

			TestDamage(gameController, battleView, testLogger);

		}

		void TestLevelUp(GameController gameController, GameConfig config, IProfileProvider profileProvider, DummyBattleView battleView, ITestLogger testLogger)
		{
			var profile = profileProvider.LoadProfile();
			
			Assert.NotZero(profile.Deck.Count);
			
			var oldAttack = profile.Deck[0].Attack;

			for (int i = 0; i < config.XpPerLevel; i++)
			{
				RunCompleteBattle(gameController, battleView, testLogger);
			}

			Assert.AreEqual(profile.Deck[0].Attack, (oldAttack + oldAttack * config.LevelUpStatsMultiplier), 0.00001f);
		}

		void TestDamage(GameController gameController, DummyBattleView battleView, ITestLogger testLogger)
		{
			gameController.StartBattle(battleView);

			foreach (var controller in gameController.GetAllControllers())
			{
				controller.SetView(new UnitDebugView(controller.Data.Name, testLogger));
			}
			
			var hero = gameController.GetAliveHeroController();

			Assert.NotNull(hero);

			var enemy = gameController.GetAliveEnemyController();
			
			Assert.NotNull(enemy);

			var enemyHp = enemy.Hp;

			battleView.SimulateHeroTap(hero);

			Assert.AreEqual(enemy.Hp,enemyHp - hero.Attack, .00001f);

			testLogger.Log("TestDamage successful!");
		}

		void RunCompleteBattle(GameController gameController, DummyBattleView battleView, ITestLogger testLogger)
		{
			gameController.StartBattle(battleView);
			
			foreach (var controller in gameController.GetAllControllers())
			{
				controller.SetView(new UnitDebugView(controller.Data.Name, testLogger));
			}
			
			while (gameController.IsBattleActive)
			{
				var aliveHero = gameController.GetAliveHeroController();
				if(aliveHero == null)
					break;
				battleView.SimulateHeroTap(aliveHero);
			}
		}
	}
}
