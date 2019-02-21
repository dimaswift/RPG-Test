using System.Collections.Generic;
using RPG.Model;

namespace RPG.Controller
{
    public class UnitFactory
    {
        readonly IRandomRange _randomRange;
        readonly int _visualsAmount;
        
        public UnitFactory(IRandomRange randomRange, int visualsAmount)
        {
            _visualsAmount = visualsAmount;
            _randomRange = randomRange;
        }

        public HeroData CreateRandomHeroData(RandomUnitGeneratorConfig generatorConfig)
        {
            var data = new HeroData();
            data.Name = GetRandomUnitName("Hero");
            data.Attack = _randomRange.Range(generatorConfig.MinAttack, generatorConfig.MaxAttack);
            data.Hp = _randomRange.Range(generatorConfig.MinHp, generatorConfig.MaxHp);
            data.VisualIndex = _randomRange.Range(0, _visualsAmount);
            return data;
        }

        public List<HeroData> CreateHeroDeck(int size, RandomUnitGeneratorConfig generatorConfig)
        {
            var deck = new List<HeroData>();

            for (int i = 0; i < size; i++)
            {
                var hero = CreateRandomHeroData(generatorConfig);
                deck.Add(hero);
            }
            return deck;
        }

        public UnitData CreateRandomUnitData(string namePrefix,
            RandomUnitGeneratorConfig generatorConfig, float multiplier = 1)
        {
            var unitConfig = new UnitData();
            unitConfig.Name = GetRandomUnitName(namePrefix);
            unitConfig.Attack = _randomRange.Range(generatorConfig.MinAttack, generatorConfig.MaxAttack) * multiplier;
            unitConfig.Hp = _randomRange.Range(generatorConfig.MinHp, generatorConfig.MaxHp) * multiplier;
            unitConfig.VisualIndex = _randomRange.Range(0, _visualsAmount);
            return unitConfig;
        }
              
        string GetRandomUnitName(string prefix)
        {
            return string.Format("{0} {1}", prefix, _randomRange.Range(0, 1000));
        }
    }
}