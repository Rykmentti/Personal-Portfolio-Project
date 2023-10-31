using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedNPC : NPC2D
{
    [SerializeField] GameObject rangerArrow; // Set in editor for now.
    protected override void AttackingEnemy() // AttackingEnemy State. Cannot use override properly here, research what's wrong with it.
    {
        if (target == null) { SetState(CurrentState.FindNearestEnemy); return; }
        npcAgent.stoppingDistance = attackDistance;

        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationWest);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationSouth);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationEast);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationNorth);

        if (targetDestination != target.position) npcAgent.SetDestination(target.position); // No need to do calculations again for an object that isn't moving.
        if (npcAgent.remainingDistance > attackDistance) SetState(CurrentState.MovingToAttack);
        else if (!attackCooldown) StartCoroutine(DoDamage());
    }
    protected IEnumerator DoDamage() // Have to make this more modular later on, so I can change the style on the fly as well.
    {
        attackCooldown = true;

        float angle = GetAngleBetweenTargetAndSelf();
        //Miksi vitussa t‰ss‰ pit‰‰ rotationiin laittaa +90, ett‰ toi kaava pit‰‰ paikkansa, ku PlayerMiekassa sit‰ ei tarvi laittaa. wtf? :D
        Vector3 projectileStartRotation = new Vector3(0f, 0f, angle + 90);
        //Changing EulerAngles to Quaternions, so we can use them in Instantiate as parameter.
        Quaternion quaternion = Quaternion.Euler(projectileStartRotation);

        Instantiate(rangerArrow, transform.position, quaternion, transform);
        yield return new WaitForSeconds(attackRate);
        attackCooldown = false;
    }
    protected override void AttackSkill()
    {

    }
    protected override void DefenseSkill()
    {

    }
    protected override void CasterSkill()
    {

    }
}
