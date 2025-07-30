using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    [Header("Initialize simple UI")]
    [SerializeField] public TMP_InputField inputField;
    [SerializeField] public Button btnVisible;
    [SerializeField] public Image imgVisible;
    [SerializeField] public Button btnReset;


    void Start()
    {

        // Initialize the regex display
        if (inputField != null)
        {
            inputField.SetTextWithoutNotify(TestPlugin.GetRegex() == null ? ".*" : TestPlugin.GetRegex());
            // Add the listener to end of change of regex
            inputField.onEndEdit.AddListener(delegate { TestPlugin.SetRegex(inputField.text); });
        }
        
        //Initialize the button for the visibility of the device
        if (btnVisible != null && imgVisible != null)
        {
            btnVisible.onClick.AddListener(delegate { TestPlugin.SetVisibility(imgVisible); });
            TestPlugin.SetVisibility(imgVisible);
        }

        if (btnReset != null)
        {
            btnReset.onClick.AddListener(delegate { TestPlugin.ResetInput(inputField);});
        }
    }
}
