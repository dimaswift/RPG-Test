using System;
using System.Collections.Generic;
using RPG.Controller;

namespace RPG.Model
{
    [Serializable]
    public class BattleSnapshot
    {
        public int Level;
        public List<UnitState> Enemies;
        public List<HeroState> Heroes;

        public BattleSnapshot(int level)
        {
            Enemies = new List<UnitState>();
            Heroes = new List<HeroState>();
            Level = level;
        }
        
        public BattleSnapshot(BattleController battleController)
        {
            Level = battleController.Level;
            Enemies = new List<UnitState>();
            Heroes = new List<HeroState>();
            foreach (var heroController in battleController.GetHeroes())
            {
                Heroes.Add(heroController.HeroState);
            }
            foreach (var heroController in battleController.GetEnemies())
            {
                Enemies.Add(heroController.State);
            }
        }
    }
}