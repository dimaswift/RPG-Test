using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Controller;
using RPG.Model;
using RPG.View;
using UnityEngine;

namespace ViewImplementation
{
    public class BattleView : View, IBattleView
    {
        [SerializeField] UnitView[] _enemies;
        [SerializeField] UnitView[] _heroes;
        [SerializeField] GameObject _victoryWindow;
        [SerializeField] GameObject _defeatWindow;
        [SerializeField] AnimationCurve _attackCurve;
        [SerializeField] float _attackTime = .5f;

        public event Action<HeroState> OnHeroTap;
        
        readonly List<UnitView> _activeUnitViews = new List<UnitView>();

       
        
        public override void Render()
        {
            foreach (var unitView in _activeUnitViews)
            {
                unitView.Render();
            }
        }

        public void StartAttack(UnitController attacker,
            UnitController defender, 
            Action damageDealtCallback,
            Action animationFinishedCallback)
        {
            var attackerView = FindUnitView(attacker);
            var defenderView = FindUnitView(defender);
            StartCoroutine(AttackAnimation(attackerView, defenderView, damageDealtCallback, animationFinishedCallback));
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
            _activeUnitViews.Clear();
            foreach (var enemy in enemies)
            {
                var enemyView = PickFreeEnemyView();
                enemyView.SetUp(enemy);
                _activeUnitViews.Add(enemyView);
            }

            foreach (var hero in heroes)
            {
                var heroView = PickFreeHeroView();
                heroView.SetUp(hero);
                _activeUnitViews.Add(heroView);
            }
            
            Show();
        }


        IEnumerator AttackAnimation(UnitView attacker, UnitView defender, Action damageDealtCallback, Action animationFinishedCallback)
        {
            var attackerStartPos = attacker.transform.position;
            yield return MoveTo(attacker.transform, defender.transform.position, Render);
            damageDealtCallback();
            yield return MoveTo(attacker.transform, attackerStartPos, Render);
            animationFinishedCallback();
        }

        IEnumerator MoveTo(Transform movingTransform, Vector3 target, Action onReach)
        {
            var t = 0f;
            var origin = movingTransform.position;
            while (t <= 1f)
            {
                var v = _attackCurve.Evaluate(t);
                movingTransform.position = Vector3.Lerp(origin, target, v);
                t += Time.deltaTime / _attackTime;
                yield return null;
            }
            movingTransform.position = target;
            onReach();
        } 
        
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
        
        UnitView FindUnitView(UnitController unitController)
        {
            foreach (var unitView in _activeUnitViews)
            {
                if (unitView.Controller.State == unitController.State)
                    return unitView;
            }

            return null;
        }

        public void Restart()
        {
            Game.ShowCollectionMenu();
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