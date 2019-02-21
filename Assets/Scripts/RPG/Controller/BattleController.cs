using System;
using System.Collections.Generic;
using RPG.Model;
using RPG.View;

namespace RPG.Controller
{
    public class BattleController : Controller<IBattleView>
    {
        public readonly int Level;
        
        readonly List<HeroController> _heroes;
        readonly List<UnitController> _enemies;
        readonly GameController _gameController;

        bool _attackInProcess;

        public BattleController(GameController gameController, 
            IEnumerable<HeroController> heroes, 
            IEnumerable<UnitController> enemies,
            int level)
        {
            _gameController = gameController;
            Level = level;
            _heroes = new List<HeroController>(heroes);
            _enemies = new List<UnitController>(enemies);
        }

        public void PrepareBattle()
        {
            View.PrepareBattle(_heroes, _enemies);
            _attackInProcess = false;
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
        
        protected override void OnViewSetUp()
        {
            View.OnHeroTap += OnHeroTap;
        }

        void OnHeroTap(HeroState heroState)
        {
            if(_attackInProcess)
                return;
            var attacker = GetHeroController(heroState);
            if(attacker == null)
                return;
            
            var defender = GetEnemyForAttack();
            if(defender == null)
                return;
            _attackInProcess = true;
            View.StartAttack(attacker, defender,
                () => ResolveDamage(attacker, defender),
                () => DoCounterAttack(defender));
        }

        void DoCounterAttack(UnitController enemy)
        {
            if(enemy.State.Attributes.Hp <= 0)
                return;
            _attackInProcess = true;
            
            var heroToAttack = _heroes.Find(h => h.HeroState.Attributes.Hp > 0);
            
            if (heroToAttack != null)
            {
                View.StartAttack(enemy, heroToAttack, 
                    () => ResolveDamage(enemy, heroToAttack),
                    () => { _attackInProcess = false; });
            } 
        }

        UnitController GetEnemyForAttack()
        {
            return _enemies.Count > 0 ? _enemies[0] : null;
        }
        
        void ResolveDamage(UnitController attacker, UnitController defender)
        {
            defender.TakeDamage(attacker.State.Attributes.Attack);
            CheckBattleResult();
        }

        HeroController GetHeroController(HeroState state)
        {
            foreach (var heroController in _heroes)
            {
                if (heroController.State.Id == state.Id)
                {
                    return heroController;
                }
            }

            return null;
        }
        
        
        
        void CheckBattleResult()
        {
            var allHeroesDied = true;
            var allEnemiesDied = true;
            foreach (var hero in _heroes)
            {
                if (hero.State.Attributes.Hp <= 0)
                    continue;
                allHeroesDied = false;
                break;
            }

            foreach (var enemy in _enemies)
            {
                if (enemy.State.Attributes.Hp <= 0)
                    continue;
                allEnemiesDied = false;
                break;
            }

            if (allHeroesDied)
            {
                ProcessDefeat();
                return;
            }

            if (allEnemiesDied)
            {
                ProcessVictory();
            }
        }

        void ProcessDefeat()
        {
            View.OnHeroTap -= OnHeroTap;
            View.ProcessDefeat();
            _gameController.ProcessBattleEnd(false);
        }

        void ProcessVictory()
        {
            View.OnHeroTap -= OnHeroTap;
            foreach (var hero in _heroes)
            {
                if(hero.State.Attributes.Hp <= 0)
                    continue;
                hero.AddExperience();
            }
            View.ProcessVictory();
            _gameController.ProcessBattleEnd(true);
        }

        ~BattleController()
        {
            if(View != null)
                View.OnHeroTap -= OnHeroTap;
        }
        
        
    }
}