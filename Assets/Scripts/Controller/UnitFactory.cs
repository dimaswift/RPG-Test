using RPG.Model;

namespace RPG.Controller
{
    public class UnitFactory
    {
        protected readonly RandomUnitGenerator _generator;
        
        public UnitFactory(RandomUnitGenerator generator)
        {
            _generator = generator;
        }
    }
}