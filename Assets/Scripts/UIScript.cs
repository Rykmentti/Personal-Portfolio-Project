using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public static UIScript uiScript;
    public GameObject pressEtoSwitch;
    // Start is called before the first frame update
    void Start()
    {
        uiScript = this;
        pressEtoSwitch = transform.Find("TextPressE").gameObject;
        pressEtoSwitch.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
