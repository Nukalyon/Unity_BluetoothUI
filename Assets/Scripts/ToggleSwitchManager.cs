
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitchGroupManager : MonoBehaviour
{
    /*
     * Crédit à la chaîne : Christina Creates Games (https://www.youtube.com/@ChristinaCreatesGames)
     * ref: https://www.youtube.com/watch?v=E9AWlbPGi_4
     */
    
    [Header("Start Value")]
    [SerializeField] public ToggleSlider initialToggleSwitch;

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
            Debug.Log("ToggleSwitchGroupManager -> toggle initial");
            bool isBluetoothEnabled = TestPlugin.GetBluetoothStatus();
            Debug.Log("Bluetooth enabled = " + isBluetoothEnabled);
            initialToggleSwitch.ToggleByGroupManager(isBluetoothEnabled);
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

                button.ToggleByGroupManager(button == toggleSwitch);
            }
        }
    }
}










