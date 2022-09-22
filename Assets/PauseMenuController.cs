using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class PauseMenuController : EditorMenuAbstract
{
    public static PauseMenuController instance;
    public MenuTip[] tips;

    [SerializeField] TMP_Text _tipTitle;
    [SerializeField] TMP_Text _tipBody;
    private void Awake()
    {
        instance = this;
    }

    public override void OpenMenu()
    {
        UpdateTip();
        base.OpenMenu();
    }




    private void UpdateTip()
    {
        int id = Random.Range(0, tips.Length);
        MenuTip tip = tips[id];

        _tipTitle.text = tip.title;
        _tipBody.text = tip.tip;

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


[System.Serializable]
public struct MenuTip
{
    public string title;
    [TextArea]public string tip;

}