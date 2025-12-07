using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class LocationProvider : MonoBehaviour
{
    public float Latitude { get; private set; }
    public float Longitude { get; private set; }

    public bool IsReady { get; private set; }
    public bool HasError { get; private set; }
    public string ErrorMessage { get; private set; }

    private static readonly WaitForSeconds WaitOneSecond = new(1);

    private IEnumerator Start()
    {
        yield return RequestPermissionRoutine();

        if (HasError)
            yield break;

        yield return StartLocationServiceRoutine();

        if (!IsReady)
            Debug.LogWarning($"[LocationProvider] Failed: {ErrorMessage}");
    }

    private IEnumerator RequestPermissionRoutine()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);

            // Wait until user responds
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                yield return null;
        }
#endif
        yield break;
    }

    private IEnumerator StartLocationServiceRoutine()
    {
        Input.location.Start();

        const int timeoutSeconds = 10;
        int timer = timeoutSeconds;

        while (Input.location.status == LocationServiceStatus.Initializing && timer > 0)
        {
            yield return WaitOneSecond;
            timer--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            SetError("Location service failed to start.");
            yield break;
        }

        if (Input.location.status != LocationServiceStatus.Running)
        {
            SetError("Location service unavailable.");
            yield break;
        }

        // SUCCESS
        Latitude = Input.location.lastData.latitude;
        Longitude = Input.location.lastData.longitude;
        IsReady = true;
    }

    private void SetError(string msg)
    {
        ErrorMessage = msg;
        HasError = true;
        IsReady = false;
        Debug.LogError("[LocationProvider] " + msg);
    }
}
