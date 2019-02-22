using RPG.View;

namespace RPG.UnitTests
{
    class UnitDebugView : IUnitView
    {
        readonly string _name;
        readonly ITestLogger _logger;

        public UnitDebugView(string name, ITestLogger testLogger)
        {
            _logger = testLogger;
            _name = name;
        }
			
        public void Render()
        {
				
        }

        public void OnHpAmountChanged(float oldHp, float newHp)
        {
            _logger.Log(string.Format("{2} hp changed from {0:0.0} to {1:0.0}", oldHp, newHp, _name));
        }

        public void OnDeath()
        {
            _logger.Log(_name + " died!");
        }
    }
}