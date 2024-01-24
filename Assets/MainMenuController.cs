using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.ImageConversionModule;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] protected Color List1;
    [SerializeField] protected Color List2;
    public Image LevelImage;
    public int SelectedFileId = 0;
    private FileType file_Type = FileType.Local;
    [SerializeField] protected GameObject PrefabMapButton;
    [SerializeField] protected GameObject FileListContainer;
    [SerializeField] Text MapDateText;
    [SerializeField] TMP_Text MapNameText;
    [SerializeField] Text CreatorText;
    [SerializeField]SaveDataScriptableObject SaveData;
    [SerializeField] Button[] NightButtons;
    [SerializeField] TMP_Text nightSelectText;

    [SerializeField] NightSelectBar selectBar;
    private int selectedNight = 0;


    void Start()
    {
        PopulateFileList(0);
        if(LoadingScreen.LoadScreen.gameObject.GetComponent<Canvas>().isActiveAndEnabled == true)
        {
          LoadingScreen.LoadScreen.LoadScreenToggle(false);
        }
    }

    public void UpdateLevelPreivew(GameObject SelectedLevel)
    {
        SelectedFileId = int.Parse(SelectedLevel.name);
        int id = SelectedLevel.gameObject.transform.GetSiblingIndex();
        print(id);


        FilePreview info = null; 

        switch (file_Type)
        {
            case FileType.Local:
                LevelImage.sprite = ImageConversion.StringToSprite(SaveData.LocalMaps[id].PreviewImage);
                info = SaveData.LocalMaps[id];
                break;

            case FileType.Community:
                LevelImage.sprite = ImageConversion.StringToSprite(SaveData.CommunityMaps[id].PreviewImage);
                 info = SaveData.CommunityMaps[id];
                break;

        }


        MapDateText.text = "Date Created: " + info.DateCreated + "    Last Modified: " + info.LastModified;
        MapNameText.text = info.LevelName;
        CreatorText.text = "Creator: " + info.CreatorName;
    }




    /// <summary>
    /// Populates list objects using the given filetype.
    /// </summary>
    /// <param name="Filetype">0 = local maps, 1 = community maps</param>
    public virtual void PopulateFileList(int Filetype)
    {
        foreach (Transform child in FileListContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        var FileList = SaveData.CommunityMaps;
        file_Type = (FileType)Filetype;
        switch (Filetype)
        {         
            case 0:
                FileList = SaveData.LocalMaps;
                break;
        }



        
        for (int i = 0; i < FileList.Count; i++)
        {
            GameObject NewBut = Instantiate(PrefabMapButton, FileListContainer.transform);
            NewBut.name = "" + i;
            ((Text)NewBut.GetComponent<Button>().targetGraphic).text = FileList[i].LevelName;


            if(i % 2 == 0)
            {
                NewBut.GetComponent<Image>().color = List1;
            }

            else
            {
                NewBut.GetComponent<Image>().color = List2;
            }

            NewBut.SetActive(true);

        }


       

       
       
      

      
    }


    public void PopulateLocalFiles()
    {
        foreach (Transform child in FileListContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < SaveData.LocalMaps.Count; i++)
        {
            GameObject NewBut = Instantiate(PrefabMapButton, FileListContainer.transform);
            NewBut.name = "" + i;
            ((Text)NewBut.GetComponent<Button>().targetGraphic).text = SaveData.LocalMaps[i].LevelName;


            if (i % 2 == 0)
            {
                NewBut.GetComponent<Image>().color = List1;
            }

            else
            {
                NewBut.GetComponent<Image>().color = List2;
            }

            NewBut.SetActive(true);

        }

    }


    public void LoadEditMode()
    {
        print("edit mode start called");
        StartCoroutine("LoadEditModeMap");
    }


    public IEnumerator LoadEditModeMap()
    {
        SaveData.LoadedFile = SaveData.RetriveFullMapFile(SaveData.LocalMaps[SelectedFileId].FilePath);
        yield return new WaitUntil(() => SaveData.LoadedFile != null);
        LoadingScreen.LoadScreen.UpdateLoadTitle(SaveData.LoadedFile.MapName);
        LoadingScreen.LoadScreen.LoadScreenToggle(true);
        SceneManager.LoadSceneAsync("WallSystems", LoadSceneMode.Single);

        yield return null;
    }

    public void SetTargetNight(Transform transform)
    {
        int id = transform.GetSiblingIndex();

        nightSelectText.text = "LOAD NIGHT " + (id + 1);
        selectedNight = id;

    }



    public void LoadNewMapMode()
    {
        StartCoroutine("LoadNewModeMap");
    }


    public IEnumerator LoadNewModeMap()
    {
        SaveData.LoadedFile = null;       
        LoadingScreen.LoadScreen.UpdateLoadTitle("New Map");
        LoadingScreen.LoadScreen.LoadScreenToggle(true);
        SceneManager.LoadSceneAsync("WallSystems", LoadSceneMode.Single);

        yield return null;
    }




    public void LoadPlayMode()
    {

    }


    public static void ExitGame()
    {
        Application.Quit();
    }


    public void OpenMapFolder()
    {
        SaveData.OpenSaveFolder(file_Type);
    }



    /*

    public string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
    public Image MainImage;

    public Camera RenderCamera;
    public Byte[] NewImageRawData;
    public RenderTexture RendTexture;




    public void ConvertCameraToImage()
    {
        Texture2D tex = new Texture2D(128, 128);
        RenderCamera.enabled = true;
        
       // NewImageRawData = RendTexture.

    }








    IEnumerator readImage()
    {
        
        using (WWW www = new WWW(url))
        {
            yield return www;
            Renderer renderer = GetComponent<Renderer>();
            MainImage.sprite = Sprite.Create(www.texture, new Rect(0.0f, 0.0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        {
            yield return www.SendWebRequest();
            Renderer renderer = GetComponent<Renderer>();
            Texture2D NewTexture = ((DownloadHandlerTexture.GetContent(www)));
            MainImage.sprite = Sprite.Create(NewTexture, new Rect(0.0f, 0.0f, NewTexture.width, NewTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        yield return null;
    }

    */
}


[Serializable]
public class FilePreview{
    public string FilePath;
    public string DateCreated;
    public string LastModified;
    public string PreviewImage;
    public string LevelName;
    public string CreatorName;


    public FilePreview(string created, string modified, string previewimage, string levelname, string creatorname, string path)
    {
        this.DateCreated = created;
        this.LastModified = modified;
        this.PreviewImage = previewimage;
        this.LevelName = levelname;
        this.CreatorName = creatorname;
        this.FilePath = path;
    }
    }