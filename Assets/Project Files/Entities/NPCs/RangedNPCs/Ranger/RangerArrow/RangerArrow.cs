using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerArrow : MonoBehaviour
{
    [SerializeField] int speed; // Set speed in editor for now.

    int damage;
    string selfIdentifierTag;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        selfIdentifierTag = transform.parent.gameObject.tag; // We are same tag as our creator.
        damage = GetComponentInParent<RangedNPC>().SetDamageForChildren();
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5) Destroy(gameObject); // Clean projectiles, if they miss.

        transform.Translate(speed * Time.deltaTime * new Vector2(0f, 1f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(selfIdentifierTag) && !other.CompareTag("Untagged"))
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
