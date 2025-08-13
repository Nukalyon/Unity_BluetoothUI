// Mirror of the CustomBluetoothDevice from plugin
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class BluetoothDevice
{
    public string name;
    public string address;
    // This line grant the serialization to keep the DeviceType as string and not an Integer
    [JsonConverter(typeof(StringEnumConverter))]
    public DeviceType deviceType = DeviceType.UNKNOWN;
    public int? rssi; // Nullable int
}

// Need to serialize the enum because the class parent is serializable
[System.Serializable]
public enum DeviceType {
    AUDIO, 
    COMPUTER, 
    PHONE, 
    UNKNOWN
}