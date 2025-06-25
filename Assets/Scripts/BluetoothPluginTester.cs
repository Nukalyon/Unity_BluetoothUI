using UnityEngine;

public class BluetoothPluginTester : MonoBehaviour
{
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    try 
	{	        
        using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.plugin.BluetoothUnityBridge"))
        {
            using (AndroidJavaObject unityActivity = GetUnityActivity())
            {
                pluginClass.CallStatic("init", unityActivity);
                Debug.Log("Bluetooth plugin initialized.");
            }

            pluginClass.CallStatic("startScan");
            Debug.Log("Started scanning.");
        }
	}
    catch (System.Exception e)
    {
        Debug.LogError("Bluetooth plugin error: " + e.Message);
    }
#else
    Debug.Log("Must be running on an Android device.");
#endif
    }

    private AndroidJavaObject GetUnityActivity()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
}
