using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{


    [SerializeField]
    int levelToGoOnEsc = 0;

    [SerializeField]
    GameObject notWorkingmenu;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (levelToGoOnEsc >= 0)
            {
                GoToScene(levelToGoOnEsc);
            }
            else
            {
                QuitApplication();
            }
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void NotWorkingButton()
    {
        notWorkingmenu.SetActive(true);
    }

    public void GoToScene(int i)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(i);
    }
}
