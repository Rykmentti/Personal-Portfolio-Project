using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerArrow : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] int damage;
    float timer;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5) Destroy(gameObject); // Clean projectiles, if they miss.

        transform.Translate(speed * Time.deltaTime * new Vector2(0f, 1f));
    }

    public void SetDamageForChildren(int parentDamage) { damage = parentDamage; }
    public void SetSpeedForChildren(int parentSpeed) { speed = parentSpeed; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(gameObject.tag) && !other.CompareTag("Untagged"))
        {
            other.GetComponent<NPC2D>().ReceiveDamage(damage);
            Debug.Log(gameObject.name + " dealt Damage to " + other.name + "!");
            Destroy(gameObject);
        }
        //if (other.gameObject.CompareTag("Blocker"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
