public interface ITimeService
{
    void Start();
    void Stop();
    int GetElapsedMinutes();
}