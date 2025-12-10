namespace Infrastructure
{
    /// <summary>
    /// Systems use the data from models and the utilities of services to implement game logic. They typically don't need
    /// to be accessed by other classes outside of initialization/teardown.
    /// </summary>
    public interface ISystem : IInitializable { }
}