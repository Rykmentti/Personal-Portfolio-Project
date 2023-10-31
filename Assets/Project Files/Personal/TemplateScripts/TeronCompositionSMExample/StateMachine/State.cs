using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public virtual State ProcessState()
    {
        // Staying on same state if no overrides
        return this;
    }

    public virtual void StartState()
    {
        // No default fucntionality
    }

    public virtual void StopState()
    {
        // No default fucntionality
    }
}
