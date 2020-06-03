using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource Audio;

    public void Awake()
    {
        Instance = this;
    }

    public void PlaySfx(string sfxSoundName)
    {
        var audio = Resources.Load<AudioClip>("Sfx/" + sfxSoundName);
        if (audio == null)
            return;
        AudioSource.PlayClipAtPoint(audio, Camera.main.transform.position);
    }


    public void PlayMusic(string musicName)
    {
        var audio = Resources.Load<AudioClip>("Music/" + musicName);
        if (audio == null)
            return;
        if (Audio.clip != audio)
        {
            Audio.clip = audio;
            Audio.Play();
        }
    }
}