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
    private static WaitForSeconds _waitForSeconds = new(1);

    private IEnumerator Start()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);

            // Wait for user to respond
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                yield return null;
        }

#endif

        Input.location.Start();

        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return _waitForSeconds;
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location.");
            yield break;
        }

        Latitude = Input.location.lastData.latitude;
        Longitude = Input.location.lastData.longitude;
        IsReady = true;
    }
}
