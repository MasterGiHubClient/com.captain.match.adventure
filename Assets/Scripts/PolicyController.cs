using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PolicyController : MonoBehaviour
{
    public GameObject policyTextObject;
    public GameObject acceptPopupObject;
    [SerializeField]
    private AudioSource musicSource;  
    [SerializeField]
    private AudioSource soundSource;
    // Start is called before the first frame update
    private void Start()
    {
        soundSource.volume = PlayerPrefs.GetFloat("soundVolume", 1f);
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 1f);

    }
    public void AcceptPolicy()
    {
        PlayerPrefs.SetInt("policyAccepted", 1);
        SceneManager.LoadScene("MenuScene");
        soundSource.Play();
    }

}
