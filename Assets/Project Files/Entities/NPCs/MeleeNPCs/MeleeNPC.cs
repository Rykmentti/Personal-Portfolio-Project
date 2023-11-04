using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeNPC : NPC2D
{
    protected override void AttackingEnemy() // AttackingEnemy State. Cannot use override properly here, research what's wrong with it.
    {
        if (target == null) { SetState(CurrentState.FindNearestEnemy); return; }
        npcAgent.stoppingDistance = attackDistance;

        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationWest);

        if (targetDestination != target.position) // No need to do calculations again for an object that isn't moving.
        {
            targetDestination = target.position;
            npcAgent.SetDestination(target.position);
        }
        if (npcAgent.remainingDistance > attackDistance) SetState(CurrentState.MovingToAttack);
        else if (!attackCooldown) StartCoroutine(DoDamage());
    }
    /*
    protected override void AttackingEnemy() // This doesn't work. Why? It is ignoring the first if statement from the base class for some reason. I have to override entire class and copy paste.
    {
        base.AttackingEnemy();
        if (!attackCooldown) StartCoroutine(DoDamage());
    }
    */
    protected IEnumerator DoDamage() // Have to make this more modular later on, so I can change the style on the fly as well.
    {
        attackCooldown = true;
        target.GetComponent<NPC2D>().ReceiveDamage(damage);
        Debug.Log(gameObject.name + " dealt Damage to " + target.name + "!");
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
