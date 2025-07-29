using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    [Header("Buttons Switch Panels")]
    public List<Button> buttons = new();
    [Header("List of UI Panels")]
    public List<GameObject> panels = new();
    
    [Header("Initialize simple UI")]
    [SerializeField] public TMP_InputField inputField;
    [SerializeField] public Button btnVisible;
    [SerializeField] public Image imgVisible;
    [SerializeField] public Button btnReset;


    void Start()
    {
        // Initialize the buttons Listener
        if (buttons.Count != 0)
        {
            foreach (Button btn in buttons)
            {
                btn.onClick.AddListener(delegate { ToggleMenu(btn); });
            }
        }

        // Initialize the visibility of the panels
        if (panels.Count != 0)
        {
            foreach (GameObject go in panels)
            {
                if (go.activeInHierarchy)
                {
                    go.SetActive(false);
                }
            }
        }

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

    private void ToggleMenu(Button btn)
    {
        if (buttons.Contains(btn) && buttons.Count != 0)
        {
            if (btn == buttons[0])
            {
                panels[0].SetActive(true);
                panels[1].SetActive(false);
                return;
            }

            if (btn == buttons[1])
            {
                panels[0].SetActive(false);
                panels[1].SetActive(false);
                return;
            }

            if (btn == buttons[2])
            {
                panels[1].SetActive(true);
                return;
            }

            if (btn == buttons[3])
            {
                panels[1].SetActive(false);
            }
        }
    }
}
