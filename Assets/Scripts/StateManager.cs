using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameDisplay;
    [SerializeField] private GameObject _idleDisplay;
    [SerializeField] private GameObject _scanningDisplay;
    [SerializeField] private GameObject _connectingDisplay;
    [SerializeField] private GameObject _connectedClientDisplay;
    [SerializeField] private GameObject _connectedServerDisplay;
    [SerializeField] private GameObject _disconnectingDisplay;
    [SerializeField] private GameObject _disconnectedDisplay;
    private bool _isMenuOpen;
    private bool _isServer = false;
    private List<GameObject> _panels = new();
    
    
    void Start()
    {
        gameObject.name = "StateManager";
        DontDestroyOnLoad(this);
        SetMenuOpen(false);
        // check if a gameobject is null
        if(_gameDisplay != null) _panels.Add(_gameDisplay);
        if(_idleDisplay != null) _panels.Add(_idleDisplay);
        if(_scanningDisplay != null) _panels.Add(_scanningDisplay);
        if(_connectingDisplay != null) _panels.Add(_connectingDisplay);
        if(_connectedClientDisplay != null) _panels.Add(_connectedClientDisplay);
        if(_connectedServerDisplay != null) _panels.Add(_connectedServerDisplay);
        if(_disconnectingDisplay != null) _panels.Add(_disconnectingDisplay);
        if(_disconnectedDisplay != null) _panels.Add(_disconnectedDisplay);
    }

    private void OnAppStateChange(string state)
    {
        Debug.Log("panels count : " + _panels.Count);
        if (_isMenuOpen)
        {
            // AppState : com.example.plugin.model.AppState$Scanning@866848f
            string currentState = state.PartAfter('$').PartBefore('@');
            Debug.LogWarning("AppState : " + currentState);
            switch (currentState)
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
                    if(_isServer) ToggleOther(_connectedServerDisplay);
                    else ToggleOther(_connectedClientDisplay);
                    break;
                case "Disconnecting":
                    ToggleOther(_disconnectingDisplay);
                    break;
                case "Disconnected":
                    ToggleOther(_disconnectedDisplay);
                    break;
                default:
                    Debug.Log("State not recognized");
                    break;
            }
        }
        else
        {
            ToggleOther(_gameDisplay);
        }
    }

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
