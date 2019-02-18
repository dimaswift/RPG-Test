namespace RPG.Utils
{
    public interface IPoolable
    {
        void Init();
        void PickFromPool();
        void ReturnToPool();
        bool IsActive { get; }
    }
}