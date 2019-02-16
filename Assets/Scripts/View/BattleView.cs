using RPG.Controller;
using RPG.Model;
using UnityEngine;

namespace RPG.View
{
    public class BattleView : View<BattleController>
    {
        [SerializeField] UnitView[] _enemies;
        [SerializeField] UnitView[] _heroes;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            foreach (var enemy in Controller.GetEnemies())
            {
                
            }
        }

        UnitView GetEnemyFromPool()
        {
            foreach (var enemy in _enemies)
            {
                if (!enemy.gameObject.activeSelf)
                    return enemy;
            }
        }

        public override void Render()
        {
            foreach (var enemy in Controller.GetEnemies())
            {
                
            }
        }
    }
}