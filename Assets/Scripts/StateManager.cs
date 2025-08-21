using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    [Header("In case of Bluetooth disconnecting")]
    [SerializeField] private GameObject _bluetoothDisplay;
    
    [Header("LifeCycle of UI")]
    [SerializeField] private GameObject _gameDisplay;
    [SerializeField] private GameObject _idleDisplay;
    [SerializeField] private GameObject _scanningDisplay;
    [SerializeField] private GameObject _connectingDisplay;
    [SerializeField] private GameObject _pairingDisplay;
    [SerializeField] private GameObject _connectedClientDisplay;
    [SerializeField] private GameObject _connectedServerDisplay;
    [SerializeField] private GameObject _disconnectingDisplay;
    [SerializeField] private GameObject _disconnectedDisplay;
    
    [Header("Visibility of the device")]
    [SerializeField] public Image imgVisible; // UI representing the state, green = visible, red = not visible
    
    private bool _isBluetoothEnabled = false;
    private bool _isMenuOpen = false;
    private bool _isServer = false;
    private List<GameObject> _panels = new();
    // var to keep track on which panel we were if something happen (bluetooth turned off, ...)
    private int CurrentPanel { get; set; }
    // var to save the state of the app sent by the plugin
    // Used to switch the display of UI
    internal static string CurrentState { get; set; }
    
    
    void Start()
    {
        // Mandatory for correspondence between plugin and the gameobject receiver
        gameObject.name = "StateManager";
        DontDestroyOnLoad(this);
        // If the menu pause was open
        SetMenuOpen(false);
        // Panel 1 is "in the game"
        CurrentPanel = 1;
        
        // Need to keep the bluetoothDisplay in first position
        if(_bluetoothDisplay != null) _panels.Add(_bluetoothDisplay);
        // Check if Null
        if(_gameDisplay != null) _panels.Add(_gameDisplay);
        if(_idleDisplay != null) _panels.Add(_idleDisplay);
        if(_scanningDisplay != null) _panels.Add(_scanningDisplay);
        if(_connectingDisplay != null) _panels.Add(_connectingDisplay);
        if(_pairingDisplay != null) _panels.Add(_pairingDisplay);
        if(_connectedClientDisplay != null) _panels.Add(_connectedClientDisplay);
        if(_connectedServerDisplay != null) _panels.Add(_connectedServerDisplay);
        if(_disconnectingDisplay != null) _panels.Add(_disconnectingDisplay);
        if(_disconnectedDisplay != null) _panels.Add(_disconnectedDisplay);
        _isBluetoothEnabled = TestPlugin.GetBluetoothStatus();
        
        Debug.LogWarning("Panels Count = " + _panels.Count);
        Debug.LogWarning("isBluetooth enabled = " + _isBluetoothEnabled);
        // Check if the bluetooth is enabled and set the display
        ToggleOther(_isBluetoothEnabled ? _panels[CurrentPanel] : _bluetoothDisplay);
    }
    
    /// <summary>
    /// This method is called from the plugin when the app changes state.
    /// We use this state to react on it like setting the right UI and maybe methods.
    /// If at any moment something happen, this will switch the display accordingly
    /// </summary>
    /// <param name="state"></param>
    private void OnAppStateChange(string state)
    {
        if(!_isBluetoothEnabled)
        {
            ToggleOther(_bluetoothDisplay);
        }
        else if (_isMenuOpen)
        {
            // AppState : com.example.plugin.model.AppState$Scanning@866848f
            CurrentState = state.PartAfter('$').PartBefore('@');
            
            Debug.LogWarning("AppState : " + CurrentState);
            switch (CurrentState)
            {
                case "Idle":
                    ToggleOther(_idleDisplay);
                    break;
                case "Scanning":
                    ToggleOther(_scanningDisplay);
                    break;
                case "Connecting":
                    ToggleOther(_connectingDisplay);
                    break;
                case "Connected":
                    ToggleOther(_isServer ? _connectedServerDisplay : _connectedClientDisplay);
                    break;
                case "Disconnecting":
                    ToggleOther(_disconnectingDisplay);
                    break;
                case "Disconnected":
                    ToggleOther(_disconnectedDisplay);
                    break;
                default:
                    Debug.Log("State not recognized = " + CurrentState);
                    break;
            }
        }
        else
        {
            ToggleOther(_gameDisplay);
        }
    }
    
    /// <summary>
    /// This method is called when the BluetoothStateReceiver in the plugin catch an Event.
    /// The new state is passed as an arg.
    /// When the bluetooth turn OFF, if the device is connected, we must disconnect them
    /// When the bluetooth turn ON, the old state will tell us what to do to not break the cycle
    ///     Ex: if the device was Scanning, we relaunch the scan
    /// </summary>
    /// <param name="state"></param>
    private void OnBluetoothStateChange(string state)
    {
        Debug.Log("bluetooth state turned " + state);
        Debug.Log("CurrentState = " + CurrentState);
        switch (state)
        {
            case "ON":
                switch (CurrentState)
                {
                    // Idle state
                    case "Idle":
                        TestPlugin.UpdateDevicePaired();
                        break;
                    // Scanning state
                    case "Scanning":
                        TestPlugin.StartScan();
                        break;
                    case "Connected":
                        ToggleOther(_disconnectedDisplay);
                        break;
                }
                break;
            case "OFF":
                switch (CurrentState)
                {
                    case "Scanning":
                        TestPlugin.StopScan();
                        break;
                    case "Connected":
                        TestPlugin.DisconnectFromDevice();
                        break;
                }
                break;
        }
        Debug.Log("isBluetoothEnabled = " + _isBluetoothEnabled);
        // Update of the var and switch of the display
        _isBluetoothEnabled = TestPlugin.GetBluetoothStatus();
        ToggleOther(_isBluetoothEnabled ? _panels[CurrentPanel] : _bluetoothDisplay);
    }
    
    /// <summary>
    /// When the visibility changes in the app, auto-update
    /// Can be changed with user request on the button
    ///         or
    /// By the software inside the device, sometimes it cancel visibility automatically
    /// </summary>
    /// <param name="state"> The new state of the visibility (true or false) </param>
    private void OnVisibilityStateChange(string state)  // can be 0 or 1
    {
        bool stateChanged = state == "1";
        Debug.Log("visibility state turned " + state);
        imgVisible.color = stateChanged ? Color.green : Color.red;
    }
    
    /// <summary>
    /// Toggle all the others panels in the list to not visible
    /// </summary>
    /// <param name="displayed"> GameObject to set visible </param>
    private void ToggleOther(GameObject displayed)
    {
        foreach (GameObject go in _panels)
        {
            if (go != displayed)
            {
                go.SetActive(false);
            }
        }
        displayed.SetActive(true);
        // Not really sure for the update but it doesn't break
        CurrentPanel = _panels.IndexOf(displayed) == 0 ? CurrentPanel : _panels.IndexOf(displayed);
    }

    public void SetMenuOpen(bool open)
    {
        _isMenuOpen = open;
        ToggleOther(_isMenuOpen ? _idleDisplay : _gameDisplay);
    }

    public void SetIsServer()
    {
        _isServer = true;
    }
}
