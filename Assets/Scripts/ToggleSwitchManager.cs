
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitchGroupManager : MonoBehaviour
{
    [Header("Start Value")]
    [SerializeField] private ToggleSlider initialToggleSwitch;

    [Header("Toggle Options")]
    [SerializeField] private bool allCanBeToggledOff;
    
    private List<ToggleSlider> _toggleSwitches = new List<ToggleSlider>();

    private void Awake()
    {
        Debug.Log("ToggleSwitchGroupManager -> Awake");
        ToggleSlider[] toggleSwitches = GetComponentsInChildren<ToggleSlider>();
        foreach (var toggleSwitch in toggleSwitches)
        {
            RegisterToggleButtonToGroup(toggleSwitch);
        }
    }

    private void RegisterToggleButtonToGroup(ToggleSlider toggleSwitch)
    {
        Debug.Log("ToggleSwitchGroupManager -> RegisterToggleButtonToGroup");
        if (_toggleSwitches.Contains(toggleSwitch))
            return;
        
        _toggleSwitches.Add(toggleSwitch);
        
        toggleSwitch.SetupManager(this);
    }

    private void Start()
    {
        Debug.Log("ToggleSwitchGroupManager -> Start");
        bool areAllToggledOff = true;
        foreach (var button in _toggleSwitches)
        {
            if (!button.CurrentValue) 
                continue;
            
            areAllToggledOff = false;
            break;
        }

        if (!areAllToggledOff || allCanBeToggledOff) 
            return;

        if (initialToggleSwitch != null)
        {
            //Call plugin to get bluetoothState
            AndroidJavaClass plugin = new AndroidJavaClass("com.example.plugin.MyUnityPlayer");
            bool isBluetoothEnabled = plugin.CallStatic<bool>("getBluetoothStatus");
            Debug.Log("Bluetooth enabled = " + isBluetoothEnabled);
            initialToggleSwitch.ToggleByGroupManager(isBluetoothEnabled);
            
            // Debug.Log("ToggleSwitchGroupManager -> initialToggleSwitch != null");
            // initialToggleSwitch.ToggleByGroupManager(false);
        }
    }

    public void ToggleGroup(ToggleSlider toggleSwitch)
    {
        Debug.Log("ToggleSwitchGroupManager -> ToggleGroup");
        if (_toggleSwitches.Count <= 1)
            return;

        if (allCanBeToggledOff && toggleSwitch.CurrentValue)
        {
            foreach (var button in _toggleSwitches)
            {
                if (button == null)
                    continue;

                button.ToggleByGroupManager(false);
            }
        }
        else
        {
            foreach (var button in _toggleSwitches)
            {
                if (button == null)
                    continue;

                if (button == toggleSwitch)
                    button.ToggleByGroupManager(true);
                else
                    button.ToggleByGroupManager(false);
            }
        }
    }
}










