using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CasterNPCProjectile : MonoBehaviour
{
    NavMeshAgent projectileAgent;
    Transform target;
    Vector3 targetDestination;
    string identifierTag;
    int damage;
    int speed;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        projectileAgent = GetComponent<NavMeshAgent>();
        projectileAgent.updateRotation = false; // Can't have this with 2D sprites using NavMesh.
        projectileAgent.updateUpAxis = false; // Can't have this with 2D sprites using NavMesh.
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) Destroy(gameObject);

        timer += Time.deltaTime;
        if (timer > 4) Destroy(gameObject); // Clean projectiles, if they miss.

        if (target != null && targetDestination != target.position) // No need to do calculations again for an object that isn't moving.
        {
            targetDestination = target.position;
            projectileAgent.SetDestination(target.position);
        }
    }

    public void SetDamageForChildren(int parentDamage) { damage = parentDamage; }
    public void SetSpeedForChildren(int parentSpeed) { speed = parentSpeed; }
    public void SetIdentifierTagForChildren(string parentTag) { identifierTag = parentTag; }
    public void SetTargetForChildren(Transform parentTarget) { target = parentTarget; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(identifierTag) && !other.CompareTag("Untagged"))
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
