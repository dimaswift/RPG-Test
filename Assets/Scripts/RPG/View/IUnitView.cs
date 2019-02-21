namespace RPG.View
{
    public interface IUnitView : IView
    {
        void OnHpAmountChanged(float oldHp, float newHp);
        void OnDeath();
    }
}