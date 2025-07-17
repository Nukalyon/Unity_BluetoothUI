using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BluetoothDeviceReceiver : MonoBehaviour
{
    private List<BluetoothDevice> _pairedDevices = new();
    public GameObject parentprefab;
    public GameObject buttonPrefab;
    
    // Device type to icon mapping (you'll need to replace with actual sprites)
    private static Dictionary<string, Sprite> _deviceIcons;
    
    void Start()
    {
        gameObject.name = "List_Paired_Devices";
        DontDestroyOnLoad(gameObject);
        if (_pairedDevices.Count != 0 && _pairedDevices != null)
        {
            _pairedDevices.Clear();
        }
        // Unparent all childs if they exist at the start
        gameObject.transform.DetachChildren();
        Debug.Assert(gameObject.transform.childCount == 0, "gameObject.transform.childCount == 0", this);
        _deviceIcons = new Dictionary<string, Sprite>
        {
            { "PHONE",    Resources.Load<Sprite>($"Images/Phone_Icon") },
            { "COMPUTER", Resources.Load<Sprite>($"Images/Computer_Icon") },
            { "AUDIO",    Resources.Load<Sprite>($"Images/Casque_Icon") },
            { "UNKNOWN",  Resources.Load<Sprite>($"Images/Unknown_Icon") }
        };
    }

    /// <summary>
    /// This method will be called from the Kotlin plugin.
    /// </summary>
    /// <param name="jsonDevices">The list of devices as json sent from the plugin</param>
    public void OnDevicesReceive(string jsonDevices)
    {
        Debug.Log("Received devices list: " + jsonDevices);
        
        // Decode the JSON
        BluetoothDevice[] devices = JsonUtility.FromJson<BluetoothDeviceArray>("{\"devices\":" + jsonDevices + "}").devices;

        // Process the devices as needed
        foreach (var device in devices)
        {
            if (!_pairedDevices.Contains(device))
            {
                _pairedDevices.Add(device);
                if (parentprefab != null && buttonPrefab != null)
                {
                    // Instanciate the DeviceButton
                    CreateDeviceButton(device);
                }
                else
                {
                    Debug.Log("parent prefab is null and / or buttonPrefab is null");
                }
            }
        }
    }

    private void CreateDeviceButton(BluetoothDevice device)
    {
        GameObject newButton = Instantiate(buttonPrefab, parentprefab.transform);
        newButton.GetComponent<DeviceButton>().deviceName.text = device.name;
        newButton.GetComponent<DeviceButton>().deviceAddress.text = device.address;
        newButton.GetComponent<DeviceButton>().deviceIcon.sprite =
            _deviceIcons.ContainsKey(device.deviceType.ToString()) 
                ? _deviceIcons[device.deviceType.ToString()] 
                : _deviceIcons["UNKNOWN"];
        // Which one ?
        newButton.GetComponent<Button>().onClick.AddListener(delegate { ConnectToDevice(device); });
        //newButton.GetComponent<Button>().onClick.AddListener(() => ConnectToDevice(device));
    }

    private void ConnectToDevice(BluetoothDevice device)
    {
        TestPlugin.ConnectToDevice(device);
    }
}

// Helper class to wrap the array for deserialization
[System.Serializable]
public class BluetoothDeviceArray
{
    public BluetoothDevice[] devices;
}