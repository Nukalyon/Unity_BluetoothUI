using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  This script is used to create a hierarchy to display a BluetoothDevice like this : 
 *  Button (interactable)
 *  |   Logo device.deviceType
 *  |   Text device.name
 *  |   Text device.address
 */


public class DeviceDisplay : MonoBehaviour
{
    private BluetoothDevice _device;
    private Button _button;
    private Image _deviceLogo;
    private int _logoSizeAudiUnknown = 90;
    private int _logoSizePhoneComputer = 100;
    private Text _deviceName;
    private Text _deviceAddress;

    // Device type to icon mapping (you'll need to replace with actual sprites)
    private static Dictionary<string, Sprite> _deviceIcons = new Dictionary<string, Sprite>()
    {
        { "PHONE",    Resources.Load<Sprite>($"Images/Phone_Icon") },
        { "COMPUTER", Resources.Load<Sprite>($"Images/Computer_Icon") },
        { "AUDIO",    Resources.Load<Sprite>($"Images/Casque_Icon") },
        { "UNKNOWN",  Resources.Load<Sprite>($"Images/Unkown_Icon") }
    };

    private void CreateDevice()
    {
        if (_device != null)
        {
            Debug.Log($"Device Name: {_device.name}, Address: {_device.address}, Type: {_device.deviceType}, RSSI: {_device.rssi}");
            
            GameObject container = new GameObject("DeviceContainer");
            container.transform.SetParent(transform);
            
            // Add Horizontal Layout Group for automatic alignment
            var layout = container.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 20f;
            layout.padding = new RectOffset(10, 10, 5, 5);
            
            // Create device logo/image on the left
            GameObject logoObj = new GameObject("DeviceLogo");
            logoObj.transform.SetParent(container.transform);
            _deviceLogo = logoObj.AddComponent<Image>();
            
            // Set appropriate sprite based on device type
            if (_deviceIcons.ContainsKey(_device.deviceType.ToString()))
            {
                _deviceLogo.sprite = _deviceIcons[_device.deviceType.ToString()];
            }
            else
            {
                _deviceLogo.sprite = _deviceIcons["OTHER"];
            }
            
            var logoRect = _deviceLogo.GetComponent<RectTransform>();
            var size = _device.deviceType is DeviceType.AUDIO or DeviceType.UNKNOWN ? _logoSizeAudiUnknown : _logoSizePhoneComputer;
            logoRect.sizeDelta = new Vector2(size, size);
            
            // Create text container on the right
            GameObject textContainer = new GameObject("DeviceText");
            textContainer.transform.SetParent(container.transform);
            
            // Add vertical layout for name and address
            var textLayout = textContainer.AddComponent<VerticalLayoutGroup>();
            textLayout.childControlHeight = false;
            textLayout.childForceExpandHeight = false;
            textLayout.spacing = 5f;
            
            // Create device name text
            SetupTextUI(textContainer.transform, "DeviceName", _device.name, Color.black,16, FontStyle.Bold);
            
            // Create device address text
            SetupTextUI(textContainer.transform, "DeviceAddress", _device.address, Color.gray,12);
            
            
            // Make the entire container clickable
            _button = container.AddComponent<Button>();
            _button.onClick.AddListener(ConnectToDevice);
            
            // Make the container fill the available space
            var layoutElement = container.AddComponent<LayoutElement>();
            layoutElement.minHeight = 60;
            layoutElement.flexibleWidth = 1;
        }
    }

    private void SetupTextUI(Transform parent, string goName, string text, Color color, int fontSize, FontStyle fontStyle = default)
    {
        GameObject obj = new GameObject(goName);
        obj.transform.SetParent(parent);
        _deviceAddress = obj.AddComponent<Text>();
        _deviceAddress.text = text;
        _deviceAddress.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        _deviceAddress.color = color;
        _deviceAddress.fontSize = fontSize;
        _deviceAddress.alignment = TextAnchor.MiddleLeft;
    }

    private void ConnectToDevice()
    {
        TestPlugin.ConnectToDevice(_device);
    }

    public void SetDevice(BluetoothDevice device)
    {
        _device = device;
        CreateDevice();
    }
}
