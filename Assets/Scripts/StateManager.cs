using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameDisplay;
    [SerializeField] private GameObject _idleDisplay;
    [SerializeField] private GameObject _scanningDisplay;
    [SerializeField] private GameObject _connectingDisplay;
    [SerializeField] private GameObject _connectedDisplay;
    [SerializeField] private GameObject _disconnectingDisplay;
    [SerializeField] private GameObject _disconnectedDisplay;
    private bool _isMenuOpen;
    private List<GameObject> _panels = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.name = "StateManager";
        DontDestroyOnLoad(this);
        _isMenuOpen = false;
        // check if a gameobject is null
        if(_gameDisplay != null) _panels.Add(_gameDisplay);
        if(_idleDisplay != null) _panels.Add(_idleDisplay);
        if(_scanningDisplay != null) _panels.Add(_scanningDisplay);
        if(_connectingDisplay != null) _panels.Add(_connectingDisplay);
        if(_connectedDisplay != null) _panels.Add(_connectedDisplay);
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
                    ToggleOther(_connectedDisplay);
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
}
