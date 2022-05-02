using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Audio_M;
    public AudioSource BGM_Player;
    public AudioSource BGS_Player;
    public AudioSource UI_Player;
    [SerializeField] AudioClip[] Buildsounds;
    [SerializeField] AudioClip _unbuildableSound;
    [SerializeField] AudioClip _hoverSound;
    [SerializeField] AudioClip _uIClick;
    // Start is called before the first frame update

    private void Awake()
    {
        Audio_M = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBuildSound()
    {
        UI_Player.PlayOneShot(Buildsounds[Random.Range(0, Buildsounds.Length)]);
    }

    public void PlayUnbuildableSound()
    {
        UI_Player.PlayOneShot(_unbuildableSound);
    }

    public void PlayHoverSound()
    {
        UI_Player.PlayOneShot(_hoverSound);
    }

    public void PlayUIClick()
    {
        UI_Player.PlayOneShot(_uIClick);
    }
}
