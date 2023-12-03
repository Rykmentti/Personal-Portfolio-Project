using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AudioPlayer : MonoBehaviour
{
    [SerializeField] NPC_AudioClipSet npcAudioSetSO; // Assign in Editor

    [SerializeField] AudioType audioType;
    AudioSource npcAudioSource;

    public enum AudioType
    {
        // Do-nothing case.
        Nothing,

        // Single Audio Playback.
        OnDeath,
        OnBeingHit,

        // Looping Audio Playback.
        Footsteps,
    }
    // Start is called before the first frame update
    void Start()
    {
        npcAudioSource = GetComponent<AudioSource>();
    }
    public void NPC_PlayFirstSingleAudioClipFromArray(AudioType audio)
    {
        switch (audio)
        {
            case AudioType.OnDeath: npcAudioSource.PlayOneShot(npcAudioSetSO.onDeath[0]); break;
            case AudioType.OnBeingHit: npcAudioSource.PlayOneShot(npcAudioSetSO.OnBeingHit[0]); break;

            default:
                Debug.Log("This case '" + audio + "' for switch case does not recognize the case enum, doesn't exist or there is a typo in name of the case");
                break;
        }
    }
    public void NPC_PlaySingleRandomAudioClipFromArray(AudioType audio)
    {
        switch (audio)
        {
            case AudioType.OnDeath: npcAudioSource.PlayOneShot(npcAudioSetSO.onDeath[Random.Range(0, npcAudioSetSO.onDeath.Length)]); break;
            case AudioType.OnBeingHit: npcAudioSource.PlayOneShot(npcAudioSetSO.OnBeingHit[Random.Range(0, npcAudioSetSO.OnBeingHit.Length)]); break;

            default:
                Debug.Log("This case '" + audio + "' for switch case does not recognize the case enum, doesn't exist or there is a typo in name of the case");
                break;
        }
    }
    public void NPC_PlayLoopingAudioClip(AudioType audio)
    {
        if (audioType == audio) return;

        else if (audioType != audio) audioType = audio;
        if (npcAudioSource.loop == false) npcAudioSource.loop = true;

        switch (audio)
        {
            case AudioType.Footsteps: npcAudioSource.clip = npcAudioSetSO.footsteps[0]; npcAudioSource.Play(); break;

            default:
                Debug.Log("This case '" + audio + "' for switch case does not recognize the case enum, doesn't exist or there is a typo in name of the case");
                break;
        }
    }
    public void NPC_StopLoopingAudioPlayback()
    {
        //npcAudioSource.Stop();
        audioType = AudioType.Nothing;
        npcAudioSource.loop = false;
    }
    public AudioSource AccessAudioSource()
    {
        return npcAudioSource;
    }
}
