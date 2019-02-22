using RPG.Controller;
using RPG.Model;

namespace RPG.UnitTests
{
    class DummyProfileProvider : IProfileProvider
    {
        PlayerProfile _profile = new PlayerProfile();
        public PlayerProfile LoadProfile()
        {
            return _profile;
        }

        public void SaveProfile(PlayerProfile profile)
        {
            _profile = profile;
        }

        public void Delete()
        {
				
        }
    }
}