using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkBackToSpawnPositionAfterCombat : MonoBehaviour
{
    bool inCombat;
    bool returningToSpawnPosition;
    NavMeshAgent navMeshAgent;
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] float timeCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCombat && WaveManager.waveManager.CheckForWaveStatus() == true) inCombat = true;
        else if (inCombat && WaveManager.waveManager.CheckForWaveStatus() == false) DelayedWalkBack();

        if (!inCombat && returningToSpawnPosition && navMeshAgent.remainingDistance <= 0.1f)
        {
            returningToSpawnPosition = false;
            GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.IdleAnimationEast);
        }
            
    }

    void DelayedWalkBack()
    {
        timeCounter += Time.deltaTime;

        if(timeCounter >= 2)
        {
            Debug.Log("Walking back to spawn position");
            inCombat = false;
            returningToSpawnPosition = true;
            navMeshAgent.stoppingDistance = 0; // If we don't do this, the NPC will stop before reaching the destination, because the attack range is higher than the actual distance to the destination.
            navMeshAgent.SetDestination(spawnPosition);
            timeCounter = 0;

            float targetAngle = Mathf.Atan2(spawnPosition.x - transform.position.x, spawnPosition.y - transform.position.y) * Mathf.Rad2Deg;
            if (targetAngle < 0) targetAngle += 360f; // Making sure value is between 0 and 360 degrees, instead of 180 and -180.

            // Setting Animations, using angle, which then sets correct animation set in Sprite Animator.
            if ((targetAngle > 315 && targetAngle < 360) || (targetAngle > 0 && targetAngle < 45)) GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationNorth);
            else if (targetAngle > 45 && targetAngle < 135) GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationEast);
            else if (targetAngle > 135 && targetAngle < 225) GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationSouth);
            else if (targetAngle > 225 && targetAngle < 315) GetComponent<SimpleSpriteAnimationController>().SetState(SimpleSpriteAnimationController.CurrentState.WalkingAnimationWest);
        }
    }
}
