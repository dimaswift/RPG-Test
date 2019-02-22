using System;
using System.Collections.Generic;
using RPG.Controller;
using RPG.View;

namespace RPG.UnitTests
{
    class DummyBattleView : IBattleView
    {
        readonly ITestLogger _testLogger;
			
        public DummyBattleView(ITestLogger testLogger)
        {
            _testLogger = testLogger;
        }
			
        public void Render()
        {
            _testLogger.Log("Rendering battle view...");
        }

        public event Action<HeroController> OnHeroTap;

        public void StartAttack(UnitController attacker, UnitController defender, Action damageDealtCallback,
            Action animationFinishedCallback)
        {
            _testLogger.Log(string.Format("{0} started attacking {1}", attacker.Data.Name, defender.Data.Name));
            damageDealtCallback();
            animationFinishedCallback();
        }

        public void SimulateHeroTap(HeroController controller)
        {
            if (OnHeroTap != null)
                OnHeroTap(controller);
        }
			
        public void ProcessDefeat()
        {
            _testLogger.Log("DEFEAT!");
        }

        public void ProcessVictory()
        {
            _testLogger.Log("VICTORY");
        }

        public void PrepareBattle(IEnumerable<HeroController> heroes, IEnumerable<UnitController> enemies)
        {
            _testLogger.Log("Preparing battle! Heroes: ");
            foreach (var hero in heroes)
            {
                _testLogger.Log(hero.HeroData.Name);
            }
            _testLogger.Log("VS: ");
            foreach (var enemy in enemies)
            {
                _testLogger.Log(enemy.Data.Name);
            }
        }
    }
}