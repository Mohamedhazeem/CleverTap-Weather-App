using CleverTap.WeatherSDK.WeatherAPI;

public class CleverTapToastService : IToastService
{
    public void ShowMessage(string message)
    {
        WeatherManager.Instance.ShowMessageToast(message);
    }
}