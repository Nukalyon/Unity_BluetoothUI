using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    [Header("Buttons Switch Panels")]
    public List<Button> buttons = new();
    [Header("List of UI Panels")]
    public List<GameObject> panels = new();
    
    [Header("Initialize simple UI")]
    [SerializeField] public GameObject inputField;
    
    
    void Start()
    {
        // Initialize the buttons Listener
        if (buttons.Count != 0)
        {
            foreach(Button btn in buttons)
            {
                btn.onClick.AddListener(delegate{ToggleMenu(btn);});
            }
        }
        // Initialize the visibility of the panels
        if (panels.Count != 0)
        {
            foreach(GameObject go in panels)
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
            InputField fieldRegex = inputField.GetComponent<InputField>();
            fieldRegex.text = TestPlugin.GetRegex() == null ? ".*" : TestPlugin.GetRegex();
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
