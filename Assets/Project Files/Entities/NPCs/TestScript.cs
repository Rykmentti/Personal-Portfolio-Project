using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10; // Set the z-coordinate to the distance from the camera

            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);

            if (hit.collider != null) Debug.Log("We hit something! " + hit.collider.name);
            if (hit.collider != null && hit.collider.name == "PlayerDeploymentZone")
            {
                Debug.Log("We hit deployment zone!");
            }
        }
    }
}
