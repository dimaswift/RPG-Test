using System;
using System.Collections.Generic;
using RPG.Controller;

namespace RPG.Model
{
    [Serializable]
    public class BattleSnapshot
    {
        public int Level;
        public List<UnitConfig> Enemies;
        public List<HeroState> Heroes;

        public BattleSnapshot(int level)
        {
            Enemies = new List<UnitConfig>();
            Heroes = new List<HeroState>();
            Level = level;
        }
        
        public BattleSnapshot(BattleController battleController)
        {
            Level = battleController.Level;
            Enemies = new List<UnitConfig>();
            Heroes = new List<HeroState>();
            foreach (var controller in battleController.GetHeroes())
            {
                Heroes.Add(controller.HeroState);
            }
            foreach (var controller in battleController.GetEnemies())
            {
                Enemies.Add(controller.Config);
            }
        }
    }
}