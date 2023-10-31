using System.Collections.Generic;
using UnityEngine;
namespace DevPlz.CombatText
{
    /*Place this script on any GameObject that you're using as a CombatText follow target and when it is
      destroyed or disabled, it will automatically detach any CombatText instances following it.
      Otherwise, any CombatText instances following it will also immediately be destroyed.*/

    public class CombatTextDetacher : MonoBehaviour
    {
        private bool quitting = false;
        private void OnApplicationQuit() => quitting = true;
        private void OnDisable()
        {
            if (quitting) return;
            List<CombatTextInstance> children = new List<CombatTextInstance>(GetComponentsInChildren<CombatTextInstance>(true));
            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.SetParent(children[i].Parent);
            }
        }
    }
}
