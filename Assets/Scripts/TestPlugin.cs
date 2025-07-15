using System;
using UnityEngine;
using UnityEngine.Events;

public class TestPlugin : MonoBehaviour
{
    private AndroidJavaClass _pluginClass;
    private static TestPlugin _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlugin();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitializePlugin()
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

    public void UpdateDevicePaired()
    {
        if (_pluginClass != null)
        {
            Debug.Log("TestPlugin -> getPairedDevices");
            _pluginClass.CallStatic("getPairedDevices");
        }
    }
    public static void ConnectToDevice(BluetoothDevice device)
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> connectToDevice");
            _instance._pluginClass.CallStatic("connectToDevice", device);
        }
        else
        {
            Debug.LogError("TestPlugin instance or plugin not initialized");
        }
    }
}