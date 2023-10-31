using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerMelee : EnemyManager
{

    public GameObject enemyMiekkaPrefab;
    public Animator animator;

    public Vector3 startRotation;
    public Vector2 localPosition;
    public float threshold;
    public float targetDistance;

    public bool enemyStrikeCooldown;
    bool isDead;
    // Start is called before the first frame update
    //void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 30, 200, 30), "Enemy Health: " + enemyHealth);
    //}
    void Start()
    {
        enemyHealth = 100;
        speed = 5;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        else if (isDead == false)
        {
            targetDistance = DistanceCalculation();
            if (targetDistance > 3.5 && targetDistance <= 30)
            {
                MoveTowardsPlayer();
            }
            if (targetDistance <= 3.5 && enemyStrikeCooldown == false)
            {
                EnemyStrike();
                StartCoroutine(EnemyStrikeCooldown());
            }
        }
        


    }
    /*
     * Vector2.Angle ei toimi koska, kohteen "direction" muuttuu liikkuessa ja arvot heittelee. Toimii hyvin jos kohde ei liiku.
     * Käytä alla olevaa kaavaa, jos haluat, että kahden pisteen välillä on AINA sama angle, aivan sama onko kohde liikkunu vai ei. +-180 astetta on väli.
     * Idässä on 0, Lännessä 180. Pohjoispuoli on 0-180, Eteläpuoli on 0-(-180)
     */
    void EnemyStrike()
    {
        float attackAnimationRunLength = 0.5f;

        float selfPosX = transform.position.x;
        float selfPosY = transform.position.y;
        float playerPosX = PlayerController.playerTransform.position.x;
        float playerPosY = PlayerController.playerTransform.position.y;

        Vector2 Point_1 = new Vector2(selfPosX,selfPosY);
        Vector2 Point_2 = new Vector2(playerPosX,playerPosY);
        float angle = Mathf.Atan2(Point_2.y - Point_1.y, Point_2.x - Point_1.x) * Mathf.Rad2Deg;

        ResetAnimatorBooleanValues();
        /*
        Debug.Log("Angle is = " + angle);
        */

        //East
        if (angle >= -45 && angle <= 45)
        {

            startRotation = new Vector3(0f, 0f, -45f);
            localPosition = new Vector2(1, 1);
            threshold = -0.92f;
            IEnumerator AnimationRunTime()
            {
                animator.SetBool("IsNotMovingRight", false);
                animator.SetBool("IsAttackingRight", true);
                yield return new WaitForSeconds(attackAnimationRunLength);
                animator.SetBool("IsAttackingRight", false);
                animator.SetBool("IsNotMovingRight", true);
            }
            StartCoroutine(AnimationRunTime());
        }
        //North
        else if (angle >= 45 && angle <= 135)
        {
            startRotation = new Vector3(0f, 0f, 45f);
            localPosition = new Vector2(-1, 1);
            threshold = -0.38f;
            IEnumerator AnimationRunTime()
            {
                animator.SetBool("IsNotMovingUp", false);
                animator.SetBool("IsAttackingUp", true);
                yield return new WaitForSeconds(attackAnimationRunLength);
                animator.SetBool("IsAttackingUp", false);
                animator.SetBool("IsNotMovingUp", true);
            }
            StartCoroutine(AnimationRunTime());
        }
        //South
        else if (angle >= -135 && angle <= -45)
        {

            startRotation = new Vector3(0f, 0f, 225f);
            localPosition = new Vector2(1, -1);
            threshold = 0.92f;
            IEnumerator AnimationRunTime()
            {
                animator.SetBool("IsNotMovingDown", false);
                animator.SetBool("IsAttackingDown", true);
                yield return new WaitForSeconds(attackAnimationRunLength);
                animator.SetBool("IsAttackingDown", false);
                animator.SetBool("IsNotMovingDown", true);
            }
            StartCoroutine(AnimationRunTime());
        }
        //West
        /*
         * Muista, > jatkuu loputtomiin ja moottori ei salli sitä rajaksi (ellet erikseen käytän Infinity ja MegativeInfinity). Arvot pitää törmätä, jotta tulee raja/true!
         * Eli aina muista, jos on kummatkin pelkästään >, niin ilmota rajat sille ja tarvii kaksi erillistä parametriä.
         * Tee niikuin alla, muista tämä!!! Taas kerran 2-3 tuntia aikaa hukkaan, ennen ku tajusit tän.
         * (else if angle >= 135 && angle <= -135) on aina false, eli EI TOIMI! Never again.
         */
        else if ((angle >= 135 && angle <= 180) || (angle <= -135 && angle >= -180)) 
        {
            startRotation = new Vector3(0f, 0f, 135f);
            localPosition = new Vector2(-1, -1);
            threshold = 0.38f;
            IEnumerator AnimationRunTime()
            {
                animator.SetBool("IsNotMovingLeft", false);
                animator.SetBool("IsAttackingLeft", true);
                yield return new WaitForSeconds(attackAnimationRunLength);
                animator.SetBool("IsAttackingLeft", false);
                animator.SetBool("IsNotMovingLeft", true);
            }
            StartCoroutine(AnimationRunTime());
        }
        Instantiate(enemyMiekkaPrefab, transform.position, enemyMiekkaPrefab.transform.rotation, transform.parent = transform);
    }
    float DistanceCalculation()
    {
        targetDistance = Vector2.Distance(PlayerController.playerTransform.position, transform.position);
        return targetDistance;
    }
    void MoveTowardsPlayer()
    {
        //Sets correct animation, when approaching player. ie. Walking towards the player.
        float selfPosX = transform.position.x;
        float selfPosY = transform.position.y;
        float playerPosX = PlayerController.playerTransform.position.x;
        float playerPosY = PlayerController.playerTransform.position.y;

        Vector2 Point_1 = new Vector2(selfPosX, selfPosY);
        Vector2 Point_2 = new Vector2(playerPosX, playerPosY);
        float angle = Mathf.Atan2(Point_2.y - Point_1.y, Point_2.x - Point_1.x) * Mathf.Rad2Deg;

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
    IEnumerator EnemyStrikeCooldown()
    {
        enemyStrikeCooldown = true;
        yield return new WaitForSeconds(3);
        enemyStrikeCooldown = false;
    }
    void ResetAnimatorBooleanValues()
    {
        animator.SetBool("IsMovingUp", false);
        animator.SetBool("IsMovingDown", false);
        animator.SetBool("IsMovingRight", false);
        animator.SetBool("IsMovingLeft", false);
        animator.SetBool("IsNotMovingUp", false);
        animator.SetBool("IsNotMovingDown", false);
        animator.SetBool("IsNotMovingRight", false);
        animator.SetBool("IsNotMovingLeft", false);
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
