using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingManager : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(load());
    }
    IEnumerator load()
    {
        Debug.Log("Started!");
        var privacyAgreed = PlayerPrefs.GetInt("policyAccepted", 0);
        yield return new WaitForSeconds(0.2f);
        slider.value = 0.5f;
        Debug.Log("Aded!");
        yield return new WaitForSeconds(0.15f);
        slider.value = 0.7f;
        Debug.Log("Aded!");
        yield return new WaitForSeconds(0.10f);
        slider.value = 0.9f;
        Debug.Log("Aded!");
        if (privacyAgreed == 1)
        {
            slider.value = 1;
            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            SceneManager.LoadScene("PolicyScene");
            slider.value = 1;
        }
    }
}
