using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public List<AudioClip> sfxList;
    private List<AudioSource> activeSources;
    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
        activeSources = new List<AudioSource>();
    }

    private void Start()
    {
        bgmSource.volume = 1.0f;
        bgmSlider.value = 1.0f;
        sfxSource.volume = 1.0f;
        sfxSlider.value = 1.0f;
    }

    public void SetMusicVolume()
    {
        bgmSource.volume = bgmSlider.value;
    }

    public void SetSfxVolume()
    {
        sfxSource.volume = sfxSlider.value;
    }

    public void PlaySound(string type)
    {
        int index = 0;
        switch (type)
        {
            case "gun": index = 0; break;
            case "AKFire": index = 1; break;
            case "shotgun2": index = 2; break;
            case "Turret": index= 3; break;
            case "Jump": index = 4; break;
            case "hitPlayer": index = 5; break;
            case "blade": index = 6; break;
            case "howl": index = 7; break;
            case "bat": index = 8; break;
            case "hitMonster": index = 9; break;
            case "Roar": index = 10; break;
            case "GolemPunch": index = 11; break;

            default: break;

        }
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = sfxList[index];
        newSource.volume = sfxSource.volume;
        newSource.Play();

        StartCoroutine(RemoveAfterPlaying(newSource));
        activeSources.Add(newSource);
    }

    private IEnumerator RemoveAfterPlaying(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);

        activeSources.Remove(source);

        Destroy(source);
    }
}
