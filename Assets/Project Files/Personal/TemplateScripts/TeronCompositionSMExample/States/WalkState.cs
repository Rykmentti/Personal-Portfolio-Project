using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State
{
    public State otherState;

    public override State ProcessState()
    {
        //Debug.Log(GetType().Name + " Process");

        // 10% chanse to switch state
        if (Random.Range(0, 100) > 90)
            return otherState;

        // Staying in this state
        return this;
    }

    public override void StartState()
    {
        Debug.Log(GetType().Name + " started");
    }

    public override void StopState()
    {
        Debug.Log(GetType().Name + " stopped");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // Most likely you dont want to use update or fixedupdate on states
    //void Update()
    //{
    //}
}
