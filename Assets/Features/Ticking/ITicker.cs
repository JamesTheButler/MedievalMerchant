namespace Features.Ticking
{
    public interface ITicker
    {
        void Initialize(int ticksPerDay);
        void Tick();
    }
}