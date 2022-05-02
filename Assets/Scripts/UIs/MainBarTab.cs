using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIAnimation;
public abstract class MainBarTab : MonoBehaviour
{

    internal bool _isOpen = false;
    [SerializeField] internal CanvasGroup canvasGroup;
    [SerializeField]internal TweenMenuAnimatorController _animator;




    public void ToggleMenu(bool open)
    {
        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;
        switch (open)
        {
            case true:
                _isOpen = true;
                _animator.PlayOpenAnimation();
                break;

            case false:
                _animator.openAnimator.SetToStart();
                _isOpen = false;
                _animator.PlayCloseAnimation();
                break;
        }
    }

    public void ToggleMenu()
    {
       // bool open = !_isOpen;
        _isOpen = !_isOpen;
        canvasGroup.interactable = !_isOpen;
        canvasGroup.blocksRaycasts = !_isOpen;
        switch (_isOpen)
        {
            case false:
                _animator.PlayOpenAnimation();
                break;

            case true:
                _animator.PlayCloseAnimation();
                break;
        }
    }







    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
