namespace RPG.Controller
{
    public interface IProfileProvider
    {
        PlayerProfile LoadProfile();
        void SaveProfile(PlayerProfile profile);
    }
}