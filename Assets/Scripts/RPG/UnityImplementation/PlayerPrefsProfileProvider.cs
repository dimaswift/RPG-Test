using RPG.Controller;
using RPG.Model;
using UnityEngine;

namespace RPG.UnityImplementation
{
    public class PlayerPrefsProfileProvider : IProfileProvider
    {
        readonly string _key;
        readonly PlayerProfile _defaultProfile;
        
        public PlayerPrefsProfileProvider(string key, PlayerProfile defaultProfile = null)
        {
            _key = key;
            _defaultProfile = defaultProfile;
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
            PlayerPrefs.SetString(_key, JsonUtility.ToJson(profile));
            PlayerPrefs.Save();
        }

        public void Delete()
        {
            PlayerPrefs.DeleteKey(_key);
            PlayerPrefs.Save();
        }
    }
}