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
    private BluetoothDevice _device;
    private Button _button;
    private Image _background;
    private Image _deviceLogo;
    private Text _deviceText;

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
            
            //Top of the hierarchy
            GameObject container = new GameObject("DeviceContainer");
            _button = container.AddComponent<Button>();
            _button.image.pixelsPerUnitMultiplier = 1.5f;
            _button.onClick.AddListener(ConnectToDevice);
            _background = container.AddComponent<Image>();
            _background.color = new Color(155, 82, 168, 128);
            container.transform.SetParent(transform);
            
            // Add Horizontal Layout Group for automatic alignment
            var horizontalLayout = container.AddComponent<HorizontalLayoutGroup>();
            horizontalLayout.childAlignment = TextAnchor.MiddleCenter;
            horizontalLayout.childControlWidth = false;
            horizontalLayout.childControlHeight = false;
            horizontalLayout.childScaleWidth = false;
            horizontalLayout.childScaleHeight = false;
            horizontalLayout.childForceExpandWidth = true;
            horizontalLayout.childForceExpandHeight = true;
            
            // Create device logo/image on the left
            GameObject logoObj = new GameObject("DeviceLogo");
            logoObj.transform.SetParent(container.transform);
            _deviceLogo = logoObj.AddComponent<Image>();
            
            // Set appropriate sprite based on device type
            _deviceLogo.sprite = _deviceIcons.ContainsKey(_device.deviceType.ToString()) 
                    ? _deviceIcons[_device.deviceType.ToString()] 
                    : _deviceIcons["OTHER"];
            
            var logoRect = _deviceLogo.GetComponent<RectTransform>();
            logoRect.sizeDelta = new Vector2(10, 20);
            
            GameObject containerText = new GameObject("DeviceTexts");
            containerText.transform.SetParent(transform);
            
            var verticalLayout = containerText.AddComponent<VerticalLayoutGroup>();
            verticalLayout.childAlignment = TextAnchor.MiddleCenter;
            verticalLayout.childControlWidth = true;
            verticalLayout.childControlHeight = true;
            verticalLayout.childScaleWidth = false;
            verticalLayout.childScaleHeight = false;
            verticalLayout.childForceExpandWidth = true;
            verticalLayout.childForceExpandHeight = true;
            
            // Create device name text
            SetupTextUI(containerText.transform, "DeviceName", _device.name, Color.black,16, FontStyle.Bold);
            
            // Create device address text
            SetupTextUI(containerText.transform, "DeviceAddress", _device.address, Color.gray,12);
        }
    }

    private void SetupTextUI(Transform parent, string goName, string text, Color color, int fontSize, FontStyle fontStyle = default)
    {
        Debug.Log("Setup Text UI");
        Debug.Log("Setup Text UI -> name : " + goName);
        Debug.Log("Setup Text UI -> value : " + text);
        GameObject obj = new GameObject(goName);
        _deviceText = obj.AddComponent<Text>();
        _deviceText.text = text;
        _deviceText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        _deviceText.color = color;
        _deviceText.fontSize = fontSize;
        _deviceText.fontStyle = fontStyle;
        _deviceText.alignment = TextAnchor.MiddleLeft;
        obj.transform.SetParent(parent);
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
