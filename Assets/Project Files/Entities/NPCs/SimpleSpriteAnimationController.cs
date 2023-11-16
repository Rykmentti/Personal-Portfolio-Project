using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpriteAnimationController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] SpriteSet spriteSetSO; // ScriptableObject, which contains all the sprites for the NPC. Assigned in Editor
    [SerializeField] CurrentState currentState;
    Coroutine currentAnimationCoroutine; // Using this to make sure only one animation coroutine is active at any point in time.

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
        if (state == currentState) return; // To make sure we are not playing same animation on top of one another. I.e. We only play new animation and replace the previous one.
        isAnimating = false;
        currentState = state;
    }
    public bool IsAnimating()
    {
        return isAnimating;
    }

    public void KillCurrentAnimationCoroutine() 
    {
        StopCoroutine(currentAnimationCoroutine); // Need to kill current Coroutine, if we want to change animation mid-animation. It WILL Crash Unity Editor otherwise, because we are trying to create/replace new Coroutine, while the old one is still running.
        currentState = CurrentState.Hold;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            case CurrentState.IdleAnimationNorth: SetSprite(spriteSetSO.idleAnimationSpritesNorth); break;
            case CurrentState.IdleAnimationEast:  SetSprite(spriteSetSO.idleAnimationSpritesEast ); break;
            case CurrentState.IdleAnimationSouth: SetSprite(spriteSetSO.idleAnimationSpritesSouth); break;
            case CurrentState.IdleAnimationWest:  SetSprite(spriteSetSO.idleAnimationSpritesWest ); break;

            case CurrentState.WalkingAnimationNorth: PlayLoopingAnimation(spriteSetSO.walkAnimationSpritesNorth, spriteSetSO.walkAnimationInterval); break;
            case CurrentState.WalkingAnimationEast:  PlayLoopingAnimation(spriteSetSO.walkAnimationSpritesEast,  spriteSetSO.walkAnimationInterval); break;
            case CurrentState.WalkingAnimationSouth: PlayLoopingAnimation(spriteSetSO.walkAnimationSpritesSouth, spriteSetSO.walkAnimationInterval); break;
            case CurrentState.WalkingAnimationWest:  PlayLoopingAnimation(spriteSetSO.walkAnimationSpritesWest,  spriteSetSO.walkAnimationInterval); break;

            case CurrentState.AttackIdleAnimationNorth: SetSprite(spriteSetSO.attackIdleAnimationSpritesNorth); break;
            case CurrentState.AttackIdleAnimationEast:  SetSprite(spriteSetSO.attackIdleAnimationSpritesEast);  break;
            case CurrentState.AttackIdleAnimationSouth: SetSprite(spriteSetSO.attackIdleAnimationSpritesSouth); break;
            case CurrentState.AttackIdleAnimationWest:  SetSprite(spriteSetSO.attackIdleAnimationSpritesWest);  break;

            case CurrentState.AttackingAnimationNorth:  PlaySingleAnimation(spriteSetSO.attackAnimationSpritesNorth, spriteSetSO.attackAnimationInterval); break;
            case CurrentState.AttackingAnimationEast:   PlaySingleAnimation(spriteSetSO.attackAnimationSpritesEast,  spriteSetSO.attackAnimationInterval); break;
            case CurrentState.AttackingAnimationSouth:  PlaySingleAnimation(spriteSetSO.attackAnimationSpritesSouth, spriteSetSO.attackAnimationInterval); break;
            case CurrentState.AttackingAnimationWest:   PlaySingleAnimation(spriteSetSO.attackAnimationSpritesWest,  spriteSetSO.attackAnimationInterval); break;

            case CurrentState.CastIdleAnimationNorth: SetSprite(spriteSetSO.castIdleAnimationSpritesNorth); break;
            case CurrentState.CastIdleAnimationEast:  SetSprite(spriteSetSO.castIdleAnimationSpritesEast);  break;
            case CurrentState.CastIdleAnimationSouth: SetSprite(spriteSetSO.castIdleAnimationSpritesSouth); break;
            case CurrentState.CastIdleAnimationWest:  SetSprite(spriteSetSO.castIdleAnimationSpritesWest);  break;

            case CurrentState.CastingAnimationNorth: PlaySingleAnimation(spriteSetSO.castAnimationSpritesNorth, spriteSetSO.castAnimationInterval); break;
            case CurrentState.CastingAnimationEast:  PlaySingleAnimation(spriteSetSO.castAnimationSpritesEast,  spriteSetSO.castAnimationInterval); break;
            case CurrentState.CastingAnimationSouth: PlaySingleAnimation(spriteSetSO.castAnimationSpritesSouth, spriteSetSO.castAnimationInterval); break;
            case CurrentState.CastingAnimationWest:  PlaySingleAnimation(spriteSetSO.castAnimationSpritesWest,  spriteSetSO.castAnimationInterval); break;

            case CurrentState.DeathAnimation: PlaySingleAnimation(spriteSetSO.deathAnimationSpritesSouth, spriteSetSO.deathAnimationInterval); break;

            default: // Default Behaviour. I.e. If statemachine does not recognize the state, it will default to this state that something is trying to put into it. (It's not in enum CurrentState?)
                Debug.Log("This state '" + currentState + "' for state machine doesn't exist, or there is a typo in name(string) of the state, defaulting to Idle State");
                SetState(CurrentState.IdleAnimationSouth);
                break;
        }

        AnimationTestingWithKeys(); // Testing
    }
    void PlaySingleAnimation(Sprite[] animationSet, float animationInterval) // Play single animation.
    {
        if (isAnimating) return;

        if (currentAnimationCoroutine != null) StopCoroutine(currentAnimationCoroutine);
        currentAnimationCoroutine = StartCoroutine(SingleAnimationRuntime(animationSet, animationInterval));
        isAnimating = true;

        IEnumerator SingleAnimationRuntime(Sprite[] animationSet, float time) // Run single animation set.
        {
            for (int i = 0; i < animationSet.Length; i++)
            {
                if (i > animationSet.Length) i = 0;
                spriteRenderer.sprite = animationSet[i];
                yield return new WaitForSeconds(time);
            }
            isAnimating = false;
            SetState(CurrentState.Hold);
        }
    }
    void PlayLoopingAnimation(Sprite[] animationSet, float animationInterval) // Loop single animation.
    {
        if (isAnimating) return;

        if (currentAnimationCoroutine != null) StopCoroutine(currentAnimationCoroutine);
        currentAnimationCoroutine = StartCoroutine(LoopAnimationRuntime(animationSet, animationInterval));
        isAnimating = true;

        IEnumerator LoopAnimationRuntime(Sprite[] animationSet, float time) // Loop single animation set forever.
        {
            do
            {
                for (int i = 0; i < animationSet.Length; i++)
                {
                    if (i > animationSet.Length) i = 0;
                    spriteRenderer.sprite = animationSet[i];
                    yield return new WaitForSeconds(time);
                }
            } while (true); // We will use StopCoroutine method to kill the coroutine running the animation and break the do-while loop.
        }
    }
    void SetSprite(Sprite[] sprites) // Setting single sprite in the renderer, no need to animate anything. In the future, even idle animations will be animated, so this is should temporary.
    {
        isAnimating = true;
        spriteRenderer.sprite = sprites[0];
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
