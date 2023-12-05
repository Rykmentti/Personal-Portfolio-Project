using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenSceneAudioManager : MonoBehaviour
{
    public static TitleScreenSceneAudioManager titleScreenSceneAudioManager; // Only in one scene, might as well be static.
    AudioSource audioSource; // Assign AudioClip in editor.

    // Start is called before the first frame update
    void Start()
    {
        titleScreenSceneAudioManager = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
        StartCoroutine(StartBGM());
    }

    IEnumerator StartBGM()
    {
        //yield return new WaitForSeconds(0.1f); //There will be a weird half second at the start, where sound will play at 0.01 volume and gradual volume increase does not occur... Adding a delay at the start, will fix it.
        // ALWAYS PRELOAD AUDIO MODERATE SIZE BGM AUDIOS!!! Otherwise they will create a small lag when they are first played!
        audioSource.Play();

        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / 2;
            yield return null;
        }
    }

    public IEnumerator GraduallyDecreaseVolume()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / 2;
            yield return null;
        }
    }
}
