using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeChildrenOutScript : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip sceneaudio;
    private bool ChildrenHaveFadedOut = false;
    public void FadeChildrenOut()
    {
        if (!ChildrenHaveFadedOut)
        {
            ChildrenHaveFadedOut = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<FadeOutScript>().StartFading();
            }
            new WaitForSecondsRealtime(0.5f);
            FadeGooseIn();
        }
    }

    private void Start()
    {
        audio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    private void FadeGooseIn()
    {
        GameObject.Find("goose").GetComponent<FadeInScript>().StartFading();
        audio.clip = sceneaudio;
        audio.Play();
        StartCoroutine(GotoScene2());
    }

    IEnumerator GotoScene2()
    {
        yield return new WaitForSeconds(audio.clip.length);
        SceneManager.LoadScene("Scene2");
    }


}
