public class CleverTapLocationService : ILocationService
{
    private readonly LocationProvider provider;

    public CleverTapLocationService(LocationProvider provider)
    {
        this.provider = provider;
    }

    public bool IsReady => provider.IsReady;
    public float Latitude => provider.Latitude;
    public float Longitude => provider.Longitude;
}




