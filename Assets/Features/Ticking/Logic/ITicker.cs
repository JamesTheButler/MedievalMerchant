namespace Features.Ticking.Logic
{
    public interface ITicker
    {
        void Initialize(int ticksPerDay);
        void Tick();
    }
}