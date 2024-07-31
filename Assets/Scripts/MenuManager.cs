using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioSource musicSource;
    private float _soundVolume;
    private float _musicVolume;

    void Start()
    {
        _soundVolume = PlayerPrefs.GetFloat("soundVolume", 1f);  
        _musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        Debug.Log($"music : {_musicVolume} | sound : {_soundVolume}");
        soundSlider.value = _soundVolume;
        musicSlider.value = _musicVolume;
    }
    public void play()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void exit()
    {
        Application.Quit();
    }
    public void changeSoundVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
        soundSource.volume = soundSlider.value;
    }
    public void changeMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        musicSource.volume = musicSlider.value;
    }
}
