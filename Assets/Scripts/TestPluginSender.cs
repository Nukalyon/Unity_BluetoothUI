using UnityEngine;

public class TestPluginSender : MonoBehaviour
{
    static AndroidJavaObject pluginClass = new AndroidJavaObject("com.example.plugin.BluetoothUnityBridge");
    static bool isInitialized = false;
    
    void Start()
    {
        using (pluginClass)
        {
            if (!isInitialized)
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                pluginClass.CallStatic("init", unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"));
                //Call isInitialized() function to be sure here
                Debug.Log("Bluetooth plugin initialized.");
                isInitialized = true;
            }
        }
    }
    
    public static void notify()
    {
        if (isInitialized)
        {
            pluginClass.CallStatic("notifyUnity", "TestPluginSender");
        }
    }
}
