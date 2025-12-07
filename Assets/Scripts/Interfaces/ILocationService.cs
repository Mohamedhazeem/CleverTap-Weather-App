public interface ILocationService
{
    bool IsReady { get; }
    float Latitude { get; }
    float Longitude { get; }
}