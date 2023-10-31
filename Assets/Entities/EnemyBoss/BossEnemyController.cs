using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyController : EnemyManager
{
    public GameObject bossCirclingSword;
    public GameObject bossMiekkaPrefab;
    public GameObject bossArrowPrefab;

    public Vector3 startRotation;
    public Vector2 localPosition;
    public float threshold;
    public float targetDistance;

    public bool bossCirclingSwordsCooldown;
    public bool bossStrikeCooldown;
    public bool bossShootCooldown;
    public bool holdingPosition;

    public bool bossPhase2;
    public bool bossPhase2CirclingSwordsOnlyExecuteOnce;
    public bool bossPhase3;
    public bool bossPhase3CirclingSwordsOnlyExecuteOnce;

    // Start is called before the first frame update
    void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 200, 30), "Enemy Health: " + enemyHealth);
    }
    void Start()
    {
        enemyHealth = 100;
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = PlayerDistanceCalculation();
        if (enemyHealth < 1)
        {
            Destroy(gameObject);
        }
        
        if (targetDistance >= 3.5 && targetDistance <= 30 && holdingPosition == false)
        {
            MoveTowardsPlayer();
        }
        
        if (targetDistance <= 3.5 && bossStrikeCooldown == false)
        {
            AIstrike();
            StartCoroutine(BossStrikeCooldown());
        }
        if (targetDistance <= 10 && bossShootCooldown == false)
        {
            BossShoot();
            if (bossPhase2 == true)
            {
                StartCoroutine(Phase2Barrage());
            }
            StartCoroutine(BossShootCooldown());

        }
        //CirclingSwords/Pyörivät miekat mekaniikka
        //Local variablet deletoituu/unloadaa aina kun method on executennu. Ei pysty tekemään onlyExecuteOnce methodia, joka toimii vain kerran, local variableilla.
        //Jos haluat, että automaattisesti, copypastaamalla tulee onlyExecuteOnce method, methodin nimen perusteella, sun pitää tehdä siitä onlyExecuteOnce boolista globaali, methodin sisällä.
        //Mieti tulevaisuudessa, teetkö sen vai onko ihan sama, koska se, että teet tämmösen viritelmän maksaa sun aikaa 15 sekuntia.
        if (bossPhase2 == true && bossPhase2CirclingSwordsOnlyExecuteOnce == false)
        {
            Instantiate(bossCirclingSword, transform.position, bossCirclingSword.transform.rotation, transform.parent = transform);
            bossPhase2CirclingSwordsOnlyExecuteOnce = true;
        }
        if (bossPhase3 == true)
        {
            if (bossPhase3 == true && bossPhase3CirclingSwordsOnlyExecuteOnce == false)
            {
                StartCoroutine(CirclingSwords());
                bossPhase3CirclingSwordsOnlyExecuteOnce = true;
            }
        }
        /*
         * Vector2.Angle ei toimi koska, kohteen "direction" muuttuu liikkuessa ja arvot heittelee. Toimii hyvin jos kohde ei liiku.
         * Käytä alla olevaa kaavaa, jos haluat, että kahden pisteen välillä on AINA sama angle, aivan sama onko kohde liikkunu vai ei. +-180 astetta on väli.
         * Idässä on 0, Lännessä 180. Pohjoispuoli on 0-180, Eteläpuoli on 0-(-180)
         */
        void AIstrike()
        {
            float selfPosX = transform.position.x;
            float selfPosY = transform.position.y;
            float playerPosX = PlayerController.playerTransform.position.x;
            float playerPosY = PlayerController.playerTransform.position.y;

            Vector2 Point_1 = new Vector2(selfPosX, selfPosY);
            Vector2 Point_2 = new Vector2(playerPosX, playerPosY);
            float angle = Mathf.Atan2(Point_2.y - Point_1.y, Point_2.x - Point_1.x) * Mathf.Rad2Deg;

            /*
            Debug.Log("Angle is = " + angle);
            */

            //East
            if (angle >= -45 && angle <= 45)
            {

                startRotation = new Vector3(0f, 0f, -45f);
                localPosition = new Vector2(1, 1);
                threshold = -0.92f;
            }
            //North
            else if (angle >= 45 && angle <= 135)
            {
                startRotation = new Vector3(0f, 0f, 45f);
                localPosition = new Vector2(-1, 1);
                threshold = -0.38f;
            }
            //South
            else if (angle >= -135 && angle <= -45)
            {

                startRotation = new Vector3(0f, 0f, 225f);
                localPosition = new Vector2(1, -1);
                threshold = 0.92f;
            }
            //West
            /*
             * Muista, > jatkuu loputtomiin ja moottori ei salli sitä rajaksi (ellet erikseen käytän Infinity ja MegativeInfinity). Arvot pitää törmätä, jotta tulee raja/true!
             * Eli aina muista, jos on kummatkin pelkästään >, niin ilmota rajat sille ja tarvii kaksi erillistä parametriä.
             * Tee niikuin alla, muista tämä!!! Taas kerran 2-3 tuntia aikaa hukkaan, ennen ku tajusit tän.
             * (else if angle >= 135 && angle <= -135) on aina false, eli EI TOIMI! Never again.
             */
            else if ((angle >= 135 && angle <= 180) || (angle >= -135 && angle <= -180))
            {
                startRotation = new Vector3(0f, 0f, 135f);
                localPosition = new Vector2(-1, -1);
                threshold = 0.38f;
            }
            Instantiate(bossMiekkaPrefab, transform.position, bossMiekkaPrefab.transform.rotation, transform.parent = transform);
        }
        /* Boss Shoot/projectile logiikka. Laskee pelaajan ja omat kordinaatit ja niitten välisen kulman.
         * Tämä on ampumiskulman, missä päin pelaaja on. Instantiate sitten spawnaa projectilen pelaajaa päin suunnattuna.)
         */
        void BossShoot()
        {
            float selfPosX = transform.position.x;
            float selfPosY = transform.position.y;
            float playerPosX = PlayerController.playerTransform.position.x;
            float playerPosY = PlayerController.playerTransform.position.y;

            Vector2 Point_1 = new Vector2(selfPosX, selfPosY);
            Vector2 Point_2 = new Vector2(playerPosX, playerPosY);
            float rotation = Mathf.Atan2(Point_2.y - Point_1.y, Point_2.x - Point_1.x) * Mathf.Rad2Deg;

            //Miksi vitussa tässä pitää rotationiin laittaa -90, että toi kaava pitää paikkansa, ku PlayerMiekassa sitä ei tarvi laittaa. wtf? :D
            Vector3 projectileStartRotation = new Vector3(0f, 0f, rotation - 90);
            Quaternion quaternion = Quaternion.Euler(projectileStartRotation);

            /*
            Debug.Log("Rotation is = " + rotation);
            Debug.Log("StartRotation is = " + startRotation);
            */
            Instantiate(bossArrowPrefab, transform.position, quaternion);
            // Phase 3 Ominaisuus, 5 projectilea, mitkä forkkaa, kaikki eri kulmilla, joka tekee conen.
            if (bossPhase3 == true)
            {
                Vector3 children1Rotation = new Vector3(0f, 0f, rotation - 60);
                Vector3 children2Rotation = new Vector3(0f, 0f, rotation - 75);
                Vector3 children3Rotation = new Vector3(0f, 0f, rotation - 105);
                Vector3 children4Rotation = new Vector3(0f, 0f, rotation - 120);

                Quaternion children1Quaternion = Quaternion.Euler(children1Rotation);
                Quaternion children2Quaternion = Quaternion.Euler(children2Rotation);
                Quaternion children3Quaternion = Quaternion.Euler(children3Rotation);
                Quaternion children4Quaternion = Quaternion.Euler(children4Rotation);

                Instantiate(bossArrowPrefab, transform.position, children1Quaternion);
                Instantiate(bossArrowPrefab, transform.position, children2Quaternion);
                Instantiate(bossArrowPrefab, transform.position, children3Quaternion);
                Instantiate(bossArrowPrefab, transform.position, children4Quaternion);
            }
        }
        float PlayerDistanceCalculation()
        {
            targetDistance = Vector2.Distance(PlayerController.playerTransform.position, transform.position);
            return targetDistance;
        }
        void MoveTowardsPlayer()
        {
            Vector2 lookDirection = (PlayerController.playerTransform.position - transform.position).normalized;
            transform.Translate(lookDirection * speed * Time.deltaTime);
        }
        IEnumerator BossShootCooldown()
        {
            bossShootCooldown = true;
            yield return new WaitForSeconds(3);
            bossShootCooldown = false;
        }
        IEnumerator BossStrikeCooldown()
        {
            bossStrikeCooldown = true;
            yield return new WaitForSeconds(3);
            bossStrikeCooldown = false;
        }
        IEnumerator Phase2Barrage()
        {
            holdingPosition = true;
            yield return new WaitForSeconds((float)0.15);
            BossShoot();
            yield return new WaitForSeconds((float)0.15);
            BossShoot();
            yield return new WaitForSeconds((float)0.15);
            BossShoot();
            yield return new WaitForSeconds((float)0.15);
            BossShoot();
            yield return new WaitForSeconds((float)0.5);
            holdingPosition = false;

        }
        IEnumerator CirclingSwords()
        {
            Instantiate(bossCirclingSword, transform.position, bossCirclingSword.transform.rotation, transform.parent = transform);
            yield return new WaitForSeconds((float)0.2);
            Instantiate(bossCirclingSword, transform.position, bossCirclingSword.transform.rotation, transform.parent = transform);
            yield return new WaitForSeconds((float)0.2);
            Instantiate(bossCirclingSword, transform.position, bossCirclingSword.transform.rotation, transform.parent = transform);
            yield return new WaitForSeconds((float)0.2);
            Instantiate(bossCirclingSword, transform.position, bossCirclingSword.transform.rotation, transform.parent = transform);
            yield return new WaitForSeconds((float)0.2);
            Instantiate(bossCirclingSword, transform.position, bossCirclingSword.transform.rotation, transform.parent = transform);
        }
    }
}