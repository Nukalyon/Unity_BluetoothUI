// Mirror of the CustomBluetoothDevice from plugin
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class BluetoothDevice
{
    public string name;
    public string address;
    [JsonConverter(typeof(StringEnumConverter))]
    public DeviceType deviceType = DeviceType.UNKNOWN;
    public int? rssi; // Nullable int
}

[System.Serializable]
public enum DeviceType {
    AUDIO, 
    COMPUTER, 
    PHONE, 
    UNKNOWN
}