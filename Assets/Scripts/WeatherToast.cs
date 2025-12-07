using System.Threading.Tasks;
using CleverTap.WeatherSDK.WeatherAPI;
using UnityEngine;

public class WeatherToast : MonoBehaviour
{
    public LocationProvider location;   // assign in Inspector

    private bool _locationReady = false;

    private async void Start()
    {
        while (!location.IsReady)
            await Task.Delay(100);

        _locationReady = true;
    }

    private async void OnMouseDown()
    {
        if (!_locationReady)
        {
            Debug.Log("Location not ready yet!");
            WeatherManager.Instance.ShowMessageToast("Location Not Enabled.");
            return;
        }

        float lat = location.Latitude;
        float lon = location.Longitude;
        await WeatherManager.Instance.ShowCurrentTemperatureToast(lat, lon);
    }
}
