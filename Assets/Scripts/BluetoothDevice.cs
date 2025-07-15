

// Mirror of the CustomBluetoothDevice from plugin
[System.Serializable]
public class BluetoothDevice
{
    public string name;
    public string address;
    public DeviceType deviceType = DeviceType.UNKNOWN; // You can use an enum if you prefer
    public int? rssi; // Nullable int
}

[System.Serializable]
public enum DeviceType {
    AUDIO, COMPUTER, PHONE, UNKNOWN
}

/*

[
  {
    "name": "Galaxy Tab A9",
    "address": "84:22:89:18:E2:17",
    "deviceType": "PHONE"
  },
  {
    "name": "FeatherBlue",
    "address": "C4:CA:B0:E2:2F:F5"
  }
]

*/