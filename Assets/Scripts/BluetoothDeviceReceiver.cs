using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothDeviceReceiver : MonoBehaviour
{
    private List<BluetoothDevice> _pairedDevices = new();
    private List<BluetoothDevice> _scannedDevices = new();
    // This GameObject is the parent who will receive the device found / paired
    public GameObject parentprefab;
    // Prefab for a BluetoothDevice, setup when a device is found / paired
    public GameObject buttonPrefab;
    
    // This enum is to switch the behaviour from Scanned and Paired
    // Maybe remove the Other but easy to find if the setup isn't done correctly
    public enum DeviceReceiver
    {
        Other,
        List_Paired_Devices,
        List_Scanned_Devices
    }
    // Dropdown in the editor for an easy setup
    // Init to Other
    public DeviceReceiver dropDown = DeviceReceiver.Other;
    
    private static Dictionary<string, Sprite> _deviceIcons;
    
    /// <summary>
    /// For this script to work, be sure your gameObject is correctly named :
    /// -   List_Paired_Devices             MyUnityPlayer.kt -> getPairedDevices()
    ///         or
    /// -   List_Scanned_Devices            CustomBluetoothController -> receiverDeviceFound update
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Clear all the paired and scanned prefab in the editor
        ClearPairedDeviceButton();
        ClearScannedDeviceButton();
        // Just to be sure the prefab used in the editor are deleted on launch
        Debug.Assert(gameObject.transform.childCount == 0, "gameObject.transform.childCount == 0", this);
        
        // Load resources from the folder
        // I never check if the resource is found or not
        _deviceIcons = new Dictionary<string, Sprite>
        {
            { "PHONE",    Resources.Load<Sprite>($"Images/Phone_Icon") },
            { "COMPUTER", Resources.Load<Sprite>($"Images/Computer_Icon") },
            { "AUDIO",    Resources.Load<Sprite>($"Images/Casque_Icon") },
            { "UNKNOWN",  Resources.Load<Sprite>($"Images/Unknown_Icon") }
        };
        // Check if the user changed the dropdown value
        if (dropDown != DeviceReceiver.Other)
        {
            gameObject.name = dropDown.ToString();
        }
        else
        {
            Debug.LogError("Drop down device receiver should not be Other");
        }
    }
    
    /// <summary>
    /// This method will clear the list of the paired devices for a fresh start.
    /// It will be called at the Start and when the main UI will be Enabled
    /// </summary>
    private void ClearPairedDeviceButton()
    {
        _pairedDevices.Clear();
        foreach (Transform child in parentprefab.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    /// <summary>
    /// This method is called before the app receive a device from the plugin.
    /// It will clear the scanned devices for a fresh start when the scan UI is quit.
    /// </summary>
    private void ClearScannedDeviceButton()
    {
        _scannedDevices.Clear();
        foreach (Transform child in parentprefab.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// This method will be called from the Kotlin plugin for getting paired devices.
    /// </summary>
    /// <param name="jsonDevices">The list of devices as json sent from the plugin</param>
    public void OnDevicesPairedReceive(string jsonDevices)
    {
        Debug.Log("Received devices list: " + jsonDevices);
        
        // Decode the JSON
        BluetoothDevice[] devices = JsonUtility.FromJson<BluetoothDeviceArray>("{\"devices\":" + jsonDevices + "}").devices;
        ClearPairedDeviceButton();
        
        DoLogicWithList(_pairedDevices, devices);
    }
    
    
    /// <summary>
    /// This method will be called from the Kotlin plugin while scanning for new devices.
    /// </summary>
    /// <param name="jsonDevice">The device found as json sent from the plugin</param>
    private void OnDevicesScannedReceive(string jsonDevice)
    {
        Debug.Log("New device detected : " + jsonDevice);
        
        // Decode the JSON
        //  Not pretty but eh
        BluetoothDevice[] device = new BluetoothDevice[1];
        device[0] = JsonUtility.FromJson<BluetoothDevice>(jsonDevice);
        // I decode as a List but there is only one device in it, index = 0
        if (!_pairedDevices.Contains(device[0]))
        {
            DoLogicWithList(_scannedDevices, device);
        }
    }
    
    /// <summary>
    /// Called when the parent prefab is being enabled, depending on the dropdown value a clear is done and
    /// a call to a specific function in the TestPlugin file
    /// This function is also called when the bluetooth is disabled and enabled again
    /// For example : UI = Paired Device, onEnable we need to clear the paired device and reset it by updating
    /// </summary>
    private void OnEnable()
    {
        Debug.LogWarning(dropDown + " -> OnEnable");
        switch (dropDown)
        {
            case DeviceReceiver.List_Paired_Devices:
                ClearPairedDeviceButton();
                TestPlugin.UpdateDevicePaired();
                break;
            case DeviceReceiver.List_Scanned_Devices:
                ClearScannedDeviceButton();
                TestPlugin.StartScan();
                break;
            default:
                Debug.LogError("Unknown device receiver type");
                break;
        }
    }

    /// <summary>
    /// This method will help to add the device to the current list if there is no copy of this device.
    /// If the device is added, create a Gameobject with the attributes and the onClick event to connect
    /// </summary>
    /// <param name="current">The current state of the list for scanned or paired devices</param>
    /// <param name="received">Can be a List with 1 or more BluetoothDevice</param>
    private void DoLogicWithList(List<BluetoothDevice> current, BluetoothDevice[] received)
    {
        //Debug.Log("DoLogicWithList");
        if (received == null)
        {
            Debug.LogError("Received array is null.");
            return;
        }
        // Process the devices as needed
        foreach (var device in received)
        {
            if (!current.Contains(device) && device != null)
            {
                //Debug.Log("DoLogicWithList: Device not in list" + device);
                current.Add(device);
                if (parentprefab != null && buttonPrefab != null)
                {
                    //Debug.Log("DoLogicWithList: Device Button created");
                    // Instanciate the DeviceButton
                    CreateDeviceButton(device);
                }
                else
                {
                    Debug.LogError("parent prefab is null and / or buttonPrefab is null");
                }
            }
        }
    }
    
    /// <summary>
    /// Create a GameObject from the prefab and instanciate the attributes from the device passed.
    /// This will help for the UI / visualisation of the state
    /// </summary>
    /// <param name="device">BluetoothDevice scanned or already paired</param>
    private void CreateDeviceButton(BluetoothDevice device)
    {
        GameObject newButton = Instantiate(buttonPrefab, parentprefab.transform);
        newButton.GetComponent<DeviceButton>().deviceName.text = device.name;
        newButton.GetComponent<DeviceButton>().deviceAddress.text = device.address;
        newButton.GetComponent<DeviceButton>().deviceType = device.deviceType;
        newButton.GetComponent<DeviceButton>().deviceIcon.sprite =
            _deviceIcons.ContainsKey(device.deviceType.ToString()) 
                ? _deviceIcons[device.deviceType.ToString()] 
                : _deviceIcons["UNKNOWN"];
        // Which one ?
        newButton.GetComponent<Button>().onClick.AddListener(delegate {
                switch (dropDown)
                {
                    case  DeviceReceiver.List_Paired_Devices:
                        TestPlugin.ConnectToDevice(device);
                        break;
                    case  DeviceReceiver.List_Scanned_Devices:
                        TestPlugin.PairToDevice(device);
                        break;
                }
            });
    }
}

// Helper class to wrap the array for deserialization
[System.Serializable]
public class BluetoothDeviceArray
{
    public BluetoothDevice[] devices;
}