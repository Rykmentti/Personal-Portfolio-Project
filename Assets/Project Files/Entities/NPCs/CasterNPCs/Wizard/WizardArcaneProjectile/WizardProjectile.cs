using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardProjectile : MonoBehaviour
{
    NavMeshAgent projectileAgent;
    CasterNPC parentScript;
    Transform target;
    Vector3 targetDestination;
    int damage;
    string selfIdentifierTag;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        projectileAgent = GetComponent<NavMeshAgent>();
        projectileAgent.updateRotation = false; // Can't have this with 2D sprites using NavMesh.
        projectileAgent.updateUpAxis = false; // Can't have this with 2D sprites using NavMesh.

        damage = GetComponentInParent<CasterNPC>().SetDamageForChildren();
        parentScript = GetComponentInParent<CasterNPC>();
        target = parentScript.SetTargetForChildren();
        selfIdentifierTag = transform.parent.gameObject.tag; // We are use tag as our creator, to differentiate enemies.
        transform.parent = null; // We don't want the projectile flying on the same localPosition "plane" as the transform.
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) target = parentScript.SetTargetForChildren();
        else if (target == null) Destroy(gameObject);

        timer += Time.deltaTime;
        if (timer > 4) Destroy(gameObject); // Clean projectiles, if they miss.

        if (target != null && targetDestination != target.position) // No need to do calculations again for an object that isn't moving.
        {
            targetDestination = target.position;
            projectileAgent.SetDestination(target.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(selfIdentifierTag) && !other.CompareTag("Untagged"))
        {
            other.GetComponent<NPC2D>().ReceiveDamage(damage);
            Debug.Log(gameObject.name + " dealt Damage to " + other.name + "!");
            //Destroy(gameObject);
        }
        //if (other.gameObject.CompareTag("Blocker"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
