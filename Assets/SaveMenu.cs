using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum SaveMode { newSave, Overwrite }
/// <summary>
/// This controls the save menu in the same manner the mainmenucontroller controls the main load menu. enough that its methods are reusable here
/// </summary>
public class SaveMenu : EditorMenuAbstract
{
    public static SaveMenu instance;
    [SerializeField] protected Color List1;
    [SerializeField] protected Color List2;
    public Image LevelImage;
    public int SelectedFileId = 0;
    [SerializeField] protected GameObject PrefabMapButton;
    [SerializeField] protected GameObject FileListContainer;
    [SerializeField] Text MapDateText;
    [SerializeField] Text MapNameText;
    [SerializeField] Text CreatorText;

    [SerializeField] Button OverwriteButton;


    [Header("Save File menu objects")]
    [SerializeField] GameObject SaveWindow;
    [SerializeField] GameObject OverwriteWindow;
    [SerializeField] GameObject NewSaveWindow;
    [SerializeField] Button SaveFileButton;
    [SerializeField] TMP_InputField _mapNameInput;
    [SerializeField] TMP_InputField _creatorNameInput;
    [SerializeField] TMP_InputField _bioInput;
    [SerializeField] Image MapIcon;
    [SerializeField] Toggle editableToggle;
    public SaveMode saveMode = SaveMode.newSave;
    
    [SerializeField] private SaveDataScriptableObject _saveData;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }


    void PopulateFileList()
    {
        ClearList();
        var FileList = _saveData.LocalMaps;

        for (int i = 0; i < FileList.Count; i++)
        {
            GameObject NewBut = Instantiate(PrefabMapButton, FileListContainer.transform);
            NewBut.name = "" + i;
            ((Text)NewBut.GetComponent<Button>().targetGraphic).text = FileList[i].LevelName;


            NewBut.GetComponent<Image>().color = (i % 2 == 0) ?  List1 : List2;          
            NewBut.SetActive(true);

        }



    }

    void ClearList()
    {
        foreach (Transform child in FileListContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    void SelectFile(Transform transform)
    {
        SelectedFileId = transform.GetSiblingIndex();
        OverwriteButton.interactable = true;
    }


    public void UpdateLevelPreivew(GameObject SelectedLevel)
    {
        SelectedFileId = int.Parse(SelectedLevel.name);
        OverwriteButton.interactable = true;
        int id = SelectedLevel.gameObject.transform.GetSiblingIndex();
        print(id);


        var info = _saveData.LocalMaps[id];
        SaveDataHandler.SaveHandler.OverwritePath = info.FilePath;
        LevelImage.color = Color.white;
        LevelImage.sprite = ImageConversion.StringToSprite(_saveData.LocalMaps[id].PreviewImage);


        MapDateText.text = "Date Created: " + info.DateCreated + "    Last Modified: " + info.LastModified;
        MapNameText.text = info.LevelName;
        CreatorText.text = "Creator: " + info.CreatorName;
    }


    public void DeselectLevel()
    {
        OverwriteButton.interactable = false;
        LevelImage.color = Color.clear;
    }

    public override void OpenMenu()
    {
        base.OpenMenu();
        PopulateFileList();
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        DeselectLevel();
        ClearList();
    }


    public void CheckSaveName()
    {

        if(_mapNameInput.text != "" && _creatorNameInput.text != "")
        {
            SaveFileButton.interactable = true;
        }
        else
        {
            SaveFileButton.interactable = false;
        }

    }

    public void CloseSavePanel()
    {
        SaveWindow.SetActive(false);
        SaveFileButton.interactable = false;
        _mapNameInput.text = "";
        _creatorNameInput.text = "";
        _bioInput.text = "";
    }


    /// <summary>
    /// opens select file dialogue
    /// </summary>
    public void SelectMapIcon()
    {
      //  StartCoroutine(SelectIcon());
        OpenFileName file = null;
        ;
        if (FileSelector.SelectFile(out file, "png", "Select Map Icon", "Map Icon"))
        {
            print("Your file is now ready bitch");
            print(file.fileTitle);
            Sprite newIcon = ImageConversion.FIlePathToSprite(file.file);
            LoadMapIcon(newIcon);
            SaveDataHandler.SaveHandler.IconBuffer = ImageConversion.FIlePathToString(file.file);
        }
    }

    public void LoadMapIcon(Sprite sprite)
    {
        MapIcon.sprite = sprite;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string RetrieveSaveIcon()
    {

        return SaveDataHandler.SaveHandler.SaveData.LoadedFile.PreviewImage;
    }

    public void SelectFileFolder()
    {
        //  StartCoroutine(SelectIcon());
        OpenFileName file = null;
        FileSelector.SelectSaveFile(out file, "png", "Select Map Icon", "Map Icon");
        if (file.fileTitle != "")
        {
            print("Your file is now ready bitch");
            Sprite newIcon = ImageConversion.FIlePathToSprite(file.file);
            MapIcon.sprite = newIcon;
            SaveDataHandler.SaveHandler.IconBuffer = ImageConversion.FIlePathToString(file.file);
        }
    }

    public void EnableOverwriteMenu()
    {
        if (SaveDataHandler.SaveHandler.SaveData.LoadedFile != null)
        {
            saveMode = SaveMode.Overwrite;
            var file = SaveDataHandler.SaveHandler.SaveData.LoadedFile;
            _mapNameInput.text = file.MapName;
            _creatorNameInput.text = file.MapCreator;
            _bioInput.text = file.Bio;
        }
        else
        {
            EnableNewSaveMenu();
        }




    }

    public void EnableNewSaveMenu()
    {
        saveMode = SaveMode.newSave;
        _mapNameInput.text = "";
        _creatorNameInput.text = "";
        _bioInput.text = "";
    }


    public void StartOverwriteSave()
    {

    }

    public void StartSave()
    {
        
        SaveDataHandler.SaveHandler.StartCoroutine(SaveDataHandler.SaveHandler.SaveLevelProcess(_mapNameInput.text,_creatorNameInput.text,_bioInput.text,editableToggle.isOn, saveMode));
        CloseSavePanel();
        
    }

}
