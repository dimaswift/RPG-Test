using System.Collections;
using System.Collections.Generic;
using RPG.Model;
using UnityEngine;

namespace RPG.Controller
{
    public class BattleController : Controller
    {
        public readonly int Level;
        readonly List<HeroController> _heroes;
        readonly List<UnitController> _enemies;

        public BattleController(IEnumerable<HeroController> heroes, IEnumerable<UnitController> enemies, int level)
        {
            Level = level;
            _heroes = new List<HeroController>(heroes);
            _enemies = new List<UnitController>(enemies);
        }

        public void ProcessAttack(UnitController attacker, UnitController defender)
        {
            defender.Damage(attacker.CurrentAttack);
            CheckBattleResult();
        }

        public IEnumerable<HeroController> GetHeroes()
        {
            foreach (var hero in _heroes)
            {
                yield return hero;
            }
        }
        
        public IEnumerable<UnitController> GetEnemies()
        {
            foreach (var enemy in _enemies)
            {
                yield return enemy;
            }
        }
        
        void CheckBattleResult()
        {
            var allHeroesDied = true;
            var allEnemiesDied = true;
            foreach (var hero in _heroes)
            {
                if (hero.State.Dead)
                    continue;
                allHeroesDied = false;
                break;
            }

            foreach (var enemy in _enemies)
            {
                if (enemy.State.Dead)
                    continue;
                allEnemiesDied = false;
                break;
            }

            if (allEnemiesDied && allHeroesDied)
            {
                OnTie();
                return;
            }

            if (allHeroesDied)
            {
                OnDefeat();
                return;
            }

            if (allEnemiesDied)
            {
                OnVictory();
            }
        }

        protected virtual void OnTie()
        {
        }

        protected virtual void OnDefeat()
        {
        }

        protected virtual void OnVictory()
        {
            foreach (var hero in _heroes)
            {
                if(hero.State.Dead || hero.State.CurrentHp <= 0)
                    continue;
                hero.HeroState.Experience++;
            }
        }
    }
}