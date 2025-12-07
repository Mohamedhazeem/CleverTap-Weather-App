using System.Threading.Tasks;
using CleverTap.WeatherSDK.WeatherAPI;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeatherToast : MonoBehaviour
{
    public LocationProvider location;   // assign in Inspector

    private bool _locationReady = false;
    Camera cam;

    private async void Start()
    {
        cam = Camera.main;
        while (!location.IsReady)
            await Task.Delay(100);

        _locationReady = true;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true)
        {
            HandleTap();
        }
    }
    private Vector2 GetPointerPosition()
    {

        if (Touchscreen.current != null && Touchscreen.current.press.isPressed)
        {
            return Touchscreen.current.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            return Mouse.current.position.ReadValue();
        }
        return Vector2.zero; // fallback
    }
    void HandleTap()
    {
        Vector2 pos = GetPointerPosition();

        Ray ray = cam.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                Debug.Log("working");

                if (!_locationReady)
                {
                    WeatherManager.Instance.ShowMessageToast("Location Not Enabled.");
                    return;
                }

                float lat = location.Latitude;
                float lon = location.Longitude;

                WeatherManager.Instance.ShowCurrentTemperatureToast(lat, lon);
            }
        }
    }
}
