using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    public GameObject leftSwitch;
    public GameObject rightSwitch;
    public GameObject gate;
    public float switchLeftTargetDistance;
    public float switchRightTargetDistance;
    public bool switchLeft;
    public bool switchRight;
    // Start is called before the first frame update

    void Start()
    {
        leftSwitch = transform.Find("SwitchLeft").gameObject;
        rightSwitch = transform.Find("SwitchRight").gameObject;
        gate = GameObject.Find("GateBlockerTilemap");
    }

    // Update is called once per frame
    void Update()
    {
        switchLeftTargetDistance = Vector2.Distance(PlayerController.playerTransform.position, leftSwitch.transform.position);
        switchRightTargetDistance = Vector2.Distance(PlayerController.playerTransform.position, rightSwitch.transform.position);

        if (switchLeftTargetDistance <= 1.5f)
        {
            UIScript.uiScript.pressEtoSwitch.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                switchLeft = true;
                Debug.Log("switchLeft is = " + switchLeft);
                UIScript.uiScript.pressEtoSwitch.SetActive(false);
                leftSwitch.SetActive(false);
                
            }
        }
        else if (switchRightTargetDistance <= 1.5f)
        {
            UIScript.uiScript.pressEtoSwitch.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                switchRight = true;
                Debug.Log("switchRight is = " + switchRight);
                UIScript.uiScript.pressEtoSwitch.SetActive(false);
                rightSwitch.SetActive(false);

            }
        }
        else if (switchLeft == true && switchRight == true)
        {
            gate.SetActive(false);
            UIScript.uiScript.pressEtoSwitch.SetActive(false);

        }
        else if (switchLeftTargetDistance >= 1.5f || switchRightTargetDistance >= 1.5f)
        {
            UIScript.uiScript.pressEtoSwitch.SetActive(false);
        }
    }
}
