using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Model
{
    [Serializable]
    public class RandomUnitGenerator
    {
        public int MinAttack = 1;
        public int MaxAttack = 10;
        public int MinHp = 1;
        public int MaxHp = 10;
		
        public void SetStats(UnitConfig config, float multiplier)
        {
            config.BaseAttack = Mathf.RoundToInt(Random.Range(MinAttack, MaxAttack) * multiplier);
            config.BaseHp = Mathf.RoundToInt(Random.Range(MinHp, MaxHp) * multiplier);
        }
    }
}