using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaTransition : MonoBehaviour
{
    private string northTransitionScenePath;
    private string eastTransitionScenePath;
    private string southTransitionScenePath;
    private string westTransitionScenePath;
    private string currentSceneName;
    private string transformName;
    /*
    void OnGUI()
    {
        GUI.Label(new Rect(10, 60, 250, 30), "Player Arriving from north is " + PlayerController.playerArrivingFromNorth);
        GUI.Label(new Rect(10, 80, 250, 30), "Player Arriving from east is " + PlayerController.playerArrivingFromEast);
        GUI.Label(new Rect(10, 100, 250, 30), "Player Arriving from south is " + PlayerController.playerArrivingFromSouth);
        GUI.Label(new Rect(10, 120, 250, 30), "Player Arriving from west is " + PlayerController.playerArrivingFromWest);
    }
    */
    // Start is called before the first frame update
    void Start()
    {
        transformName = transform.name;

        if (transformName == "SouthTransition" && PlayerController.playerArrivingFromSouth == true)
        {
            PlayerController.playerTransform.position = new Vector2(transform.position.x, transform.position.y + 5);
        }
        if (transformName == "WestTransition" && PlayerController.playerArrivingFromWest == true)
        {
            PlayerController.playerTransform.position = new Vector2(transform.position.x + 5, transform.position.y);
        }
        if (transformName == "NorthTransition" && PlayerController.playerArrivingFromNorth == true)
        {
            PlayerController.playerTransform.position = new Vector2(transform.position.x, transform.position.y - 5);
        }
        if (transformName == "EastTransition" && PlayerController.playerArrivingFromEast == true)
        {
            PlayerController.playerTransform.position = new Vector2(transform.position.x - 5, transform.position.y);
        }
        /*
         PlayerController.playerArrivingFromNorth = false;
         PlayerController.playerArrivingFromEast = false;
         PlayerController.playerArrivingFromSouth = false;
         PlayerController.playerArrivingFromWest = false;
         
         * miksi tämä koodin pätkä rikkoo ylhäällä olevan koodin? Ku on sen alla? Eikö koodin pitäis lukea ylempi ensin ja sen jälkeen,
         * laittaa kaikki boolit false? Miten vitussa alaosa laittaa falsen, ennen ku yläosa on lukennut true arvon? Purkkaratkasu onTriggerEnter:issä sitten.
        */
        currentSceneName = SceneManager.GetActiveScene().name;
        //Debug.Log("Scene name is = " + currentSceneName);
        /*
         
         * AreaTransition for Scenes Template. 
         * Put the scene name, that you want the transitions on, after currentSceneName within "", then put target scene names on the transition scene paths before .unity.
         
        if (currentSceneName == "")
        {
            northTransitionScenePath = "Assets/Scenes/.unity";
            eastTransitionScenePath = "Assets/Scenes/.unity";
            southTransitionScenePath ="Assets/Scenes/.unity";
            westTransitionScenePath = "Assets/Scenes/.unity";
        }
        */
        if (currentSceneName == "PlayerStartScene")
        {
            northTransitionScenePath = "Assets/Scenes/NorthOfPlayerStartScene.unity";
            eastTransitionScenePath = "Assets/Scenes/EastOfPlayerStartScene.unity";
            southTransitionScenePath = null;
            westTransitionScenePath = null;
        }
        if (currentSceneName == "NorthOfPlayerStartScene")
        {
            northTransitionScenePath = null;
            eastTransitionScenePath = "Assets/Scenes/CaveEntranceScene.unity";
            southTransitionScenePath = "Assets/Scenes/PlayerStartScene.unity";
            westTransitionScenePath = null;
        }
        if (currentSceneName == "EastOfPlayerStartScene")
        {
            northTransitionScenePath = "Assets/Scenes/MountainsScene.unity";
            eastTransitionScenePath = null;
            southTransitionScenePath = null;
            westTransitionScenePath = "Assets/Scenes/PlayerStartScene.unity";
        }
        if (currentSceneName == "MountainsScene")
        {
            northTransitionScenePath = null;
            eastTransitionScenePath = null;
            southTransitionScenePath = "Assets/Scenes/EastOfPlayerStartScene.unity";
            westTransitionScenePath = "Assets/Scenes/CaveEntranceScene.unity";
        }
        if (currentSceneName == "CaveEntranceScene")
        {
            northTransitionScenePath = "Assets/Scenes/CavesScene.unity";
            eastTransitionScenePath = "Assets/Scenes/MountainsScene.unity";
            southTransitionScenePath = null;
            westTransitionScenePath = "Assets/Scenes/NorthOfPlayerStartScene.unity";
        }
        if (currentSceneName == "CavesScene")
        {
            northTransitionScenePath = null;
            eastTransitionScenePath = null;
            southTransitionScenePath = "Assets/Scenes/CaveEntranceScene.unity";
            westTransitionScenePath = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (transform.name == "NorthTransition")
            {
                PlayerController.playerArrivingFromNorth = false;
                PlayerController.playerArrivingFromEast = false;
                PlayerController.playerArrivingFromSouth = false;
                PlayerController.playerArrivingFromWest = false;

                PlayerController.playerArrivingFromSouth = true;
                Debug.Log("Target Scene Path is " + northTransitionScenePath);
                SceneManager.LoadScene(northTransitionScenePath); ;
            }
            else if (transform.name == "EastTransition")
            {
                PlayerController.playerArrivingFromNorth = false;
                PlayerController.playerArrivingFromEast = false;
                PlayerController.playerArrivingFromSouth = false;
                PlayerController.playerArrivingFromWest = false;

                PlayerController.playerArrivingFromWest = true;
                Debug.Log("Target Scene Path is " + eastTransitionScenePath);
                SceneManager.LoadScene(eastTransitionScenePath); ;
            }
            else if (transform.name == "SouthTransition")
            {
                PlayerController.playerArrivingFromNorth = false;
                PlayerController.playerArrivingFromEast = false;
                PlayerController.playerArrivingFromSouth = false;
                PlayerController.playerArrivingFromWest = false;

                PlayerController.playerArrivingFromNorth = true;
                Debug.Log("Target Scene Path is " + southTransitionScenePath);
                SceneManager.LoadScene(southTransitionScenePath); ;
            }
            else if (transform.name == "WestTransition")
            {
                PlayerController.playerArrivingFromNorth = false;
                PlayerController.playerArrivingFromEast = false;
                PlayerController.playerArrivingFromSouth = false;
                PlayerController.playerArrivingFromWest = false;

                PlayerController.playerArrivingFromEast = true;
                Debug.Log("Target Scene Path is " + westTransitionScenePath);
                SceneManager.LoadScene(westTransitionScenePath); ;
            }
        }
    }
}
