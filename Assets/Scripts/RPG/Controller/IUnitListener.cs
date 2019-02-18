namespace RPG.Controller
{
    public interface IUnitListener
    {
        void OnHpAmountChanged(float oldHp, float newHp);
        void OnDeath();
    }
}