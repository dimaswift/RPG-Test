using System;
using RPG.Controller;

namespace RPG.UnitTests
{
    class RandomRange : IRandomRange
    {
        readonly Random _random;
        public RandomRange()
        {
            _random = new Random();
        }
        public int Range(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}