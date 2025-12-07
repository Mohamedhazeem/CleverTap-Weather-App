using CleverTap.WeatherSDK.WeatherAPI;
using UnityEngine;

public class WeatherToast : MonoBehaviour
{
    public LocationProvider location;   // assign in Inspector

    private bool _locationReady = false;

    private async void Start()
    {
        // Wait for GPS to be ready
        while (!location.IsReady)
            await System.Threading.Tasks.Task.Delay(100);

        _locationReady = true;
    }

    private async void OnMouseDown()
    {
        if (!_locationReady)
        {
            Debug.Log("Location not ready yet!");
            return;
        }

        float lat = location.Latitude;
        float lon = location.Longitude;
        // Show toast using SDK
        await WeatherManager.Instance.ShowCurrentTemperatureToast(lat, lon);
    }
}
