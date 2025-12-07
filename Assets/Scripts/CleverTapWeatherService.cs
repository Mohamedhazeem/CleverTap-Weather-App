using CleverTap.WeatherSDK.WeatherAPI;

public class CleverTapWeatherService : IWeatherService
{
    public void ShowCurrentTemperature(float lat, float lon)
    {
        WeatherManager.Instance.ShowCurrentTemperatureToast(lat, lon);
    }
}