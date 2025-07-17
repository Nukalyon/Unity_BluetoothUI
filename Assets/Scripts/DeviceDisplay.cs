using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  This script is used to create a hierarchy to display a BluetoothDevice like this : 
 *  Button (interactable)
 *  |   Logo device.deviceType
 *  |   GameObject Devicetexts
*       |   Text device.name
*       |   Text device.address
 */


public class DeviceDisplay : MonoBehaviour
{
    private List<BluetoothDevice> _devices;
    private GameObject _parent;

    // Device type to icon mapping (you'll need to replace with actual sprites)
    private static readonly Dictionary<string, Sprite> DeviceIcons = new()
    {
        { "PHONE",    Resources.Load<Sprite>($"Images/Phone_Icon") },
        { "COMPUTER", Resources.Load<Sprite>($"Images/Computer_Icon") },
        { "AUDIO",    Resources.Load<Sprite>($"Images/Casque_Icon") },
        { "UNKNOWN",  Resources.Load<Sprite>($"Images/Unknown_Icon") }
    };

    private void CreateDevice()
    {
        if (_devices != null && _devices.Count > 0)
        {
            Debug.Log("_devices != null && _devices.Count > 0 = " + (_devices != null && _devices.Count > 0));
            Debug.Log("_parent = " + _parent);
            foreach (var device in _devices)
            {
                Debug.Log("| -> device " + device.name);
                //Top of the hierarchy
                GameObject container = new GameObject("DeviceContainer");
                Debug.Log("(1)");
                Button button = container.AddComponent<Button>();
                Debug.Log("(2)");
                button.image.pixelsPerUnitMultiplier = 1.5f;
                Debug.Log("(2,5)");
                button.image.SetVerticesDirty();
                Debug.Log("(3)");
                button.onClick.AddListener(delegate{ConnectToDevice(device);});
                Debug.Log("(4)");
                Image background = container.AddComponent<Image>();
                Debug.Log("(5)");
                background.color = new Color(155/255f, 82/255f, 168/255f, 128/255f);
                Debug.Log("(6)");
                container.transform.SetParent(_parent.transform);
                Debug.Log("(7)");
                
                Debug.Log("| -> mainContainer " + container);
                // Add Horizontal Layout Group for automatic alignment
                var horizontalLayout = container.AddComponent<HorizontalLayoutGroup>();
                horizontalLayout.childAlignment = TextAnchor.MiddleCenter;
                horizontalLayout.childControlWidth = false;
                horizontalLayout.childControlHeight = false;
                horizontalLayout.childScaleWidth = false;
                horizontalLayout.childScaleHeight = false;
                horizontalLayout.childForceExpandWidth = true;
                horizontalLayout.childForceExpandHeight = true;
                
                Debug.Log("| -> horizontalLayout " + horizontalLayout);
                // Create device logo/image on the left
                GameObject logoObj = new GameObject("DeviceLogo");
                logoObj.transform.SetParent(container.transform);
                Image deviceLogo = logoObj.AddComponent<Image>();
                
                
                // Set appropriate sprite based on device type
                deviceLogo.sprite = DeviceIcons.ContainsKey(device.deviceType.ToString()) 
                        ? DeviceIcons[device.deviceType.ToString()] 
                        : DeviceIcons["UNKNOWN"];
                
                var logoRect = deviceLogo.GetComponent<RectTransform>();
                logoRect.sizeDelta = new Vector2(10, 20);
                
                Debug.Log("| -> deviceLogo " + deviceLogo.sprite);
                GameObject containerText = new GameObject("DeviceTexts");
                containerText.transform.SetParent(container.transform);
                
                Debug.Log("| -> containerText " + containerText);
                var verticalLayout = containerText.AddComponent<VerticalLayoutGroup>();
                verticalLayout.childAlignment = TextAnchor.MiddleCenter;
                verticalLayout.childControlWidth = true;
                verticalLayout.childControlHeight = true;
                verticalLayout.childScaleWidth = false;
                verticalLayout.childScaleHeight = false;
                verticalLayout.childForceExpandWidth = true;
                verticalLayout.childForceExpandHeight = true;
                
                Debug.Log("| -> verticalLayout " + verticalLayout);
                // Create device name text
                SetupTextUI(containerText.transform, "DeviceName", device.name, Color.black,16, FontStyle.Bold);
                
                // Create device address text
                SetupTextUI(containerText.transform, "DeviceAddress", device.address, Color.gray,12); 
                
                Debug.Log("| -> containerText " + containerText);
            }
        }
    }

    private void SetupTextUI(Transform parent, string goName, string text, Color color, int fontSize, FontStyle fontStyle = default)
    {
        Debug.Log("Setup Text UI");
        Debug.Log("Setup Text UI -> name : " + goName);
        Debug.Log("Setup Text UI -> value : " + text);
        GameObject obj = new GameObject(goName);
        Text deviceText = obj.AddComponent<Text>();
        deviceText.text = text;
        deviceText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        deviceText.color = color;
        deviceText.fontSize = fontSize;
        deviceText.fontStyle = fontStyle;
        deviceText.alignment = TextAnchor.MiddleLeft;
        obj.transform.SetParent(parent);
    }

    private void ConnectToDevice(BluetoothDevice device)
    {
        TestPlugin.ConnectToDevice(device);
    }

    public void SetDevices(GameObject parent, List<BluetoothDevice> devices)
    {
        _parent = parent;
        Debug.Log("Set parent : " + _parent);
        _devices = devices;
        // Used for debug and correct init
        string debug = "Set devices with :\n";
        foreach (var device in _devices)
        {
            debug += device.name + "\n";
        }
        Debug.Log(debug);
        CreateDevice();
    }
}
