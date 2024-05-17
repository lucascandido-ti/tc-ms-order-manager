namespace Domain.Queue.Interfaces
{
    public interface IQueueData<T>
    {
        string Pattern { get; set; }
        T Data { get; set; }
    }
}
