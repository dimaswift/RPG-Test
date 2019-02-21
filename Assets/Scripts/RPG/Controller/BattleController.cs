using System;
using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class BattleController
    {
        public event Action OnDefeat;
        public event Action OnVictory;
        
        public readonly int Level;
        readonly List<HeroController> _heroes;
        readonly List<UnitController> _enemies;
        readonly GameController _gameController;
        readonly IBattleView _view;

        bool _attackInProcess;

        public BattleController(IEnumerable<HeroController> heroes, 
            IEnumerable<UnitController> enemies, IBattleView view,
            int level)
        {
            _view = view;
            Level = level;
            _heroes = new List<HeroController>(heroes);
            _enemies = new List<UnitController>(enemies);
            _view.OnHeroTap += OnHeroTap;
            view.PrepareBattle(_heroes, _enemies);
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
            _view.StartAttack(attacker.State, defender.State, () => ProcessAttack(attacker, defender));
        }

        UnitController GetEnemyForAttack()
        {
            return _enemies.Count > 0 ? _enemies[0] : null;
        }
        
        void ProcessAttack(UnitController attacker, UnitController defender)
        {
            _attackInProcess = false;
            defender.TakeDamage(attacker.State.Attributes.Attack);
            CheckBattleResult();
            _view.EndAttack(attacker.State, defender.State);
            
            if (attacker is HeroController)
            {
                _view.StartAttack(defender.State, attacker.State, () => ProcessAttack(defender, attacker));
            }
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
            _view.OnHeroTap -= OnHeroTap;
            _view.ProcessDefeat();
            if (OnDefeat != null)
                OnDefeat();
        }

        void ProcessVictory()
        {
            _view.OnHeroTap -= OnHeroTap;
            foreach (var hero in _heroes)
            {
                if(hero.State.Attributes.Hp <= 0)
                    continue;
                hero.AddExperience();
            }
            _view.ProcessVictory();
            if (OnVictory != null)
                OnVictory();
        }

        ~BattleController()
        {
            if(_view != null)
                _view.OnHeroTap -= OnHeroTap;
        }
        
        
    }
}