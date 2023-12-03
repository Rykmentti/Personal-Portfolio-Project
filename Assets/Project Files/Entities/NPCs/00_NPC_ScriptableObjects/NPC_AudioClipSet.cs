using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC AudioClip Set", menuName = "ScriptableObjects/NPC AudioClip Set")]
public class NPC_AudioClipSet : ScriptableObject
{
    public AudioClip[] footsteps;

    public AudioClip[] onDeath;

    public AudioClip[] OnBeingHit;

    // Anything that I'm forgetting? Hopefully not.
}
