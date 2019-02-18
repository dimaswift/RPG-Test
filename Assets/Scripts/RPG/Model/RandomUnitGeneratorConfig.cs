using System;

namespace RPG.Model
{
    [Serializable]
    public class RandomUnitGeneratorConfig
    {
        public int MinAttack = 1;
        public int MaxAttack = 10;
        public int MinHp = 1;
        public int MaxHp = 10;
    }
}