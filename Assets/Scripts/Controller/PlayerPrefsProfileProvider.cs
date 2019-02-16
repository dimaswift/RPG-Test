using UnityEngine;

namespace RPG.Controller
{
    public class PlayerPrefsProfileProvider : IProfileProvider
    {
        readonly string _key;
        readonly PlayerProfile _defaultProfile;
        
        public PlayerPrefsProfileProvider(string key, PlayerProfile defaultProfile = null)
        {
            _key = key;
            _defaultProfile = null;
        }
        
        public PlayerProfile LoadProfile()
        {
            PlayerProfile profile;
            if (!PlayerPrefs.HasKey(_key))
            {
                profile = _defaultProfile ?? new PlayerProfile();
                SaveProfile(profile);
            }
            else
            {
                profile = JsonUtility.FromJson<PlayerProfile>(PlayerPrefs.GetString(_key));
            }
            return profile;
        }

        public void SaveProfile(PlayerProfile profile)
        {
            PlayerPrefs.GetString(_key, JsonUtility.ToJson(profile));
            PlayerPrefs.Save();
        }
    }
}