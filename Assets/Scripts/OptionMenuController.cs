using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    public List<Button> buttons = new();
    public List<GameObject> panels = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (buttons.Count != 0)
        {
            foreach(Button btn in buttons)
            {
                btn.onClick.AddListener(delegate{ToggleMenu(btn);});
            }
        }

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
