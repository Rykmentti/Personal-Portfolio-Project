using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrganizeInLineFormation : MonoBehaviour
{
    bool inFormation;
    NavMeshAgent navMeshAgent;

    // Update is called once per frame
    void Update()
    {
        if (inFormation == false && navMeshAgent.remainingDistance <= 0.1f)
        {
            inFormation = true;
            GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.IdleAnimationSouth);
        }
    }

    public void TakePlaceInLineFormation(Vector3 destination)
    {
        inFormation = false;
        Debug.Log("Taking place in formation");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(destination);

        float targetAngle = Mathf.Atan2(destination.x - transform.position.x, destination.y - transform.position.y) * Mathf.Rad2Deg;
        if (targetAngle < 0) targetAngle += 360f; // Making sure value is between 0 and 360 degrees, instead of 180 and -180.

        // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
        if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationNorth);
        else if (targetAngle > 45 && targetAngle < 135) GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationEast);
        else if (targetAngle > 135 && targetAngle < 225) GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationSouth);
        else if (targetAngle > 225 && targetAngle < 315) GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationWest);
    }
}
