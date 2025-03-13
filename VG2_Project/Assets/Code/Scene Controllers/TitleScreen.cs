using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public static TitleScreen instance;

    // menus
    public GameObject mainMenu;
    public GameObject controlsMenu;

    public static bool ControlPopupActive;

    void Awake()
    {
        instance = this;
        HideControls();
    }

    void SwitchMenu(GameObject someMenu)
    {
        // cleanup
        controlsMenu.SetActive(false);

        //turn on
        someMenu.SetActive(true);
    }

    public void ShowTitle()
    {
        SwitchMenu(mainMenu);
    }

    public void ShowControls()
    {
        SwitchMenu(controlsMenu);
        ControlPopupActive = true;
    }

    // on start hide control pop up
    public void HideControls()
    {
        controlsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("FInal Scene");
            HideControls();
        }

        if (ControlPopupActive == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideControls();
                SwitchMenu(mainMenu);
            }
        }
    }
}
