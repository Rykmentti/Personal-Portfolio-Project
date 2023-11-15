using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpriteAnimationControllerSO : MonoBehaviour
{
    [SerializeField] SpriteSet spriteSetSO; // ScriptableObject, assigned in Editor
    [SerializeField] SpriteRenderer spriteRenderer; // Assigned in Editor
    [SerializeField] CurrentState currentState;
    Coroutine currentAnimationCoroutine; // Using this to make sure only one animation coroutine is active at any point in time.

    // 2D Animation sprites for 4 directions. Implemented with state machine.
    // Decide how many sprites there in the animation, add sprites themselves into the arrays and in correct order and the script should do the rest by itself.
    // Just make sure to access this methods within this script, hopefully with a state machine, to make sure the animation is played properly.

    [SerializeField] bool isAnimating;

    public enum CurrentState
    {
        // Top-Down 2D animation requires 4 directions per animation set. North, East, South and West.

        // Hold state, to make sure we don't play any animation, when we don't want to.
        Hold, 

        // 4-Directional Animation States.
        IdleAnimationNorth, IdleAnimationEast, IdleAnimationSouth, IdleAnimationWest,
        WalkingAnimationNorth, WalkingAnimationEast, WalkingAnimationSouth, WalkingAnimationWest,
        AttackIdleAnimationNorth, AttackIdleAnimationEast, AttackIdleAnimationSouth, AttackIdleAnimationWest,
        AttackingAnimationNorth, AttackingAnimationEast, AttackingAnimationSouth, AttackingAnimationWest,
        CastIdleAnimationNorth, CastIdleAnimationEast, CastIdleAnimationSouth, CastIdleAnimationWest,
        CastingAnimationNorth, CastingAnimationEast, CastingAnimationSouth, CastingAnimationWest,

        // 1-Directional Animation States.
        DeathAnimation,
    }
    public void SetState(CurrentState state)
    {
        if (state == currentState) return; // To make sure we are not playing same animation on top of one another. I.e. We only play new animation, to replace the previous one.
        isAnimating = false;
        currentState = state;
    }
    public bool IsAnimating()
    {
        return isAnimating;
    }

    public void KillCurrentAnimationCoroutine() 
    {
        StopCoroutine(currentAnimationCoroutine); // Need to kill current Coroutine, if we want to change animation mid-animation, will Crash Unity Editor otherwise, because we are trying to create/replace new Coroutine, while the old one is still running.
        currentState = CurrentState.Hold;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(CurrentState.IdleAnimationSouth);
    }

    // Update is called once per frame
    void Update() // Animation State Machine body.
    {
        switch (currentState)
        {
            // Hold state for the animation controller, to make sure we don't play any animation, when we don't want to.
            case CurrentState.Hold: break;

            // False = looping animation. True = play only once animation.
            case CurrentState.IdleAnimationNorth: PlayAnimation(spriteSetSO.idleAnimationSpritesNorth, 0, false); break;
            case CurrentState.IdleAnimationEast:  PlayAnimation(spriteSetSO.idleAnimationSpritesEast,  0, false); break;
            case CurrentState.IdleAnimationSouth: PlayAnimation(spriteSetSO.idleAnimationSpritesSouth, 0, false); break;
            case CurrentState.IdleAnimationWest:  PlayAnimation(spriteSetSO.idleAnimationSpritesWest,  0, false); break;

            case CurrentState.WalkingAnimationNorth: PlayAnimation(spriteSetSO.walkAnimationSpritesNorth, spriteSetSO.walkAnimationInterval, false); break;
            case CurrentState.WalkingAnimationEast:  PlayAnimation(spriteSetSO.walkAnimationSpritesEast,  spriteSetSO.walkAnimationInterval,  false); break;
            case CurrentState.WalkingAnimationSouth: PlayAnimation(spriteSetSO.walkAnimationSpritesSouth, spriteSetSO.walkAnimationInterval, false); break;
            case CurrentState.WalkingAnimationWest:  PlayAnimation(spriteSetSO.walkAnimationSpritesWest,  spriteSetSO.walkAnimationInterval, false); break;

            case CurrentState.AttackIdleAnimationNorth: PlayAnimation(spriteSetSO.attackIdleAnimationSpritesNorth, 0, false); break;
            case CurrentState.AttackIdleAnimationEast:  PlayAnimation(spriteSetSO.attackIdleAnimationSpritesEast,  0,  false); break;
            case CurrentState.AttackIdleAnimationSouth: PlayAnimation(spriteSetSO.attackIdleAnimationSpritesSouth, 0, false); break;
            case CurrentState.AttackIdleAnimationWest:  PlayAnimation(spriteSetSO.attackIdleAnimationSpritesWest,  0, false); break;

            case CurrentState.AttackingAnimationNorth:  PlayAnimation(spriteSetSO.attackAnimationSpritesNorth, spriteSetSO.attackAnimationInterval, true); break;
            case CurrentState.AttackingAnimationEast:   PlayAnimation(spriteSetSO.attackAnimationSpritesEast,  spriteSetSO.attackAnimationInterval,  true); break;
            case CurrentState.AttackingAnimationSouth:  PlayAnimation(spriteSetSO.attackAnimationSpritesSouth, spriteSetSO.attackAnimationInterval, true); break;
            case CurrentState.AttackingAnimationWest:   PlayAnimation(spriteSetSO.attackAnimationSpritesWest,  spriteSetSO.attackAnimationInterval, true); break;

            case CurrentState.CastIdleAnimationNorth: PlayAnimation(spriteSetSO.castIdleAnimationSpritesNorth, 0, false); break;
            case CurrentState.CastIdleAnimationEast:  PlayAnimation(spriteSetSO.castIdleAnimationSpritesEast,  0, false); break;
            case CurrentState.CastIdleAnimationSouth: PlayAnimation(spriteSetSO.castIdleAnimationSpritesSouth, 0, false); break;
            case CurrentState.CastIdleAnimationWest:  PlayAnimation(spriteSetSO.castIdleAnimationSpritesWest,  0, false); break;

            case CurrentState.CastingAnimationNorth: PlayAnimation(spriteSetSO.castAnimationSpritesNorth, spriteSetSO.castAnimationInterval, true); break;
            case CurrentState.CastingAnimationEast:  PlayAnimation(spriteSetSO.castAnimationSpritesEast,  spriteSetSO.castAnimationInterval, true); break;
            case CurrentState.CastingAnimationSouth: PlayAnimation(spriteSetSO.castAnimationSpritesSouth, spriteSetSO.castAnimationInterval, true); break;
            case CurrentState.CastingAnimationWest:  PlayAnimation(spriteSetSO.castAnimationSpritesWest,  spriteSetSO.castAnimationInterval, true); break;

            case CurrentState.DeathAnimation: PlayAnimation(spriteSetSO.deathAnimationSpritesSouth, spriteSetSO.deathAnimationInterval, true); break;

            default: // Default Behaviour. I.e. If statemachine does not recognize the state, it will default to this state that something is trying to put into it. (It's not in enum CurrentState?)
                Debug.Log("This state '" + currentState + "' for state machine doesn't exist, or there is a typo in name(string) of the state, defaulting to Idle State");
                SetState(CurrentState.IdleAnimationSouth);
                break;
        }

        AnimationTestingWithKeys(); // Testing
    }
    void PlayAnimation(Sprite[] animationSet, float animationInterval, bool playOnce)
    {
        if (isAnimating) return;

        if (currentAnimationCoroutine != null) StopCoroutine(currentAnimationCoroutine);
        currentAnimationCoroutine = StartCoroutine(AnimationRuntime(animationSet, animationInterval, playOnce));
        isAnimating = true;
    }
    IEnumerator AnimationRuntime(Sprite[] animationSet, float time, bool playOnce)
    {
        do
        {
            for (int i = 0; i < animationSet.Length; i++)
            {
                if (i > animationSet.Length) i = 0;
                spriteRenderer.sprite = animationSet[i];
                yield return new WaitForSeconds(time);
            }
        } while (!playOnce); // We will use StopCoroutine method or playOnce bool, to break the do-while loop.
        isAnimating = false;
        SetState(CurrentState.Hold);
    }

    void AnimationTestingWithKeys() // Testing.
    {
        if (Input.GetKey(KeyCode.W)) SetState(CurrentState.WalkingAnimationNorth);
        if (Input.GetKey(KeyCode.D)) SetState(CurrentState.WalkingAnimationEast);
        if (Input.GetKey(KeyCode.S)) SetState(CurrentState.WalkingAnimationSouth);
        if (Input.GetKey(KeyCode.A)) SetState(CurrentState.WalkingAnimationWest);
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftControl)) SetState(CurrentState.AttackingAnimationNorth);
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftControl)) SetState(CurrentState.AttackingAnimationEast);
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftControl)) SetState(CurrentState.AttackingAnimationSouth);
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftControl)) SetState(CurrentState.AttackingAnimationWest);
    }
}
