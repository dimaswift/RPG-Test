namespace RPG.Controller
{
    public interface IBattleListener
    {
        void OnDefeat();
        void OnVictory();
    }
}