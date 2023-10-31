using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public StateMachine stateMachine;
    public string currenStateName;
    // Update is called once per frame
    void Update()
    {
        // Slowing down to every 10 frame
        if (Time.frameCount % 10 != 0)
            return;

        stateMachine.Process();

        currenStateName = stateMachine.GetCurrentStateName();
    }
}
