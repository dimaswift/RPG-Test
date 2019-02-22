using System;
using System.Collections.Generic;
using RPG.Model;
using RPG.View;

namespace RPG.Controller
{
    public class BattleController : Controller<IBattleView>
    {
        public event Action OnVictory;
        public event Action OnDefeat;
        
        readonly List<HeroController> _heroes;
        readonly List<UnitController> _enemies;
        readonly GameConfig _gameConfig;
        readonly int _level;
        readonly UnitFactory _unitFactory;
        readonly IRandomRange _randomRange;
        readonly List<HeroController> _heroesBuffer;
        
        bool _attackInProcess;
        
        public BattleController(GameConfig config, IEnumerable<HeroData> heroes, UnitFactory unitFactory, int level, IRandomRange randomRange)
        {
            _unitFactory = unitFactory;
            _level = level;
            _gameConfig = config;
            _heroes = CreateHeroes(heroes);
            _heroesBuffer = new List<HeroController>(_heroes.Count);
            _enemies = CreateEnemies();
            _randomRange = randomRange;
        }

        public IEnumerable<HeroController> GetHeroControllers()
        {
            foreach (var heroController in _heroes)
            {
                yield return heroController;
            }
        }
        
        public IEnumerable<UnitController> GetEnemyControllers()
        {
            foreach (var enemyController in _enemies)
            {
                yield return enemyController;
            }
        }
        
        public void PrepareBattle()
        {
            View.PrepareBattle(_heroes, _enemies);
            _attackInProcess = false;
        }

        List<UnitController> CreateEnemies()
        {
            var enemies = new List<UnitController>(_gameConfig.EnemiesAmount);
            for (int i = 0; i < _gameConfig.EnemiesAmount; i++)
            {
                var levelMultiplier = 1 + (_level * _gameConfig.EnemyLevelUpMultiplier);
                var enemyConfig =_unitFactory.CreateRandomUnitData("Enemy", _gameConfig.EnemyGeneratorConfig, levelMultiplier);
                enemies.Add(new UnitController(enemyConfig));
            }

            return enemies;
        }

        List<HeroController> CreateHeroes(IEnumerable<HeroData> heroesData)
        {
            var heroes = new List<HeroController>();
            foreach (var heroData in heroesData)
            {
                if(!heroData.Selected)
                    continue;
                var heroController = new HeroController(heroData,
                    _gameConfig.XpPerLevel, 
                    _gameConfig.LevelUpStatsMultiplier);
                heroes.Add(heroController);
            }
            return heroes;
        }

        protected override void OnViewSetUp()
        {
            View.OnHeroTap += OnHeroTap;
        }

        void OnHeroTap(HeroController attacker)
        {
            if(_attackInProcess)
                return;
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
            if(enemy.Hp <= 0)
                return;
            _attackInProcess = true;
            _heroesBuffer.Clear();
            foreach (var heroController in _heroes)
            {
                if(heroController.Hp > 0)
                    _heroesBuffer.Add(heroController);
            }

            if (_heroesBuffer.Count > 0)
            {
                var heroToAttack = _heroesBuffer[_randomRange.Range(0, _heroesBuffer.Count)];
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
            defender.TakeDamage(attacker.Attack);
            CheckBattleResult();
        }

        void CheckBattleResult()
        {
            var allHeroesDied = true;
            var allEnemiesDied = true;
            foreach (var hero in _heroes)
            {
                if (hero.Hp <= 0)
                    continue;
                allHeroesDied = false;
                break;
            }

            foreach (var enemy in _enemies)
            {
                if (enemy.Hp <= 0)
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
            if (OnDefeat != null)
                OnDefeat();
        }

        void ProcessVictory()
        {
            View.OnHeroTap -= OnHeroTap;
            foreach (var hero in _heroes)
            {
                if(hero.Hp <= 0)
                    continue;
                hero.AddExperience();
            }
            View.ProcessVictory();
            if (OnVictory != null)
                OnVictory();
        }

        ~BattleController()
        {
            if(View != null)
                View.OnHeroTap -= OnHeroTap;
        }
    }
}