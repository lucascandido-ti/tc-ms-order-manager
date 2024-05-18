
using Newtonsoft.Json;

namespace Queue.Utils
{
    public class ResponseFormat<T>
    {
        public string pattern { get; set; }
        public T data { get; set; }
    }


    public interface IQueueData<T>
    {
        string pattern { get; set; }
        T data { get; set; }
    }

    public class QueueData<T> : IQueueData<T>
    {
        public string pattern { get; set; }
        public T data { get; set; }
    }

    public static class JsonParser
    {
        public static IQueueData<T> ParseMessage<T>(string message)
        {
            return JsonConvert.DeserializeObject<QueueData<T>>(message);
        }
    }
}
