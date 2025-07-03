using System;
using UnityEngine;

public class TestPluginSender : MonoBehaviour
{
    private AndroidJavaClass _pluginClass;
    private bool _isInitialized = false;

    private void Start()
    {
        // Initialize the Android plugin
        try
        {
            //var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _pluginClass = new AndroidJavaClass("com.example.plugin.MyUnityPlayer");
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
        if (_isInitialized)
            try
            {
                _pluginClass.Call("notifyUnity", "TestPluginSender");
            }
            catch (Exception e)
            {
                Debug.LogError("Error calling notifyUnity: " + e.Message);
            }
        else
            Debug.LogWarning("Bluetooth plugin is not initialized.");
    }

    public void ShowToast()
    {
        if (_pluginClass != null)
        {
            _pluginClass.CallStatic("showToast", "showToast called from Unity");
        }
    }
}