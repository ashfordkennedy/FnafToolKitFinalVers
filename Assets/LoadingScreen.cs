using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen LoadScreen;
    public Text LoadingTitle;
    public string[] LoadingText1;
    public string[] LoadingText2;
    public Text LoadText1;
    public Text LoadText2;
    
    private void Awake()
    {
        if(LoadingScreen.LoadScreen != null)
        {
            Destroy(this.gameObject);
        }

        else
        {
            LoadingScreen.LoadScreen = this;

        }
    }

    public void UpdateLoadTitle(string newtitle)
    {
        LoadingTitle.text = newtitle;

    }

    public void LoadScreenToggle(bool Active)
    {
        var canvas = this.GetComponent<Canvas>();
        var animator = this.GetComponent<Animator>();


        animator.enabled = Active;
        canvas.enabled = Active;
    

    }


    public void UpdateLoadingText1()
    {
        LoadText1.text = LoadingText1[Random.Range(0, LoadingText1.Length)];
    }

    public void UpdateLoadingText2()
    {
        LoadText2.text = LoadingText2[Random.Range(0, LoadingText2.Length)];
    }
}
