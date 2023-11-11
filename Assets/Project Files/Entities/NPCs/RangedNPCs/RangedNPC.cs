using DevPlz.CombatText;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedNPC : NPC2D
{
    [SerializeField] int projectileSpeed;

    [SerializeField] bool defaultAttackCooldown;
    [SerializeField] bool heavyAttackCooldown;

    [SerializeField] float defaultAttackCooldownTime;
    [SerializeField] float heavyAttackCooldownTime;

    [SerializeField] GameObject rangerArrow; // Set in editor for now.
    protected override void AttackingEnemy() // AttackingEnemy State. Cannot use override properly here, research what's wrong with it.
    {
        if (target == null) { SetState(CurrentState.FindNearestEnemy); return; }
        npcAgent.stoppingDistance = attackDistance;
        if (targetDestination != target.position) npcAgent.SetDestination(target.position); // No need to do calculations again for an object that isn't moving.
        if (npcAgent.remainingDistance > attackDistance) SetState(CurrentState.MovingToAttack);
        else if (!globalCooldown) DoAttackActionFromListActions();
        else if (simpleSpriteAnimationController.IsAnimating() == false) FaceEnemyWithWeaponDrawn(); // If we are not attacking, we are facing the enemy with our weapon drawn.
    }

    void DoAttackActionFromListActions() // Do attack action, from a "list" of attacks, which are just if statements for now. Figure out a better way to do it later on. The ones on top go first and then it goes to the next one etc.
    {
        if (!globalCooldown)
        {
            if (!heavyAttackCooldown) // Heavy Attack
            {
                HeavyAttack();
            }
            else if (!defaultAttackCooldown) // Default Attack
            {
                DefaultAttack();
            }
        }
    }
    void FaceEnemyWithWeaponDrawn()
    {
        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackIdleAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackIdleAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackIdleAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackIdleAnimationWest);
    }
    void DefaultAttack()
    {
        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationWest);

        float angle = GetAngleBetweenTargetAndSelf();
        // Wow... hour wasted, only to figure out, that the Unity specifies rotations in counterclockwise manner, which means I have to add - in front angle value below. So clockwise rotation decreases value on z axis and counterclockwise rotation increases it? Gotta love it!
        Vector3 projectileStartRotation = new Vector3(0f, 0f, -angle);
        //Changing EulerAngles to Quaternions, so we can use them in Instantiate as parameter.
        Quaternion quaternion = Quaternion.Euler(projectileStartRotation);
        GameObject projectile = Instantiate(rangerArrow, transform.position, quaternion);
        StartCoroutine(GlobalCooldownCounter());
        StartCoroutine(DefaultAttackCooldownCounter());

        //Set damage and speed for children.
        RangerArrow projectileScript = projectile.GetComponentInChildren<RangerArrow>();
        projectileScript.SetDamageForChildren(damage ); 
        projectileScript.SetSpeedForChildren(projectileSpeed * 2); //Double speed for default attack.
        projectile.tag = gameObject.tag; // Projectile is same tag the creator.

        Vector3 dialogTextPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        CombatText.Spawn(TextStyle.CombatDialogue, "Default Attack!", dialogTextPosition, transform); // Visual Debugging.
    }
    void HeavyAttack()
    {
        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationWest);

        float angle = GetAngleBetweenTargetAndSelf();
        // Wow... hour wasted, only to figure out, that the Unity specifies rotations in counterclockwise manner, which means I have to add - in front angle value below. So clockwise rotation decreases value on z axis and counterclockwise rotation increases it? Gotta love it!
        Vector3 projectileStartRotation = new Vector3(0f, 0f, -angle);
        //Changing EulerAngles to Quaternions, so we can use them in Instantiate as parameter.
        Quaternion quaternion = Quaternion.Euler(projectileStartRotation);
        GameObject projectile = Instantiate(rangerArrow, transform.position, quaternion);
        StartCoroutine(GlobalCooldownCounter());
        StartCoroutine(HeavyAttackCooldownCounter());

        //Set damage and speed for children.
        RangerArrow projectileScript = projectile.GetComponentInChildren<RangerArrow>();
        projectileScript.SetDamageForChildren(damage * 2); //Double damage for heavy attack.
        projectileScript.SetSpeedForChildren(projectileSpeed);
        projectile.tag = gameObject.tag; // Projectile is same tag as the creator.

        Vector3 dialogTextPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        CombatText.Spawn(TextStyle.CombatDialogue, "Heavy Attack!", dialogTextPosition, transform); // Visual Debugging.
    }
    protected IEnumerator HeavyAttackCooldownCounter()
    {
        heavyAttackCooldown = true;
        yield return new WaitForSeconds(heavyAttackCooldownTime);
        heavyAttackCooldown = false;
    }
    protected IEnumerator DefaultAttackCooldownCounter()
    {
        defaultAttackCooldown = true;
        yield return new WaitForSeconds(defaultAttackCooldownTime);
        defaultAttackCooldown = false;
    }
}
