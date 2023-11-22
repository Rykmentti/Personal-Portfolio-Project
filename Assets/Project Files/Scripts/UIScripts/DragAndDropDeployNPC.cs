using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropDeployNPC : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // Drag and drop Functionality to deploy player NPCs via button elements from the UI via touch and mouse.

    [SerializeField] GameObject deployableNPC_Prefab; // Assign in Editor
    [SerializeField] Sprite previewSprite;

    GameObject previewSpritePrefab;
    bool isDragging = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        // Instantiate the deployable sprite preview when the button is clicked and start Drag & Drop functionality.
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            CreateDeployablePreviewPrefab();
            isDragging = true;
            Debug.Log("Deploying preview sprite");
        }
#endif  // Testing with mouse on PC. Oh this actually still works on tablet, even though I'm not using touch controls because UnityEvents PointEventData is a generic type.
#if UNITY_EDITOR || UNITY_STANDALONE
        CreateDeployablePreviewPrefab();
        isDragging = true;
        Debug.Log("Button has been clicked");
#endif
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the deployable preview prefab while dragging
#if UNITY_ANDROID
        if (isDragging && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            previewSpritePrefab.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            Debug.Log("Dragging preview sprite");
        }
#endif  // Testing with mouse on PC. Oh this actually still works on tablet, even though I'm not using touch controls because UnityEvents PointEventData is a generic type.
#if UNITY_EDITOR || UNITY_STANDALONE
        if (isDragging)
        {
            previewSpritePrefab.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            Debug.Log("Dragging Player NPC image");
        } 
#endif
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Deploy the prefab when the mouse click is released after dragging.
#if UNITY_ANDROID
        if (isDragging && Input.touchCount > 0)
        {
            if (UI_Manager.uiManager.CheckIfEnoughMoneyToDeployNPC(deployableNPC_Prefab.GetComponent<NPC2D>().GetNPCDeployValue()) == false) { Destroy(previewSpritePrefab); return; }
            

            UI_Manager.uiManager.UpdateMoneyLeftText(-deployableNPC_Prefab.GetComponent<NPC2D>().GetNPCDeployValue());
            Touch touch = Input.GetTouch(0);
            isDragging = false;
            Destroy(previewSpritePrefab);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            GameObject npc = Instantiate(deployableNPC_Prefab, worldPosition, Quaternion.identity);
            npc.tag = "Blue";
            Debug.Log("Deploying Player NPC and destroying preview sprite");
        }
#endif  // Testing with mouse on PC. Oh this actually still works on tablet, even though I'm not using touch controls because UnityEvents PointEventData is a generic type.
#if UNITY_EDITOR || UNITY_STANDALONE
        if (isDragging)
        {
            if (UI_Manager.uiManager.CheckIfEnoughMoneyToDeployNPC(deployableNPC_Prefab.GetComponent<NPC2D>().GetNPCDeployValue()) == false) { Destroy(previewSpritePrefab); return; }

            UI_Manager.uiManager.UpdateMoneyLeftText(-deployableNPC_Prefab.GetComponent<NPC2D>().GetNPCDeployValue());
            isDragging = false;
            Destroy(previewSpritePrefab);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            GameObject npc = Instantiate(deployableNPC_Prefab, worldPosition, Quaternion.identity);
            npc.tag = "Blue";
            Debug.Log("Deploying Player NPC");
        } 
#endif
    }
    void CreateDeployablePreviewPrefab()
    {
        previewSpritePrefab = new GameObject("SpritePreviewWhileDragging");
        previewSpritePrefab.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        SpriteRenderer spriteRenderer = previewSpritePrefab.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = previewSprite;
        spriteRenderer.sortingOrder = -998;
        spriteRenderer.transform.localScale = new Vector3(3, 3, 3);
    }
}