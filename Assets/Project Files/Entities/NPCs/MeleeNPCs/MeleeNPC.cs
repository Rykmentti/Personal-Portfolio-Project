using DevPlz.CombatText;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeNPC : NPC2D
{
    [SerializeField] bool defaultAttackCooldown;
    [SerializeField] bool heavyAttackCooldown;

    [SerializeField] float defaultAttackCooldownTime;
    [SerializeField] float heavyAttackCooldownTime;
    protected override void DoAttackActionFromListActions() // Do attack action, from a "list" of attacks, which are just if statements for now. Figure out a better way to do it later on. The ones on top go first and then it goes to the next one etc.
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
    
    void DefaultAttack()
    {
        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) simpleSpriteAnimationController.SetState(SimpleSpriteAnimationController.CurrentState.AttackingAnimationWest);

        StartCoroutine(DoDamage(damage));
        StartCoroutine(GlobalCooldownCounter());
        StartCoroutine(DefaultAttackCooldownCounter());
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

        StartCoroutine(DoDamage(damage * 2));
        StartCoroutine(GlobalCooldownCounter());
        StartCoroutine(HeavyAttackCooldownCounter());
        Vector3 dialogTextPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        CombatText.Spawn(TextStyle.CombatDialogue, "Heavy Attack!", dialogTextPosition, transform); // Visual Debugging.
    }
    IEnumerator DoDamage(int damage)
    {
        yield return new WaitForSeconds(0.5f);
        if (target != null)
        {
            target.GetComponent<NPC2D>().ReceiveDamage(damage);
            Debug.Log(gameObject.name + " dealt Damage to " + target.name + "!");
        }
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
