using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpriteAnimationControllerOld : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer; // Assigned in Editor
    [SerializeField] CurrentState currentState;
    Coroutine currentAnimationCoroutine; // Using this to make sure only one animation is active at any point in time.

    // 2D Animation sprites for 4 directions. Implemented with state machine.
    // Decide how many sprites there in the animation, add sprites themselves into the arrays and in correct order and the script should do the rest by itself.
    // Just make sure to access this methods within this script, hopefully with a state machine, to make sure the animation is played properly.

    // Main Jagged Array, from which all the arrays below are accessed from.

    [SerializeField] Sprite[][][] animationSpritesJaggedArray = new Sprite[7][][];

    // 4-Directional Sprite sets.
    [SerializeField] Sprite[][] idleAnimationSprites = new Sprite[4][];
    [SerializeField] Sprite[][] walkAnimationSprites = new Sprite[4][];
    [SerializeField] Sprite[][] attackIdleAnimationSprites = new Sprite[4][];
    [SerializeField] Sprite[][] attackAnimationSprites = new Sprite[4][];
    [SerializeField] Sprite[][] castIdleAnimationSprites = new Sprite[4][];
    [SerializeField] Sprite[][] castAnimationSprites = new Sprite[4][];

    // 1-Directional Sprite sets.
    [SerializeField] Sprite[][] deathAnimationSprites = new Sprite[1][];

    //Sprite sets themselves.
    [SerializeField] Sprite[] idleAnimationSpritesNorth = new Sprite[0];
    [SerializeField] Sprite[] idleAnimationSpritesEast = new Sprite[0];
    [SerializeField] Sprite[] idleAnimationSpritesSouth = new Sprite[0];
    [SerializeField] Sprite[] idleAnimationSpritesWest = new Sprite[0];
    [SerializeField] float idleAnimationInterval; // Set all animation interval values in editor for now.

    [SerializeField] Sprite[] walkAnimationSpritesNorth = new Sprite[0];
    [SerializeField] Sprite[] walkAnimationSpritesEast = new Sprite[0];
    [SerializeField] Sprite[] walkAnimationSpritesSouth = new Sprite[0];
    [SerializeField] Sprite[] walkAnimationSpritesWest = new Sprite[0];
    [SerializeField] float walkAnimationInterval; // 12 Frames per second animation = (60 : 12) : 60 = 0.83333...f

    [SerializeField] Sprite[] attackIdleAnimationSpritesNorth = new Sprite[0];
    [SerializeField] Sprite[] attackIdleAnimationSpritesEast = new Sprite[0];
    [SerializeField] Sprite[] attackIdleAnimationSpritesSouth = new Sprite[0];
    [SerializeField] Sprite[] attackIdleAnimationSpritesWest = new Sprite[0];
    [SerializeField] float attackIdleAnimationInterval; // Set all animation interval values in editor for now.

    [SerializeField] Sprite[] attackAnimationSpritesNorth = new Sprite[0];
    [SerializeField] Sprite[] attackAnimationSpritesEast = new Sprite[0];
    [SerializeField] Sprite[] attackAnimationSpritesSouth = new Sprite[0];
    [SerializeField] Sprite[] attackAnimationSpritesWest = new Sprite[0];
    [SerializeField] float attackAnimationInterval; // 8 Frames per second animation = (60 : 8) : 60 = 0.125f

    [SerializeField] Sprite[] castIdleAnimationSpritesNorth = new Sprite[0];
    [SerializeField] Sprite[] castIdleAnimationSpritesEast = new Sprite[0];
    [SerializeField] Sprite[] castIdleAnimationSpritesSouth = new Sprite[0];
    [SerializeField] Sprite[] castIdleAnimationSpritesWest = new Sprite[0];
    [SerializeField] float castIdleAnimationInterval;

    [SerializeField] Sprite[] castAnimationSpritesNorth = new Sprite[0];
    [SerializeField] Sprite[] castAnimationSpritesEast = new Sprite[0];
    [SerializeField] Sprite[] castAnimationSpritesSouth = new Sprite[0];
    [SerializeField] Sprite[] castAnimationSpritesWest = new Sprite[0];
    [SerializeField] float castAnimationInterval;

    [SerializeField] Sprite[] deathAnimationSpritesSouth = new Sprite[0];
    [SerializeField] float deathAnimationInterval;
    void InitializeJaggedArray()
    {
        animationSpritesJaggedArray[0] = idleAnimationSprites;
        animationSpritesJaggedArray[1] = walkAnimationSprites;
        animationSpritesJaggedArray[2] = attackIdleAnimationSprites;
        animationSpritesJaggedArray[3] = attackAnimationSprites;
        animationSpritesJaggedArray[4] = castIdleAnimationSprites;
        animationSpritesJaggedArray[5] = castAnimationSprites;
        animationSpritesJaggedArray[6] = deathAnimationSprites;

        idleAnimationSprites[0] = idleAnimationSpritesNorth;
        idleAnimationSprites[1] = idleAnimationSpritesEast;
        idleAnimationSprites[2] = idleAnimationSpritesSouth;
        idleAnimationSprites[3] = idleAnimationSpritesWest;

        walkAnimationSprites[0] = walkAnimationSpritesNorth;
        walkAnimationSprites[1] = walkAnimationSpritesEast;
        walkAnimationSprites[2] = walkAnimationSpritesSouth;
        walkAnimationSprites[3] = walkAnimationSpritesWest;

        attackIdleAnimationSprites[0] = attackIdleAnimationSpritesNorth;
        attackIdleAnimationSprites[1] = attackIdleAnimationSpritesEast;
        attackIdleAnimationSprites[2] = attackIdleAnimationSpritesSouth;
        attackIdleAnimationSprites[3] = attackIdleAnimationSpritesWest;

        attackAnimationSprites[0] = attackAnimationSpritesNorth;
        attackAnimationSprites[1] = attackAnimationSpritesEast;
        attackAnimationSprites[2] = attackAnimationSpritesSouth;
        attackAnimationSprites[3] = attackAnimationSpritesWest;

        castIdleAnimationSprites[0] = castIdleAnimationSpritesNorth;
        castIdleAnimationSprites[1] = castIdleAnimationSpritesEast;
        castIdleAnimationSprites[2] = castIdleAnimationSpritesSouth;
        castIdleAnimationSprites[3] = castIdleAnimationSpritesWest;

        castAnimationSprites[0] = castAnimationSpritesNorth;
        castAnimationSprites[1] = castAnimationSpritesEast;
        castAnimationSprites[2] = castAnimationSpritesSouth;
        castAnimationSprites[3] = castAnimationSpritesWest;

        deathAnimationSprites[0] = deathAnimationSpritesSouth;
    }
    Sprite[] animationSet;

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
        InitializeJaggedArray();
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
            case CurrentState.IdleAnimationNorth: PlayAnimation(0, 0, idleAnimationInterval, false); break;
            case CurrentState.IdleAnimationEast:  PlayAnimation(0, 1, idleAnimationInterval, false); break;
            case CurrentState.IdleAnimationSouth: PlayAnimation(0, 2, idleAnimationInterval, false); break;
            case CurrentState.IdleAnimationWest:  PlayAnimation(0, 3, idleAnimationInterval, false); break;

            case CurrentState.WalkingAnimationNorth: PlayAnimation(1, 0, walkAnimationInterval, false); break;
            case CurrentState.WalkingAnimationEast:  PlayAnimation(1, 1, walkAnimationInterval, false); break;
            case CurrentState.WalkingAnimationSouth: PlayAnimation(1, 2, walkAnimationInterval, false); break;
            case CurrentState.WalkingAnimationWest:  PlayAnimation(1, 3, walkAnimationInterval, false); break;

            case CurrentState.AttackIdleAnimationNorth: PlayAnimation(2, 0, attackIdleAnimationInterval, false); break;
            case CurrentState.AttackIdleAnimationEast:  PlayAnimation(2, 1, attackIdleAnimationInterval, false); break;
            case CurrentState.AttackIdleAnimationSouth: PlayAnimation(2, 2, attackIdleAnimationInterval, false); break;
            case CurrentState.AttackIdleAnimationWest:  PlayAnimation(2, 3, attackIdleAnimationInterval, false); break;

            case CurrentState.AttackingAnimationNorth:  PlayAnimation(3, 0, attackAnimationInterval, true); break;
            case CurrentState.AttackingAnimationEast:   PlayAnimation(3, 1, attackAnimationInterval, true); break;
            case CurrentState.AttackingAnimationSouth:  PlayAnimation(3, 2, attackAnimationInterval, true); break;
            case CurrentState.AttackingAnimationWest:   PlayAnimation(3, 3, attackAnimationInterval, true); break;

            case CurrentState.CastIdleAnimationNorth: PlayAnimation(4, 0, castAnimationInterval, false); break;
            case CurrentState.CastIdleAnimationEast: PlayAnimation(4, 1, castAnimationInterval, false); break;
            case CurrentState.CastIdleAnimationSouth: PlayAnimation(4, 2, castAnimationInterval, false); break;
            case CurrentState.CastIdleAnimationWest: PlayAnimation(4, 3, castAnimationInterval, false); break;

            case CurrentState.CastingAnimationNorth: PlayAnimation(5, 0, castAnimationInterval, true); break;
            case CurrentState.CastingAnimationEast:  PlayAnimation(5, 1, castAnimationInterval, true); break;
            case CurrentState.CastingAnimationSouth: PlayAnimation(5, 2, castAnimationInterval, true); break;
            case CurrentState.CastingAnimationWest:  PlayAnimation(5, 3, castAnimationInterval, true); break;

            case CurrentState.DeathAnimation: PlayAnimation(6, 0, deathAnimationInterval, true); break;

            default: // Default Behaviour. I.e. If statemachine does not recognize the state, it will default to this state that something is trying to put into it. (It's not in enum CurrentState?)
                Debug.Log("This state '" + currentState + "' for state machine doesn't exist, or there is a typo in name(string) of the state, defaulting to Idle State");
                SetState(CurrentState.IdleAnimationSouth);
                break;
        }

        AnimationTestingWithKeys(); // Testing
    }
    void PlayAnimation(int row, int column, float animationInterval, bool playOnce)
    {
        if (isAnimating) return;

        animationSet = animationSpritesJaggedArray[row][column];
        if (currentAnimationCoroutine != null) StopCoroutine(currentAnimationCoroutine);
        currentAnimationCoroutine = StartCoroutine(PlayAnimation(animationInterval, playOnce));
        isAnimating = true;
    }
    IEnumerator PlayAnimation(float time, bool playOnce)
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
