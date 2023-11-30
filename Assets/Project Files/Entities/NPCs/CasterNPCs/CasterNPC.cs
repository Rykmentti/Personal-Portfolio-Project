using DevPlz.CombatText;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterNPC : NPC2D
{
    [SerializeField] GameObject defaultSpellProjectile; // Set in editor for now.
    [SerializeField] GameObject homingSpellProjectile; // Set in editor for now.

    [SerializeField] int projectileSpeed;

    [SerializeField] bool defaultAttackCooldown;
    [SerializeField] bool heavyAttackCooldown;

    [SerializeField] float defaultAttackCooldownTime;
    [SerializeField] float heavyAttackCooldownTime;

    protected override void DoAttackActionFromListActions() // Do attack action, from a "list" of attacks, which are just if statements for now. Figure out a better way to do it later on. The ones on top go first and then it goes to the next one etc.
    {
        if (!heavyAttackCooldown) // Heavy Attack
        {
            StartCoroutine(HomingAttack());
        }
        else if (!defaultAttackCooldown) // Default Attack
        {
            StartCoroutine(DefaultAttack());
        }
    }
    IEnumerator DefaultAttack()
    {
        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationWest);
        StartCoroutine(GlobalCooldownCounter());
        StartCoroutine(DefaultAttackCooldownCounter());

        yield return new WaitForSeconds(1f);

        if (target == null) yield break; // If target is dead, we don't want to spawn projectiles.
        float angle = GetAngleBetweenTargetAndSelf();
        // Wow... hour wasted, only to figure out, that the Unity specifies rotations in counterclockwise manner, which means I have to add - in front angle value below. So clockwise rotation decreases value on z axis and counterclockwise rotation increases it? Gotta love it!
        Vector3 projectileStartRotation = new Vector3(0f, 0f, -angle);
        //Changing EulerAngles to Quaternions, so we can use them in Instantiate as parameter.
        Quaternion quaternion = Quaternion.Euler(projectileStartRotation);
        GameObject projectile = Instantiate(homingSpellProjectile, transform.position, quaternion);
        //Set damage and speed for children.
        CasterNPCProjectile projectileScript = projectile.GetComponentInChildren<CasterNPCProjectile>();
        projectileScript.SetDamageForChildren(damage);
        projectileScript.SetSpeedForChildren(projectileSpeed * 2); //Double speed for default attack.
        projectileScript.SetTargetForChildren(target);
        projectileScript.SetIdentifierTagForChildren(gameObject.tag);

        Vector3 dialogTextPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        CombatText.Spawn(TextStyle.CombatDialogue, "Default Attack!", dialogTextPosition, transform); // Visual Debugging.
    }
    IEnumerator HomingAttack()
    {
        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationWest);
        StartCoroutine(GlobalCooldownCounter());
        StartCoroutine(HeavyAttackCooldownCounter());

        yield return new WaitForSeconds(1f);

        float angle = GetAngleBetweenTargetAndSelf();
        // Wow... hour wasted, only to figure out, that the Unity specifies rotations in counterclockwise manner, which means I have to add - in front angle value below. So clockwise rotation decreases value on z axis and counterclockwise rotation increases it? Gotta love it!
        Vector3 projectileStartRotation = new Vector3(0f, 0f, -angle);
        //Changing EulerAngles to Quaternions, so we can use them in Instantiate as parameter.
        Quaternion quaternion = Quaternion.Euler(projectileStartRotation);
        GameObject projectile = Instantiate(homingSpellProjectile, transform.position, quaternion);
        //Set damage, speed and target for children.
        CasterNPCProjectile projectileScript = projectile.GetComponentInChildren<CasterNPCProjectile>();
        projectileScript.SetDamageForChildren(damage * 2); //Double damage for heavy attack.
        projectileScript.SetSpeedForChildren(projectileSpeed);
        projectileScript.SetTargetForChildren(target);
        projectileScript.SetIdentifierTagForChildren(gameObject.tag);

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
