using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    [Header("Initialize simple UI")]
    [SerializeField] public TMP_InputField inputField; // Input for the Regex
    [SerializeField] public Button btnVisible; // Button for device visibility
    [SerializeField] public Image imgVisible; // UI representing the state, green = visible, red = not visible
    [SerializeField] public Button btnReset; // Button to reset Advanced Setting (regex mostly)

    
    
    void Start()
    {

        // Initialize the regex display + the listener
        if (inputField != null)
        {
            inputField.SetTextWithoutNotify(TestPlugin.GetRegex() == null ? ".*" : TestPlugin.GetRegex());
            // Add the listener to end of change of regex
            inputField.onEndEdit.AddListener(delegate { TestPlugin.SetRegex(inputField.text); });
        }
        
        // Initialize the button for the visibility of the device
        // This one needs a rework (split in 2 functions ?)
        if (btnVisible != null && imgVisible != null)
        {
            btnVisible.onClick.AddListener(delegate { TestPlugin.SetVisibility(imgVisible); });
            // basic call to init on startup
            TestPlugin.SetVisibility(imgVisible);
        }
        
        // Initialize the Reset Button Action
        if (btnReset != null)
        {
            btnReset.onClick.AddListener(delegate { TestPlugin.ResetInput(inputField);});
        }
    }
}
