using System.Collections.Generic;
using RPG.Controller;
using UnityEngine;

namespace RPG.View
{
    public class BattleView : ControlledView<BattleController>, IBattleListener
    {
        [SerializeField] UnitView[] _enemies;
        [SerializeField] UnitView[] _heroes;
        [SerializeField] GameObject _victoryWindow;
        [SerializeField] GameObject _defeatWindow;

        readonly List<UnitView> _activeUnits = new List<UnitView>();

        protected override void OnSetUp()
        {
            base.OnSetUp();
            foreach (var enemy in _enemies)
            {
                enemy.gameObject.SetActive(false);
            }
            foreach (var hero in _heroes)
            {
                hero.gameObject.SetActive(false);
            }
            _activeUnits.Clear();
            foreach (var enemy in Controller.GetEnemies())
            {
                var enemyView = GetEnemyView();
                enemyView.SetUp(enemy);
                _activeUnits.Add(enemyView);
            }

            foreach (var hero in Controller.GetHeroes())
            {
                var heroView = GetHeroView();
                heroView.SetUp(hero);
                _activeUnits.Add(heroView);
            }
        }

        UnitView GetEnemyView()
        {
            foreach (var enemy in _enemies)
            {
                if (!enemy.gameObject.activeSelf)
                    return enemy;
            }

            Debug.LogError("Not enough UnitView instances for enemy controllers inside BattleView. Add more!");
            return null;
        }

        UnitView GetHeroView()
        {
            foreach (var hero in _heroes)
            {
                if (!hero.gameObject.activeSelf)
                    return hero;
            }

            Debug.LogError("Not enough UnitView instances for hero controllers inside BattleView. Add more!");
            return null;
        }
        
        public override void Render()
        {
            foreach (var unitView in _activeUnits)
            {
                unitView.Render();
            }
        }

        public void OnDefeat()
        {
            _defeatWindow.SetActive(true);
        }

        public void OnVictory()
        {
            _victoryWindow.SetActive(true);
        }
    }
}