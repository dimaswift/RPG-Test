using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Controller;
using RPG.Model;
using UnityEngine;

namespace RPG.View
{
    public class BattleView : View, IBattleView
    {
        [SerializeField] UnitView[] _enemies;
        [SerializeField] UnitView[] _heroes;
        [SerializeField] GameObject _victoryWindow;
        [SerializeField] GameObject _defeatWindow;
        [SerializeField] AnimationCurve _attackCurve;

        public event Action<HeroState> OnHeroTap;
        
        readonly List<UnitView> _activeUnits = new List<UnitView>();

        UnitView PickFreeEnemyView()
        {
            foreach (var enemy in _enemies)
            {
                if (!enemy.gameObject.activeSelf)
                {
                    enemy.gameObject.SetActive(true);
                    return enemy;
                }
                
            }

            Debug.LogError("Not enough UnitView instances for enemy controllers inside BattleView. Add more!");
            return null;
        }

        UnitView PickFreeHeroView()
        {
            foreach (var hero in _heroes)
            {
                if (!hero.gameObject.activeSelf)
                {
                    hero.gameObject.SetActive(true);
                    return hero;
                }
            }

            Debug.LogError("Not enough UnitView instances for hero controllers inside BattleView. Add more!");
            return null;
        }
        
        UnitView GetUnitView(UnitState state)
        {
            foreach (var unit in _activeUnits)
            {
                if (unit.Controller.State.Id == state.Id)
                    return unit;
            }

            return null;
        }
        
        public override void Render()
        {
            foreach (var unitView in _activeUnits)
            {
                unitView.Render();
            }
        }

        public void StartAttack(UnitState attacker, UnitState defender, Action onAttackFinish)
        {
            var attackerView = GetUnitView(attacker);
            var defenderView = GetUnitView(defender);
            StartCoroutine(AttackAnimation(attackerView, defenderView, onAttackFinish));
        }

        public void EndAttack(UnitState attacker, UnitState defender)
        {
            GetUnitView(attacker).Render();
            GetUnitView(defender).Render();
        }

        public void ProcessDefeat()
        {
            _defeatWindow.SetActive(true);
        }

        public void ProcessVictory()
        {
            _victoryWindow.SetActive(true);
        }

        public void PrepareBattle(IEnumerable<HeroController> heroes, IEnumerable<UnitController> enemies)
        {
            _defeatWindow.SetActive(false);
            _victoryWindow.SetActive(false);
           
            foreach (var enemy in _enemies)
            {
                enemy.gameObject.SetActive(false);
            }
            foreach (var hero in _heroes)
            {
                hero.gameObject.SetActive(false);
            }
            _activeUnits.Clear();
            foreach (var enemy in enemies)
            {
                var enemyView = PickFreeEnemyView();
                enemyView.SetUp(enemy);
                _activeUnits.Add(enemyView);
            }

            foreach (var hero in heroes)
            {
                var heroView = PickFreeHeroView();
                heroView.SetUp(hero);
                _activeUnits.Add(heroView);
            }
            
            Show();
        }


        IEnumerator AttackAnimation(UnitView attacker, UnitView defender, Action onFinish)
        {
            var attackerStartPos = attacker.transform.position;
            yield return MoveTo(attacker.transform, defender.transform.position);
            onFinish();
            yield return MoveTo(attacker.transform, attackerStartPos);
        }

        IEnumerator MoveTo(Transform movingTransform, Vector3 target)
        {
            var t = 0f;
            var origin = movingTransform.position;
            while (t <= 1f)
            {
                var v = _attackCurve.Evaluate(t);
                movingTransform.position = Vector3.Lerp(origin, target, v);
                t += Time.deltaTime;
                yield return null;
            }
            movingTransform.position = target;
        }

        public void TapOnUnit(UnitView unitView)
        {
            if (OnHeroTap != null)
            {
                var heroController = unitView.Controller as HeroController;
                if (heroController != null)
                {
                    OnHeroTap(heroController.HeroState);
                }
            }
        }
    }
}