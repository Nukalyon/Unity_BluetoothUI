using System;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            Debug.Log("TestPlugin -> InitializePlugin");
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
            Debug.Log("TestPlugin -> Showing Toast");
            _pluginClass.CallStatic("showToast", "showToast called from Unity");
        }
    }

    public void StartScan()
    {
        if (_pluginClass != null)
        {
            Debug.Log("TestPlugin -> StartScan");
            _pluginClass.CallStatic("startScan");
        }
    }
    
    public void StopScan()
    {
        if (_pluginClass != null)
        {
            Debug.Log("TestPlugin -> StopScan");
            _pluginClass.CallStatic("stopScan");
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
            //string toConnect = JsonUtility.ToJson(device, true);
            string toConnect = JsonConvert.SerializeObject(device);
            Debug.Log("Device serialized : " + toConnect);
            _instance._pluginClass.CallStatic("connectToDevice", toConnect);
        }
        else
        {
            Debug.LogError("TestPlugin instance or plugin not initialized");
        }
    }

    public static string GetRegex()
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> getRegex");
            return _instance._pluginClass.CallStatic<string>("getRegex");
        }
        return null;
    }

    public static void SetRegex(string regex)
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> setRegex");
            _instance._pluginClass.CallStatic("setRegex", regex);
        }
    }

    public static void SetVisibility(Image imgVisible)
    {
        bool res = false;
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> getVisibility");
            res = _instance._pluginClass.CallStatic<bool>("isDeviceVisible");
        }
        else
        {
            Debug.LogError("TestPlugin instance or plugin not initialized");
        }
        imgVisible.color = res ? Color.green : Color.red;
    }

    public static void Reset(TMP_InputField inputField)
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> reset");
            SetRegex(".*");
            inputField.SetTextWithoutNotify(GetRegex());
        }
    }
}