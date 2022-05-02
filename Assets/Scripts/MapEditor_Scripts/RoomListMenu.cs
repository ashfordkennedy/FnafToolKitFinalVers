using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomListMenu : EditorMenuAbstract
{
    public static RoomListMenu instance = null;
    [SerializeField] GameObject ListObjPrefab;
    [SerializeField] Transform ContentPanel;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateListContents()
    {
        WipeList();

        if (EditorController.Instance.Rooms.Count > 0)
        {

            for (int i = 0; i < EditorController.Instance.Rooms.Count; i++)
            {
                GameObject NewBut = Instantiate(ListObjPrefab, ContentPanel);



                InputField IField = NewBut.GetComponentInChildren<InputField>();
                ((Text)IField.placeholder).text = "" + EditorController.Instance.Rooms[i].gameObject.name;
                NewBut.SetActive(true);
            }
        }
    }


    public override void OpenMenu()
    {
        print("open menu called");
        
        UpdateListContents();
        base.OpenMenu();
        
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        WipeList();
    }


    public override void ToggleMenu()
    {
        switch (UICanvas.activeInHierarchy == true)
        {
            case true:
                CloseMenu();
                break;

            case false:
                OpenMenu();
                break;

        }

    }


    /*
      public void OpenMenu()
      {
          if (this.gameObject.activeInHierarchy == false)
          {
              this.gameObject.SetActive(true);
              UpdateListContents();

          }

          else
          {

              CloseMenu();
          }


      }
      */

    public void ConfirmText(GameObject T_Button)
    {
        int id = T_Button.transform.GetSiblingIndex();
        InputField IField = ContentPanel.GetChild(id).GetComponentInChildren<InputField>();
         EditorController.Instance.Rooms[id].gameObject.name = IField.text;
        EditorController.Instance.Rooms[id].Name = IField.text;
    }


    public void CenterRoom(GameObject T_Button)
    {
        int id = T_Button.transform.GetSiblingIndex();
        EditorController.Instance.Rooms[id].EditorSelect();
        CloseMenu();
    }


    public void DeleteRoom(GameObject T_Button)
    {
        int id = T_Button.transform.GetSiblingIndex();
        EditorController.Instance.Rooms[id].DestroyRoom();
        RoomSettingsUI.Instance.CloseMenu();
        UpdateListContents();

    }

   /*
   public void CloseMenu()
    {
        WipeList();
        this.gameObject.SetActive(false);
    }
    */

    void WipeList()
    {
        foreach (Transform child in ContentPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }
}
