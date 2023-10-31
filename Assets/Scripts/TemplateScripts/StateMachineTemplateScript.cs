using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineTemplateScript : MonoBehaviour
{
    enum CurrentState  // Here we declare State Machine States as enumerations.
    {
        Idle,
        Hold,
        State1,
        State2,
        State3,
    }

    [SerializeField] CurrentState currentState;

    void SetState(CurrentState state) // Method we use to change states in the state machine.
    {
        currentState = state;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetState(CurrentState.Idle);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 30), currentState + " Statemachine is active!");
        GUI.Label(new Rect(10, 25, 300, 30), "Something");
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CurrentState.Hold: // <-- This is a State Machine State. In this line it's the Hold state. Follow same format for the ones below, if it starts with CurrentState.something, it's state for the state machine.
                HoldBehaviour();
                break;
            case CurrentState.Idle: 
                IdleBehaviour();
                break;
            case CurrentState.State1:
                State1Behaviour();
                break;
            case CurrentState.State2:
                State2Behaviour();
                break;
            case CurrentState.State3:
                State3Behaviour();
                break;
            default: // Default Behaviour. I.e. If statemachine does not recognize the state, it will default to this state that something is trying to put into it. (It's not in enum CurrentState?)
                Debug.Log("This state '" + currentState + "' for state machine doesn't exist, or there is a typo in name(string) of the state, defaulting to Idle State");
                SetState(CurrentState.Idle);
                break;
        }
    }
    // Start of behaviours.
    void HoldBehaviour() // Hold Behaviour, which doesn't contain anything. I use it to temporarily suspend the state machime, while script is executing a method. I.e Do Nothing/State Machine Interupt method, while your method is executing.
    {

    }
    void IdleBehaviour() // Idle Behaviour. This is the behaviour that is on, when the gameObject is passive. I.e, player is not detected/nearby so AI is wandering or something like that. Could also be used as default behaviour.
    {

    }
    // Here you can start creating different kinds of active behaviours. Your imagination is the limit.
    void State1Behaviour() // State1 Behaviour Example. Could be Approaching player behaviour.
    {

    }
    void State2Behaviour() // State2 Behaviour Example. Could be Ranged Attack behaviour.
    {

    }
    void State3Behaviour() // State3 Behaviour Example. Could be Melee Attack behaviour.
    {

    }
    // End of behaviours.
}
