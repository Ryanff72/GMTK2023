using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxAudioSrc;
    public AudioSource musicAudioSrc;
    public List<AudioClip> SFX = new List<AudioClip>();
    public List<AudioClip> Music = new List<AudioClip>();
    private AudioClip chosenSFX;
    private AudioClip chosenMusic;
    public float musicTransitionTime;
    private bool changingSongs;


    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        sfxAudioSrc = transform.GetChild(0).GetComponent<AudioSource>();
        musicAudioSrc = transform.GetChild(1).GetComponent<AudioSource>();
        musicAudioSrc.volume = 0f;
    }

    public void PlaySoundEffect(string clipName, float volume)
    {
        
        for (int i = 0; i < SFX.Count; i++)
        {
            if (SFX[i].name == clipName)
            {
                chosenSFX = SFX[i];
            }
        }
        sfxAudioSrc.PlayOneShot(chosenSFX, volume);
    }

    private void Update()
    {
        if(changingSongs)
        {
            musicAudioSrc.volume = Mathf.Lerp(musicAudioSrc.volume,0,Time.deltaTime*4);
        }
        else
        {
            musicAudioSrc.volume = Mathf.Lerp(musicAudioSrc.volume, 0.3f, Time.deltaTime*0.2f);
        }
    }

    public void PlayMusic(string songName)
    {
        StartCoroutine("NewMusic");
        for (int i = 0; i < Music.Count; i++)
        {
            if (Music[i].name == songName)
            {
                chosenMusic = Music[i];
            }
        }
    }

    IEnumerator NewMusic()
    {
        changingSongs = true;
        yield return new WaitForSeconds(musicTransitionTime);
        musicAudioSrc.clip = chosenMusic;
        changingSongs= false;
        musicAudioSrc.Play();
    }
}
