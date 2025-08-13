using TMPro;
using UnityEngine;
using UnityEngine.UI;

// File attached in the prefab, reference to each part that will be set when the prefab is instantiated
public class DeviceButton : MonoBehaviour
{
    public Image deviceIcon;
    public TMP_Text deviceName;
    public TMP_Text deviceAddress;
    public DeviceType deviceType;
}
