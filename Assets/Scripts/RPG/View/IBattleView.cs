using System;
using System.Collections.Generic;
using RPG.Controller;
using RPG.Model;

namespace RPG.View
{
    public interface IBattleView : IView
    {
        event Action<HeroState> OnHeroTap;
        void StartAttack(UnitController attacker, UnitController defender, Action damageDealtCallback, Action animationFinishedCallback);
        void ProcessDefeat();
        void ProcessVictory();
        void PrepareBattle(IEnumerable<HeroController> heroes, IEnumerable<UnitController> enemies);
    }
}