using System;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main class that will be the bridge between the plugin and the Unity scripts
/// Be sure that when you want to call a method in the plugin, the JVMName is the same
/// </summary>
public class TestPlugin : MonoBehaviour
{
    private AndroidJavaClass _pluginClass;
    private static TestPlugin _instance;
    private void Awake()
    {
        // Singleton
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
        // Try to load the Custom UnityPlayer in the plugin as ref in _pluginClass
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

    /// <summary>
    /// Link to the method showToast in the plugin
    /// Launch a Toast whe a simple message
    /// </summary>
    public void ShowToast()
    {
        if (_pluginClass != null)
        {
            Debug.Log("TestPlugin -> Showing Toast");
            _pluginClass.CallStatic("showToast", "showToast called from Unity");
        }
    }
    
    /// <summary>
    /// This method start the discovery of bluetooth devices
    /// </summary>
    public static void StartScan()
    {
        if (_instance !=null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> StartScan");
            _instance._pluginClass.CallStatic("startScan");
        }
    }
    
    /// <summary>
    /// This method stop the discovery of bluetooth devices
    /// </summary>
    public static void StopScan()
    {
        if (_instance !=null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> StopScan");
            _instance._pluginClass.CallStatic("stopScan");
        }
    }
    
    /// <summary>
    /// Called to update the paired devices
    /// </summary>
    public static void UpdateDevicePaired()
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> getPairedDevices");
            _instance._pluginClass.CallStatic("getPairedDevices");
        }
    }
    
    /// <summary>
    /// Try to connect to the target BluetoothDevice (paired or scanned)
    /// Need to serialize it otherwise the plugin doesn't understand the device as is
    /// </summary>
    /// <param name="device"> Device to connect </param>
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

    /// <summary>
    /// Try to create a pairing between the main device and the target.
    /// </summary>
    /// <param name="device"> Device targeted for pairing </param>
    public static void PairToDevice(BluetoothDevice device)
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> pairToDevice");
            string toPair = JsonConvert.SerializeObject(device);
            Debug.Log("Device serialized : " + toPair);
            _instance._pluginClass.CallStatic("pairToDevice", toPair);
        }
        else
        {
            Debug.LogError("TestPlugin instance or plugin not initialized");
        }
    }
    
    /// <summary>
    /// Disconnect for the device connected
    /// </summary>
    public static void DisconnectFromDevice()
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> disconnectFromDevice");
            _instance._pluginClass.CallStatic("disconnect");
        }
        else
        {
            Debug.LogError("TestPlugin instance or plugin not initialized");
        }
    }
    
    /// <summary>
    /// Try to reconnect to the device lastly disconnected
    /// </summary>
    public static void Reconnect()
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> Reconnect");
            _instance._pluginClass.CallStatic("retryConnection");
        }
    }
    
    /// <summary>
    /// Try to send the text from the inputField to the other device (server)
    /// </summary>
    /// <param name="inputField"> Inputfield of the message to send </param>
    public static void SendMessageToServer(TMP_InputField inputField)
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> SendMessageToServer");
            _instance._pluginClass.CallStatic("sendMessage", inputField.text.Trim());
            inputField.text = "";
        }
        else
        {
            Debug.LogError("TestPlugin instance or plugin not initialized");
        }
    }
    
    /// <summary>
    /// get the regex from the plugin
    /// </summary>
    /// <returns>string : Representing the regex </returns>
    public static string GetRegex()
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> getRegex");
            return _instance._pluginClass.CallStatic<string>("getRegex");
        }
        return null;
    }
    
    /// <summary>
    /// Set the new regex inside the plugin
    /// </summary>
    /// <param name="regex"> string extracted from an InputField </param>
    public static void SetRegex(string regex)
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> setRegex");
            _instance._pluginClass.CallStatic("setRegex", regex);
        }
    }
    
    /// <summary>
    /// Request to the user to make the device visible
    /// </summary>
    public static void RequestVisibility()
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> getVisibility");
            _instance._pluginClass.CallStatic("requestVisibility");
        }
        else
        {
            Debug.LogError("TestPlugin instance or plugin not initialized");
        }
    }

    /// <summary>
    /// Reset the Regex in the plugin and update the inputField text
    /// </summary>
    /// <param name="inputField"> target for Regex display </param>
    public static void ResetInput(TMP_InputField inputField)
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> reset");
            SetRegex(".*");
            inputField.SetTextWithoutNotify(GetRegex());
        }
    }
    
    public void StartServer()
    {
        if (_pluginClass != null)
        {
            Debug.Log("TestPlugin -> StartServer");
            _pluginClass.CallStatic("startServer");
        }
    }
    
    /// <summary>
    /// Ask the plugin if the bluetooth is enabled
    /// If the plugin doesn't load, keep it at false
    /// </summary>
    /// <returns></returns>
    public static bool GetBluetoothStatus()
    {
        bool res = false;
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> GetBluetoothStatus");
            res = _instance._pluginClass.CallStatic<bool>("getBluetoothStatus");
        }
        return res;
    }
    
    /// <summary>
    /// Request to the user to let the device be visible
    /// </summary>
    public static void RequestBluetoothActivation()
    {
        if (_instance != null && _instance._pluginClass != null)
        {
            Debug.Log("TestPlugin -> RequestBluetoothActivation");
            _instance._pluginClass.CallStatic("requestBluetoothActivation");
        }
    }
}