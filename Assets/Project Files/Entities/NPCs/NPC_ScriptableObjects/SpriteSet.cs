using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sprite Set", menuName = "ScritableObjects/Sprite Set")]
public class SpriteSet : ScriptableObject
{
    //Sprite sets themselves.
    [SerializeField] internal Sprite[] idleAnimationSpritesNorth = new Sprite[0];
    [SerializeField] internal Sprite[] idleAnimationSpritesEast = new Sprite[0];
    [SerializeField] internal Sprite[] idleAnimationSpritesSouth = new Sprite[0];
    [SerializeField] internal Sprite[] idleAnimationSpritesWest = new Sprite[0];
    [SerializeField] internal float idleAnimationInterval; // Set all animation interval values in editor for now.

    [SerializeField] internal Sprite[] walkAnimationSpritesNorth = new Sprite[0];
    [SerializeField] internal Sprite[] walkAnimationSpritesEast = new Sprite[0];
    [SerializeField] internal Sprite[] walkAnimationSpritesSouth = new Sprite[0];
    [SerializeField] internal Sprite[] walkAnimationSpritesWest = new Sprite[0];
    [SerializeField] internal float walkAnimationInterval; // 12 Frames per second animation = (60 : 12) : 60 = 0.83333...f

    [SerializeField] internal Sprite[] attackIdleAnimationSpritesNorth = new Sprite[0];
    [SerializeField] internal Sprite[] attackIdleAnimationSpritesEast = new Sprite[0];
    [SerializeField] internal Sprite[] attackIdleAnimationSpritesSouth = new Sprite[0];
    [SerializeField] internal Sprite[] attackIdleAnimationSpritesWest = new Sprite[0];
    [SerializeField] internal float attackIdleAnimationInterval; // Set all animation interval values in editor for now.

    [SerializeField] internal Sprite[] attackAnimationSpritesNorth = new Sprite[0];
    [SerializeField] internal Sprite[] attackAnimationSpritesEast = new Sprite[0];
    [SerializeField] internal Sprite[] attackAnimationSpritesSouth = new Sprite[0];
    [SerializeField] internal Sprite[] attackAnimationSpritesWest = new Sprite[0];
    [SerializeField] internal float attackAnimationInterval; // 8 Frames per second animation = (60 : 8) : 60 = 0.125f

    [SerializeField] internal Sprite[] castIdleAnimationSpritesNorth = new Sprite[0];
    [SerializeField] internal Sprite[] castIdleAnimationSpritesEast = new Sprite[0];
    [SerializeField] internal Sprite[] castIdleAnimationSpritesSouth = new Sprite[0];
    [SerializeField] internal Sprite[] castIdleAnimationSpritesWest = new Sprite[0];
    [SerializeField] internal float castIdleAnimationInterval;

    [SerializeField] internal Sprite[] castAnimationSpritesNorth = new Sprite[0];
    [SerializeField] internal Sprite[] castAnimationSpritesEast = new Sprite[0];
    [SerializeField] internal Sprite[] castAnimationSpritesSouth = new Sprite[0];
    [SerializeField] internal Sprite[] castAnimationSpritesWest = new Sprite[0];
    [SerializeField] internal float castAnimationInterval;

    [SerializeField] internal Sprite[] deathAnimationSpritesSouth = new Sprite[0];
    [SerializeField] internal float deathAnimationInterval;
}
