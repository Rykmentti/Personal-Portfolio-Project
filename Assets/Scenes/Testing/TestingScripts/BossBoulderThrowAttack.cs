using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBoulderThrowAttack : MonoBehaviour
{
    float damage;
    float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = 20f;
        damage = 25f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * new Vector2(0f, 1f));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Blocker"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player") && (PlayerController.rollActive == false))
        {
            PlayerController.PlayerReceiveDamage(damage);
            Destroy(gameObject);
        }
    }
}
