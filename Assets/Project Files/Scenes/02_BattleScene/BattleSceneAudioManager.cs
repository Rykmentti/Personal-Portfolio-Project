using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneAudioManager : MonoBehaviour
{
    public static BattleSceneAudioManager battleSceneAudioManager; // Only in one scene, might as well be static.
    AudioSource audioSource;
    [SerializeField] AudioClip[] deploymentBGM = new AudioClip[0]; // Assign in Editor
    [SerializeField] AudioClip[] battleBGM = new AudioClip[0]; // Assign in Editor

    // Start is called before the first frame update
    void Start()
    {
        battleSceneAudioManager = this;
        audioSource = GetComponent<AudioSource>(); // First clip already assigned in editor
        audioSource.volume = 0;
        StartCoroutine(StartBGM());
    }

    private void Update()
    {

    }


    IEnumerator StartBGM()
    {
        //yield return new WaitForSeconds(0.1f); //There will be a weird half second at the start, where sound will play at 0.01 volume and gradual volume increase does not occur... Adding a delay at the start, will fix it.
        // ALWAYS PRELOAD AUDIO FOR MODERATE SIZE BGM AUDIOS!!! Otherwise they will create a small lag when they are first played!
        audioSource.clip = deploymentBGM[0];
        audioSource.Play();
    
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / 2;
            yield return null;
        }
    }
    IEnumerator ChangeBGM(AudioClip newBGM) // We use this to change the BGM, by fading the current clip volume to zero and then inserting new clip and fading it in instead.
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / 2;
            yield return null;
        }
        audioSource.clip = newBGM;
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / 2;
            yield return null;
        }
    }

    public void ChangeToBattleBGM()
    {
        StartCoroutine(ChangeBGM(battleBGM[0]));
    }
}
