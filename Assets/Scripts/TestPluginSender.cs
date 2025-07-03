using System;
using UnityEngine;

public class TestPluginSender : MonoBehaviour
{
    private AndroidJavaClass pluginClass;
    private bool isInitialized = false;

    private void Start()
    {
        // Initialize the Android plugin
        try
        {
            //var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            pluginClass = new AndroidJavaClass("com.example.plugin.MyUnityPlayer");
            // if (pluginClass != null)
            // {
            //     pluginClass.Call("init", unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"));
            //     // Optionally check if initialization was successful
            //     isInitialized = true;
            //     Debug.Log("Bluetooth plugin initialized.");
            // }
            // else
            // {
            //     Debug.LogError("Failed to create instance of BluetoothUnityBridge.");
            // }
        }
        catch (Exception e)
        {
            Debug.LogError("Error initializing Bluetooth plugin: " + e.Message);
        }
    }

    public void Notify()
    {
        if (isInitialized)
            try
            {
                pluginClass.Call("notifyUnity", "TestPluginSender");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error calling notifyUnity: " + e.Message);
            }
        else
            Debug.LogWarning("Bluetooth plugin is not initialized.");
    }

    public void ShowToast()
    {
        if (pluginClass != null)
        {
            pluginClass.CallStatic("showToast", "showToast called from Unity");
        }
    }
}