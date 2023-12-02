using DevPlz.CombatText;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPC2D : MonoBehaviour
{
    // Base Inheritance Script for the NPC, every script that is NPC should inherit this. Tag should be assigned in the editor, at least for now for convience.
    // Add tag in the prefab or editor, to make it into it's own "faction" like Enemy or Ally and have everyone else be enemy, except in Ally player will be an ally.

    // Communicates with DetectorScript to find targets, which should be attached to children object of the NPC. DetectorScripts assigns targets, based on needs of the NPC?
    // Should communicate with SimpleSpriteAnimator for sprite animation, that I myself made to make frame-by-frame animation easier to implement.
    protected NavMeshAgent npcAgent;
    protected DetectorScript2D detectorScript;
    protected SimpleSpriteAnimationController simpleSpriteAnimationController;
    
    protected Transform target;
    protected Vector3 targetDestination;
    protected float targetAngle;
    [SerializeField] protected float targetDistance;

    // Setting battle related values in editor for now, for testing purposes.
    [SerializeField] protected bool globalCooldown;
    [SerializeField] protected float globalCooldownTime;
    [SerializeField] protected float attackDistance;
    [SerializeField] int npcDeployValue;
    [SerializeField] protected int health;
    [SerializeField] protected int damage;

    protected bool inCombat;
    protected bool isDead;


    protected enum CurrentState
    {
        Idle, SuspendStateMachine, MovingToAttack, AttackingEnemy, FindNearestEnemy,
    }

    [SerializeField] CurrentState currentState;

    protected void SetState(CurrentState state) // Method we use to change states in the state machine.
    {
        currentState = state;
    }
    // Start is called before the first frame update
    protected void Start()
    {
        if (gameObject.tag == "Blue") UI_ManagerBattleScene.uiManagerBattleScene.UpdatePlayerNPCTotalValueText(npcDeployValue);
        else if (gameObject.tag == "Red") UI_ManagerBattleScene.uiManagerBattleScene.UpdateEnemyNPCTotalValueText(npcDeployValue);

        // Purkka at it's finest. Just to showcase other how this could work in class. :D If we actually end up using this, we need to create one script dedicated wholly to scrolling combat text.
        Vector3 dialogTextPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        npcAgent = GetComponent<NavMeshAgent>();
        npcAgent.updateRotation = false; // Can't have this with 2D sprites using NavMesh.
        npcAgent.updateUpAxis = false; // Can't have this with 2D sprites using NavMesh.

        detectorScript = GetComponentInChildren<DetectorScript2D>();
        simpleSpriteAnimationController = GetComponent<SimpleSpriteAnimationController>();
        SetState(CurrentState.Idle);
    }
    // Update is called once per frame
    protected void Update() // Main State Machine Body.
    {
        if (target != null)
        {
            targetDistance = npcAgent.remainingDistance;
            targetAngle = GetAngleBetweenTargetAndSelf();
        }
        TestingWithKeys(); // Testing

        switch (currentState)
        {
            case CurrentState.SuspendStateMachine: HoldBehaviour(); break;
            case CurrentState.Idle: IdleBehaviour(); break;
            case CurrentState.FindNearestEnemy: FindNearestEnemy(); break;
            case CurrentState.MovingToAttack: MoveToAttackNearestEnemy(); break;
            case CurrentState.AttackingEnemy: AttackingEnemy(); break;

            default: // Default Behaviour. I.e. If statemachine does not recognize the state, it will default to this state that something is trying to put into it. (It's not in enum CurrentState?)
                Debug.Log("This state '" + currentState + "' for state machine doesn't exist, or there is a typo in name(string) of the state, defaulting to Idle State");
                SetState(CurrentState.Idle);
                break;
        }
    }
    // Start of State Machine behaviours.
    protected void HoldBehaviour() // Hold Behaviour, which doesn't contain anything. I use it to temporarily suspend the state machime. I.e Do Nothing/State Machine Interupt method, while your method is executing or an animation is playing.
    {

    }
    protected void IdleBehaviour() // Idle Behaviour. This is the behaviour that is on, when the gameObject is passive. I.e, enemies are not detected/nearby so AI is wandering or something like that. Could also be used as default behaviour.
    {
        if (detectorScript.FindNearestTarget() != null && !inCombat)
        {
            inCombat = true;
            SetState(CurrentState.FindNearestEnemy);
            Debug.Log(transform.name + " entering Combat!");
        }
        else if (target == null && inCombat) // Victory/End Combat Check.
        {
            inCombat = false;
            simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.IdleAnimationSouth);
            Debug.Log("Enemy list is empty, all enemies have been defeated. We are victorious!");

            // Just to showcase how battle banter and dialogue could work.
            Vector3 dialogTextPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            CombatText.Spawn(TextStyle.Dialogue, "We are victorious! Hurrah!!!", dialogTextPosition, transform);
        }
    }
    protected void FindNearestEnemy() // FindNearestEnemy State
    {
        if (detectorScript.FindNearestTarget() == null) { SetState(CurrentState.Idle); return; }

        target = detectorScript.FindNearestTarget();
        Debug.Log(gameObject.name + " has been assigned nearest target: " + target.name);
        SetState(CurrentState.MovingToAttack);
        Debug.Log(gameObject.name + " is moving to attack " + target.name + "!");
    }
    // General Behaviors for Combat?
    protected void MoveToAttackNearestEnemy() // MovingToAttack State
    {
        if (target == null) { SetState(CurrentState.FindNearestEnemy); return; }

        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationWest);

        if (targetDestination != target.position) // No need to do calculations again for an object that isn't moving.
        {
            targetDestination = target.position;
            npcAgent.SetDestination(target.position);
        }

        if (npcAgent.remainingDistance != 0 && npcAgent.remainingDistance < attackDistance) // Need to make sure 0 isn't a valid value, otherwise during the first frame when the value is temporarily set zero by NavMeshAgent, condition will be true.
        {
            simpleSpriteAnimationController.KillCurrentAnimationCoroutine(); // Killing the animation coroutine, so the old one animation isn't hanging around after we switch states.
            SetState(CurrentState.AttackingEnemy);
        } 
    }
    protected virtual void AttackingEnemy() // AttackingEnemy State, Overridable from inherited variants.
    {
        if (target == null) { SetState(CurrentState.FindNearestEnemy); return; }
        npcAgent.stoppingDistance = attackDistance; // Need to set the attack distance here, to make sure attack range is consistent if we change the range for whatever reason.

        if (targetDestination != target.position) // No need to do calculations again for an object that isn't moving.
        {
            targetDestination = target.position;
            npcAgent.SetDestination(target.position);
        }
        if (npcAgent.remainingDistance > attackDistance) SetState(CurrentState.MovingToAttack);
        else if (!globalCooldown) DoAttackActionFromListActions();
        else if (simpleSpriteAnimationController.IsAnimating() == false) FaceEnemyWithWeaponDrawn(); // If we are not attacking, we are facing the enemy with our weapon drawn.
    }
    // End of State Machine behaviours.
    protected void TestingWithKeys()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Testing
        }
    }
    protected float GetAngleBetweenTargetAndSelf() // Calculating angles always start from North. I.e. North = 0/360, East = 90, South = 180 and West = 270.
    {
        float angle = Mathf.Atan2(target.position.x - transform.position.x, target.position.y - transform.position.y) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f; // Making sure value is between 0 and 360 degrees, instead of 180 and -180.
        return angle;
    }
    public void ReceiveDamage(int damage)
    {
        Vector3 scrollingCombatTextPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        CombatText.Spawn(TextStyle.DamageEnemy, damage.ToString(), scrollingCombatTextPosition, transform);
        health -= damage;

        if (health < 0 && !isDead) StartCoroutine(DeathSequence());
    }
    public int SetDamageForChildren()
    {
        int damageForChildren = damage;
        return damageForChildren;
    }
    public Transform SetTargetForChildren()
    {
        Transform targetForChildren = target;
        return targetForChildren;
    }
    protected IEnumerator DeathSequence()
    {
        isDead = true;
        SetState(CurrentState.SuspendStateMachine);
        simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.DeathAnimation);
        Debug.Log(gameObject.name + " has fallen in battle!");
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    protected IEnumerator GlobalCooldownCounter()
    {
        globalCooldown = true;
        yield return new WaitForSeconds(globalCooldownTime);
        globalCooldown = false;
    }
    protected virtual void DoAttackActionFromListActions() { } // Overridable attack action. Does attack from a "list" of attacks.
    protected void FaceEnemyWithWeaponDrawn()
    {
        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackIdleAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackIdleAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackIdleAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackIdleAnimationWest);
    }

    void OnDrawGizmosSelected() // Visualizing attackDistance in editor if NPC is selected.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
    public int GetNPCDeployValue()
    {
        return npcDeployValue;
    }
    //void OnEnable() // Object fooling. I.e. If we have a pool of objects, we can use this to update the UI when the object is enabled.
    //{
    //    if (gameObject.tag == "Blue") UI_Manager.uiManager.UpdatePlayerNPCTotalValueText(npcDeployValue);
    //    else if (gameObject.tag == "Red") UI_Manager.uiManager.UpdateEnemyNPCTotalValueText(npcDeployValue);
    //}
    //void OnDisable()
    //{
    //    if (gameObject.tag == "Blue") UI_Manager.uiManager.UpdatePlayerNPCTotalValueText(-npcDeployValue);
    //    else if (gameObject.tag == "Red") UI_Manager.uiManager.UpdateEnemyNPCTotalValueText(-npcDeployValue);
    //}
    void OnDestroy()
    {
        if (gameObject.tag == "Blue") UI_ManagerBattleScene.uiManagerBattleScene.UpdatePlayerNPCTotalValueText(-npcDeployValue);
        else if (gameObject.tag == "Red") UI_ManagerBattleScene.uiManagerBattleScene.UpdateEnemyNPCTotalValueText(-npcDeployValue);
    }
}
