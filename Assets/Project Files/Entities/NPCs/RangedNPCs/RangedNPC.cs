using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedNPC : NPC2D
{
    bool defaultAttackCooldown;

    [SerializeField] GameObject rangerArrow; // Set in editor for now.
    protected override void AttackingEnemy() // AttackingEnemy State. Cannot use override properly here, research what's wrong with it.
    {
        if (target == null) { SetState(CurrentState.FindNearestEnemy); return; }
        npcAgent.stoppingDistance = attackDistance;

        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationWest);

        if (targetDestination != target.position) npcAgent.SetDestination(target.position); // No need to do calculations again for an object that isn't moving.
        if (npcAgent.remainingDistance > attackDistance) SetState(CurrentState.MovingToAttack);
        else if (!defaultAttackCooldown) StartCoroutine(DoDamage());
    }
    protected IEnumerator DoDamage() // Have to make this more modular later on, so I can change the style on the fly as well.
    {
        defaultAttackCooldown = true;

        float angle = GetAngleBetweenTargetAndSelf();
        // Wow... hour wasted, only to figure out, that the Unity specifies rotations in counterclockwise manner, which means I have to add - in front angle value below. So clockwise rotation decreases value on z axis and counterclockwise rotation increases it? Gotta love it!
        Vector3 projectileStartRotation = new Vector3(0f, 0f, -angle);
        //Changing EulerAngles to Quaternions, so we can use them in Instantiate as parameter.
        Quaternion quaternion = Quaternion.Euler(projectileStartRotation);

        Instantiate(rangerArrow, transform.position, quaternion, transform);
        yield return new WaitForSeconds(attackRate);
        defaultAttackCooldown = false;
    }
}
