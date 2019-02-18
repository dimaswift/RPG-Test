using System.Collections.Generic;

namespace RPG.Controller
{
    public class BattleController : ObservableController<IBattleListener>
    {
        public readonly int Level;
        readonly List<HeroController> _heroes;
        readonly List<UnitController> _enemies;
        readonly GameController _gameController;

        public BattleController(IEnumerable<HeroController> heroes, 
            IEnumerable<UnitController> enemies,
            int level)
        {
            Level = level;
            _heroes = new List<HeroController>(heroes);
            _enemies = new List<UnitController>(enemies);
        }

        public void ProcessAttack(UnitController attacker, UnitController defender)
        {
            defender.Damage(attacker.State.Attributes.Attack);
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
                
                foreach (var battleListener in GetListeners())
                {
                    battleListener.OnDefeat();
                }
               
                return;
            }

            if (allEnemiesDied)
            {
                ProcessVictory();
                
                foreach (var battleListener in GetListeners())
                {
                    battleListener.OnVictory();
                }
            }
        }

        void ProcessDefeat()
        {
            
        }

        void ProcessVictory()
        {
            
            foreach (var hero in _heroes)
            {
                if(hero.State.Attributes.Hp <= 0)
                    continue;
                hero.AddExperience();
            }

        }
    }
}