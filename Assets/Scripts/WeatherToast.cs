using System.Threading.Tasks;
using CleverTap.WeatherSDK.WeatherAPI;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeatherToast : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private LocationProvider location;
    private float toastCooldown = 1.2f;
    private float lastToastTime = 0f;

    private InputAction tapAction;
    private Camera cam;
    private bool _locationReady = false;

    private async void Start()
    {
        cam = Camera.main;

        while (!location.IsReady)
            await Task.Delay(100);

        _locationReady = true;
    }

    private void OnEnable()
    {
        var map = inputActions.FindActionMap("Gameplay", true);
        tapAction = map.FindAction("Tap", true);

        map.Enable();
        tapAction.performed += OnTap;
    }

    private void OnDisable()
    {
        if (tapAction != null)
            tapAction.performed -= OnTap;
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        if (Time.time - lastToastTime < toastCooldown)
            return; // prevent spam

        lastToastTime = Time.time;
        Vector2 screenPos = GetPointerPosition();

        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform == transform)
            {
                Debug.Log("Cube tapped!");

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

    private Vector2 GetPointerPosition()
    {
        if (Touchscreen.current != null)
            return Touchscreen.current.primaryTouch.position.ReadValue();

        if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        return Vector2.zero;
    }

}
