namespace FanControl.PubSub
{
    public class SkipDuplicatesPublisher<T> : IPublisher<T>
    {
        private static readonly ILogger Log = LoggerProvider.GetInstance(); 
        private readonly IPublisher<T> publisher;
        private T? currentEvent;

        public SkipDuplicatesPublisher(IPublisher<T> publisher)
        {
            this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public void Publish(T newEvent)
        {
            if (!object.Equals(currentEvent, newEvent))
            {
                publisher.Publish(newEvent);
                this.currentEvent = newEvent;
            }else{
                Log.Trace("Temperature have not changed. Skip.");
            }
        }
    }
}