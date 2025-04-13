namespace Core
{
    public interface ITickService
    {
        float TickRate { get; }
        void SetTickRate(float newRate);
    }
}