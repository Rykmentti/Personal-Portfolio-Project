using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCirclingSword : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private float speed;
    private float swingSpeed;
    private Transform parentTransform;
    private bool inPhase2;
    private bool inPhase3;
    // Start is called before the first frame update

    //Opettele listojen teko ja hyödynnä niitä tässä
    void Start()
    {
        if (GetComponentInParent<BossEnemyController>().bossPhase3 == true)
        {
            inPhase3 = true;
        }
        else
        {
            inPhase2 = true;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        speed = 10f;
        swingSpeed = 360f;
        parentTransform = transform.parent;
        transform.position = new Vector3(parentTransform.position.x, parentTransform.position.y + 1.5f,parentTransform.position.z );
    }

    // Update is called once per frame
    void Update()
    {
        if (inPhase3 == false && inPhase2 == true && GetComponentInParent<BossEnemyController>().bossPhase3 == true)
        {
            Destroy(gameObject);
        }
        transform.Translate(speed * Time.deltaTime * Vector2.right);
        transform.Rotate(swingSpeed * Time.deltaTime * Vector3.back);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (PlayerController.rollActive == false))
        {
            PlayerController.playerHealth -= 25;
        }

        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Destroy(collision.gameObject);
            spriteRenderer.enabled = false;
            boxCollider.enabled = false;
            StartCoroutine(SwordDisabled());
        }
    }
    IEnumerator SwordDisabled()
    {
        yield return new WaitForSeconds(5);
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }
}