namespace FanControl.PubSub
{
    public interface IPublisher<T>
    {
        void Publish(T newEvent);
    }
}