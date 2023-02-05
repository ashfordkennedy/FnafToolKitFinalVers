using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuideDatabase", menuName = "HelpSystem/GuideDatabase", order = 1)]
public class GuideDatabase : ScriptableObject
{
    public List<GuidePage> pages;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class GuidePage
{
    public string title = "";
    [TextArea] public string mainText = "";
    public Sprite image = null;

}
