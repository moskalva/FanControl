namespace FanControl.PubSub
{
    public interface ISubscriber<T>
    {
        void OnEvent(T newEvent);
    }
}