using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerRanged : EnemyManager
{
    public GameObject enemyArrowPrefab;
    public Animator animator;

    public float targetDistance;
    public float angle;

    public bool enemyShootCooldown;
    bool isDead;

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 50, 200, 30), "Ranged Enemy Health: " + enemyHealth);
    //}
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = 100;
        speed = 5;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("TargetDistance is = " + targetDistance);
        }

        if (enemyHealth < 1)
        {
            isDead = true;
        }
        if (isDead == true)
        {
            StopAllCoroutines();

            ResetAnimatorBooleanValues();
            animator.SetBool("IsDead", true);
            Invoke("SelfDestroy", 1f);
        }
        else if(isDead == false)
        {
            targetDistance = DistanceCalculation();
            if (targetDistance >= 15 && targetDistance <= 30)
            {
                MoveTowardsPlayer();
            }
            if (targetDistance <= 15 && enemyShootCooldown == false)
            {
                EnemyShoot();
                StartCoroutine(EnemyShootCooldown());
            }
        } 
    }

    void EnemyShoot()
    {
        ResetAnimatorBooleanValues();
        AngleCalculation();
        float attackAnimationRunLength = 1.2f;

        StartCoroutine(DelayedShoot());
        //East
        if (angle >= -45 && angle <= 45)
        {
            IEnumerator AnimationRunTime()
            {
                animator.SetBool("IsNotAttackingRight", false);
                animator.SetBool("IsAttackingRight", true);
                yield return new WaitForSeconds(attackAnimationRunLength);
                animator.SetBool("IsAttackingRight", false);
                animator.SetBool("IsNotAttackingRight", true);
            }
            StartCoroutine(AnimationRunTime());
        }
        //North
        else if (angle >= 45 && angle <= 135)
        {
            IEnumerator AnimationRunTime()
            {
                animator.SetBool("IsNotAttackingUp", false);
                animator.SetBool("IsAttackingUp", true);
                yield return new WaitForSeconds(attackAnimationRunLength);
                animator.SetBool("IsAttackingUp", false);
                animator.SetBool("IsNotAttackingUp", true);
            }
            StartCoroutine(AnimationRunTime());
        }
        //South
        else if (angle >= -135 && angle <= -45)
        {
            IEnumerator AnimationRunTime()
            {
                animator.SetBool("IsNotAttackingDown", false);
                animator.SetBool("IsAttackingDown", true);
                yield return new WaitForSeconds(attackAnimationRunLength);
                animator.SetBool("IsAttackingDown", false);
                animator.SetBool("IsNotAttackingDown", true);
            }
            StartCoroutine(AnimationRunTime());
        }
        //West
        else if ((angle >= 135 && angle <= 180) || (angle <= -135 && angle >= -180))
        {
            IEnumerator AnimationRunTime()
            {
                animator.SetBool("IsNotAttackingLeft", false);
                animator.SetBool("IsAttackingLeft", true);
                yield return new WaitForSeconds(attackAnimationRunLength);
                animator.SetBool("IsAttackingLeft", false);
                animator.SetBool("IsNotAttackingLeft", true);
            }
            StartCoroutine(AnimationRunTime());
        }

        IEnumerator DelayedShoot()
        {
            yield return new WaitForSeconds(0.8f);

            AngleCalculation();
            //Miksi vitussa tässä pitää rotationiin laittaa -90, että toi kaava pitää paikkansa, ku PlayerMiekassa sitä ei tarvi laittaa. wtf? :D
            Vector3 projectileStartRotation = new Vector3(0f, 0f, angle - 90);
            //Changing EulerAngles to Quaternions, so we can use them in Instantiate as parameter.
            Quaternion quaternion = Quaternion.Euler(projectileStartRotation);

            Instantiate(enemyArrowPrefab, transform.position, quaternion); 
        }
    }
    void MoveTowardsPlayer()
    {
        //Sets correct animation, when approaching player. ie. Walking towards the player.
        AngleCalculation();

        //East
        if (angle >= -45 && angle <= 45)
        {
            ResetAnimatorBooleanValues();
            animator.SetBool("IsMovingRight", true);
        }
        //North
        else if (angle >= 45 && angle <= 135)
        {
            ResetAnimatorBooleanValues();
            animator.SetBool("IsMovingUp", true);

        }
        //South
        else if (angle >= -135 && angle <= -45)
        {
            ResetAnimatorBooleanValues();
            animator.SetBool("IsMovingDown", true);
        }
        //West
        else if ((angle >= 135 && angle <= 180) || (angle <= -135 && angle >= -180))
        {
            ResetAnimatorBooleanValues();
            animator.SetBool("IsMovingLeft", true);
        }

        //Physically Moves character towards player.
        Vector2 moveDirection = (PlayerController.playerTransform.position - transform.position).normalized;
        transform.Translate(speed * Time.deltaTime * moveDirection);
    }
    void ResetAnimatorBooleanValues()
    {
        animator.SetBool("IsMovingUp", false);
        animator.SetBool("IsMovingDown", false);
        animator.SetBool("IsMovingRight", false);
        animator.SetBool("IsMovingLeft", false);
        animator.SetBool("IsNotAttackingUp", false);
        animator.SetBool("IsNotAttackingDown", false);
        animator.SetBool("IsNotAttackingRight", false);
        animator.SetBool("IsNotAttackingLeft", false);
        animator.SetBool("IsAttackingUp", false);
        animator.SetBool("IsAttackingDown", false);
        animator.SetBool("IsAttackingRight", false);
        animator.SetBool("IsAttackingLeft", false);
    }
    float DistanceCalculation()
    {
        targetDistance = Vector2.Distance(PlayerController.playerTransform.position, transform.position);
        return targetDistance;
    }
    //Calculating angle between player and our current position.
    float AngleCalculation()
    {
        float selfPosX = transform.position.x;
        float selfPosY = transform.position.y;
        float playerPosX = PlayerController.playerTransform.position.x;
        float playerPosY = PlayerController.playerTransform.position.y;

        Vector2 Point_1 = new Vector2(selfPosX, selfPosY);
        Vector2 Point_2 = new Vector2(playerPosX, playerPosY);
        angle = Mathf.Atan2(Point_2.y - Point_1.y, Point_2.x - Point_1.x) * Mathf.Rad2Deg;
        return angle;
    }
    IEnumerator EnemyShootCooldown()
    {
        enemyShootCooldown = true;
        yield return new WaitForSeconds(1.2f);
        enemyShootCooldown = false;
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
