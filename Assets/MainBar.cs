using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIAnimation;
public enum MenuTab { none, decor, animatronic, gameplay, night };
public class MainBar : MonoBehaviour
{
    

    [SerializeField]private MenuTab _openMenu = MenuTab.none;

    [SerializeField] TweenMenuAnimatorController _decorationAnimator;
    [SerializeField] TweenMenuAnimatorController _animatronicAnimator;
    [SerializeField] TweenMenuAnimatorController _gameplayAnimator;
    [SerializeField] TweenMenuAnimatorController _nightAnimator;

    [SerializeField] CanvasGroup _decorationGroup;
    [SerializeField] CanvasGroup _animatronicGroup;
    [SerializeField] CanvasGroup _gameplayGroup;
    [SerializeField] CanvasGroup _nightGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenMenu(string tabName)
    {
        MenuTab tab = (MenuTab)System.Enum.Parse(typeof(MenuTab),tabName);
        if(_openMenu == tab )
        {
            CloseMenu(tab);
        }
        else
        {
            CloseMenu(_openMenu);
            switch (tab)
            {
                case MenuTab.night:
                    _nightAnimator.PlayOpenAnimation();
                    _nightGroup.interactable = true;
                    _nightGroup.blocksRaycasts = true;
                    break;
                case MenuTab.gameplay:
                    _gameplayAnimator.PlayOpenAnimation();
                    _gameplayGroup.interactable = true;
                    _gameplayGroup.blocksRaycasts = true;
                    break;
                case MenuTab.decor:
                    _decorationAnimator.PlayOpenAnimation();
                    _decorationGroup.interactable = true;
                    _decorationGroup.blocksRaycasts = true;
                    break;
                case MenuTab.animatronic:
                    _animatronicAnimator.PlayOpenAnimation();
                    _animatronicGroup.interactable = true;
                    _animatronicGroup.blocksRaycasts = true;
                    break;
                   
            }
            _openMenu = tab;




        }





    }

    public void CloseMenu(MenuTab tab)
    {

        switch (tab)
        {
            case MenuTab.night:
                _nightAnimator.PlayCloseAnimation();
                _nightGroup.interactable = false;
                _nightGroup.blocksRaycasts = false;
                break;
            case MenuTab.gameplay:
                _gameplayAnimator.PlayCloseAnimation();
                _gameplayGroup.interactable = false;
                _gameplayGroup.blocksRaycasts = false;
                break;
            case MenuTab.decor:
                _decorationAnimator.PlayCloseAnimation();
                _decorationGroup.interactable = false;
                _decorationGroup.blocksRaycasts = false;
                break;
            case MenuTab.animatronic:
                _animatronicAnimator.PlayCloseAnimation();
                _animatronicGroup.interactable = false;
                _animatronicGroup.blocksRaycasts = false;
                break;

        }
        _openMenu = MenuTab.none;
    }




    public void CloseMenu(string tabName)
    {
        MenuTab tab = (MenuTab)System.Enum.Parse(typeof(MenuTab), tabName);
        switch (tab)
        {
            case MenuTab.night:
                _nightAnimator.PlayCloseAnimation();
                _nightGroup.interactable = false;
                _nightGroup.blocksRaycasts = false;
                break;
            case MenuTab.gameplay:
                _gameplayAnimator.PlayCloseAnimation();
                _gameplayGroup.interactable = false;
                _gameplayGroup.blocksRaycasts = false;
                break;
            case MenuTab.decor:
                _decorationAnimator.PlayCloseAnimation();
                _decorationGroup.interactable = false;
                _decorationGroup.blocksRaycasts = false;
                break;
            case MenuTab.animatronic:
                _animatronicAnimator.PlayCloseAnimation();
                _animatronicGroup.interactable = false;
                _animatronicGroup.blocksRaycasts = false;
                break;

        }
        _openMenu = MenuTab.none;
    }

    /*
    public void CloseOpenMenu()
    {
        MenuTab tab = (MenuTab)System.Enum.Parse(typeof(MenuTab), tabName);
        switch (opent)
        {
            case MenuTab.night:
                _nightAnimator.PlayCloseAnimation();
                break;
            case MenuTab.gameplay:
                _gameplayAnimator.PlayCloseAnimation();
                break;
            case MenuTab.decor:
                _decorationAnimator.PlayCloseAnimation();
                break;
            case MenuTab.animatronic:
                _animatronicAnimator.PlayCloseAnimation();
                break;

        }
        _openMenu = MenuTab.none;
    }
    */
}
