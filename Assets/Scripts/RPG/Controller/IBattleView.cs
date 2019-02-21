using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public interface IBattleView
    {
        event Action<HeroState> OnHeroTap;
        void StartAttack(UnitState attacker, UnitState defender, Action onAttackFinish);
        void EndAttack(UnitState attacker, UnitState defender);
        void ProcessDefeat();
        void ProcessVictory();
        void PrepareBattle(IEnumerable<HeroController> heroes, IEnumerable<UnitController> enemies);
    }
}