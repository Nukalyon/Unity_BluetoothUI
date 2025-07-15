using Unity.VisualScripting;
using UnityEngine;

public class BluetoothDeviceReceiver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.name = "List_Paired_Devices";
        DontDestroyOnLoad(gameObject);
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
            //Debug.Log($"Device Name: {device.name}, Address: {device.address}, Type: {device.deviceType}, RSSI: {device.rssi}");
            DeviceDisplay obj = this.AddComponent<DeviceDisplay>();
            obj.SetDevice(device);
        }
    }
}

// Helper class to wrap the array for deserialization
[System.Serializable]
public class BluetoothDeviceArray
{
    public BluetoothDevice[] devices;
}