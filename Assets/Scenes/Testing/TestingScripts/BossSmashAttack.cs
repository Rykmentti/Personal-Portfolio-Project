using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmashAttack : MonoBehaviour
{
    float damage;
    float timeUntilDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        damage = 25f;
        timeUntilDestroyed = 1f;
        Invoke(nameof(DestroySelf), timeUntilDestroyed);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (PlayerController.rollActive == false))
        {
            PlayerController.PlayerReceiveDamage(damage);
        }
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void SpriteFrameByFrameAnimation()
    {

    }
}
