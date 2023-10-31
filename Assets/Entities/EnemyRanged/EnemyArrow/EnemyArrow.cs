using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * new Vector2(0f,1f));
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Blocker"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player") && (PlayerController.rollActive == false))
        {
            PlayerController.playerHealth -= 25;
            Destroy(gameObject);
        }
    }
}
