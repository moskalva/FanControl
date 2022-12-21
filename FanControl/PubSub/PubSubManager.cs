namespace FanControl.PubSub
{
    public class PubSubManager<T> : IPublisher<T>
    {
        private static readonly ILogger Log = LoggerProvider.GetInstance(); 
        private readonly List<ISubscriber<T>> subscribers = new List<ISubscriber<T>>(0);

        public ISubscriber<T> Subscribe(ISubscriber<T> subscriber)
        {
            if (subscriber is null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }
            Log.Info($"Added subscriber '{subscriber.GetType().Name}'");
            this.subscribers.Add(subscriber);
            return subscriber;
        }

        public void Publish(T newEvent)
        {
            if (newEvent is null)
            {
                throw new ArgumentNullException(nameof(newEvent));
            }

            foreach (var subscriber in this.subscribers)
            {
                try
                {
                    subscriber.OnEvent(newEvent);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}