using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DetectorScript2D : MonoBehaviour
{
    // Point of this script is to assign targets, based on needs of NPC.
    // It has a detectorCollider, which detects all game objects inside it's radius. 
    // Anything that isn't same tag as itself and Untagged is then added into enemyTargetList.

    CircleCollider2D detectorCollider;

    [SerializeField] int detectorRadius;

    [SerializeField] string selfIdentifierTag; // Identifier set in editor. Red or Blue for now.
    [SerializeField] List<Transform> enemyTargetList = new List<Transform>(); 
    [SerializeField] Transform target; // Can be ally and an enemy?

    //[SerializeField] Transform allyTargetList;
    //[SerializeField] Transform allyTarget;

    // Update is called once per frame
    void Start()
    {
        // Creating detectorCollider in script for convenience, trivial to create it in Editor and set the trigger and radius there, if we want to do it that way.
        
        if (detectorRadius == 0) detectorRadius = 5; // Default value, if I forget to set it.
        if (detectorCollider == null)
        {
            detectorCollider = gameObject.AddComponent<CircleCollider2D>();
            detectorCollider.radius = detectorRadius;
            detectorCollider.isTrigger = true;
        }
        detectorCollider.enabled = true;

        selfIdentifierTag = transform.parent.gameObject.tag;
    }
    void Update()
    {
        //Testing
        if (Input.GetKeyDown(KeyCode.T))
        {
            target = FindNearestTarget();
            Debug.Log("Nearest Target is: " + target.name + "!");
        }
    }
    public Transform FindNearestTarget() // Find Nearest Target Routine
    {
        if (enemyTargetList.Count == 0) return null;

        Transform target = null; // <-- Script breaks without declaring null? Why?
        List<float> targetDistances = new List<float>();
        float shortestDistance;

        foreach (Transform enemy in enemyTargetList)
        {
            float targetDistance;
            targetDistance = Vector3.Distance(enemy.transform.position, transform.position);
            targetDistances.Add(targetDistance);
        }
        shortestDistance = targetDistances.Min();

        foreach (Transform enemy in enemyTargetList)
        {
            float targetDistance;
            targetDistance = Vector3.Distance(enemy.transform.position, transform.position);
            if (targetDistance == shortestDistance)
            {
                target = enemy.transform;
                break;
            }
        }
        return target;
    }
    // We could disable colliders after we have detected enemies, or detect enemies at certain time intervals, if we want to optimize performance. 
    // No idea how heavy it is to constanly check for triggers.
    // Writing for myself, because I'm on Mobile Game Programming course, as a reminder.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(selfIdentifierTag) && !other.CompareTag("Untagged"))
        {
            enemyTargetList.Add(other.transform);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(selfIdentifierTag) && !other.CompareTag("Untagged"))
        {
            enemyTargetList.Remove(other.transform);
        }
    }
}
