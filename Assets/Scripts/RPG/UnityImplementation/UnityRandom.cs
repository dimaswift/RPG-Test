using RPG.Controller;
using UnityEngine;

namespace RPG.UnityImplementation
{
    public class UnityRandom : IRandomRange
    {
        public int Range(int min, int max)
        {
            return Random.Range(min, max);
        }
    }
}