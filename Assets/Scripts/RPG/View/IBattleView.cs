using System;
using System.Collections.Generic;
using RPG.Controller;

namespace RPG.View
{
    public interface IBattleView : IView
    {
        event Action<HeroController> OnHeroTap;
        void StartAttack(UnitController attacker, UnitController defender, Action damageDealtCallback, Action animationFinishedCallback);
        void ProcessDefeat();
        void ProcessVictory();
        void PrepareBattle(IEnumerable<HeroController> heroes, IEnumerable<UnitController> enemies);
    }
}