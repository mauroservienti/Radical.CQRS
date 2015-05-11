namespace Radical.CQRS
{
    public interface IHaveState<TState>
    {
        TState State { get; set; }
    }
}
