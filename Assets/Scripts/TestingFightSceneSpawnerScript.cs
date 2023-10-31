using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingFightSceneSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject meleeEnemy;
    [SerializeField] GameObject rangedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Instantiate(meleeEnemy, new Vector2(-3, 6), meleeEnemy.transform.rotation);
            Instantiate(meleeEnemy, new Vector2(3, 6), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(-7, 9), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(0, 9), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(7, 9), meleeEnemy.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Instantiate(meleeEnemy, new Vector2(-10, -10), meleeEnemy.transform.rotation);
            Instantiate(meleeEnemy, new Vector2(-10, 10), meleeEnemy.transform.rotation);
            Instantiate(meleeEnemy, new Vector2(10, -10), meleeEnemy.transform.rotation);
            Instantiate(meleeEnemy, new Vector2(10, 10), meleeEnemy.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Instantiate(rangedEnemy, new Vector2(-10, 10), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(10, 10), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(-5, 10), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(5, 10), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(0, 10), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(3, 7), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(-3, 7), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(6, 7), meleeEnemy.transform.rotation);
            Instantiate(rangedEnemy, new Vector2(-6, 7), meleeEnemy.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerController.playerHealth += 100;
        }
    }
}
