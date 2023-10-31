using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMiekka : MonoBehaviour
{
    public Vector3 startRotation;
    public Vector2 localPosition;

    void Start()
    {
        StartCoroutine(SelfDestroy());
        localPosition = GetComponentInParent<PlayerController>().blockLocalPosition;
        startRotation = GetComponentInParent<PlayerController>().blockStartRotation;
        transform.localPosition = localPosition;
        transform.eulerAngles = startRotation;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MeleeWeapon"))
        {
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            Destroy(collision.gameObject);
        }
    }
    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}