using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeatherToast : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("SDK Providers")]
    [SerializeField] private LocationProvider locationProvider;

    private InputAction tapAction;
    private Camera cam;

    private readonly float _toastCooldown = 1.2f;
    private float _lastToastTime;

    private ILocationService location;
    private IWeatherService weather;
    private IToastService toast;

    private bool locationReady;

    private void Awake()
    {
        location = new CleverTapLocationService(locationProvider);
        weather = new CleverTapWeatherService();
        toast = new CleverTapToastService();

    }

    private async void Start()
    {
        cam = Camera.main;

        while (!location.IsReady)
            await Task.Delay(100);

        locationReady = true;
        weather.Initialize();
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
        if (Time.time - _lastToastTime < _toastCooldown)
            return;

        _lastToastTime = Time.time;

        if (!IsTappedObjectThis())
            return;

        HandleWeatherTap();
    }

    private void HandleWeatherTap()
    {
        if (!locationReady)
        {
            toast.ShowMessage("Location Not Enabled.");
            return;
        }

        weather.ShowCurrentTemperature(location.Latitude, location.Longitude);
    }

    private bool IsTappedObjectThis()
    {
        Vector2 screenPos = GetPointerPosition();
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out var hit))
        {
            return hit.transform == transform;
        }
        return false;
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
