using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController instance;
    public bool menuOpen = false;
    [SerializeField] GameObject MenuObject;

    private void Awake()
    {
        instance = this;
    }


    public void OpenCloseMenu(bool Opening)
    {
        
                MenuObject.SetActive(Opening);
                menuOpen = Opening;         

    }


    public void ReturnToMainMenu()
    {
        StartCoroutine("LoadMainMenu");

    }

    private IEnumerator LoadMainMenu()
    {
        if (LoadingScreen.LoadScreen != null)
        {
            LoadingScreen.LoadScreen.UpdateLoadTitle("Returning To Menu");
            LoadingScreen.LoadScreen.LoadScreenToggle(true);
        }

        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);

        yield return null;
    }
   
}
