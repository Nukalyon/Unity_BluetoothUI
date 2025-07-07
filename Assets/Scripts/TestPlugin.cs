using System;
using UnityEngine;

public class TestPlugin : MonoBehaviour
{
    private AndroidJavaClass _pluginClass;

    private void Start()
    {
        try
        {
            _pluginClass = new AndroidJavaClass("com.example.plugin.MyUnityPlayer");
        }
        catch (Exception e)
        {
            Debug.LogError("Error initializing Bluetooth plugin: " + e.Message);
        }
    }

    public void ShowToast()
    {
        if (_pluginClass != null)
        {
            _pluginClass.CallStatic("showToast", "showToast called from Unity");
        }
    }
    /*
    public void Notify()
    {
        if (_permissionsGranted)
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
    }*/
}