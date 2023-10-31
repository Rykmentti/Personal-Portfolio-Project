using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    public float speed;
    public float rotation;
    // Start is called before the first frame update
    void Start()
    {
        /*
        float mousePosX = PlayerController.mouseScreenPosition.x;
        float mousePosY = PlayerController.mouseScreenPosition.y;
        float playerPosX = PlayerController.playerTransform.position.x;
        float playerPosY = PlayerController.playerTransform.position.y;

        Vector2 Point_1 = new Vector2(mousePosX, mousePosY);
        Vector2 Point_2 = new Vector2(playerPosX, playerPosY);
        rotation = Mathf.Atan2(Point_2.y - Point_1.y, Point_2.x - Point_1.x) * Mathf.Rad2Deg;
        //Miksi vitussa t‰ss‰ pit‰‰ rotationiin laittaa +90, ett‰ toi kaava toimii, ku PlayerMiekassa sit‰ ei tarvi laittaa. EnemyArrow ihan sama juttu, ku PlayerArrow, mutta siin‰ sen sit taas pit‰‰ olla -90.  wtf? :D
        Vector3 startRotation = new Vector3(0f, 0f, rotation + 90);
        transform.eulerAngles = startRotation;
        */
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(0f, 1f) * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Collider ei yksin‰‰n riit‰ detectioniin, esim Tilemap Collider2D, vaan sill‰ pit‰‰ olla rigidbody, jotta collision voi detectaa sen.
        if (collision.gameObject.CompareTag("Blocker"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyManager>().enemyHealth -= 100;
            Destroy(gameObject);
        }
    }
}
