using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropDeployNPC : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // Drag and drop Functionality to deploy player NPCs via button elements from the UI via touch and mouse.

    [SerializeField] GameObject deployableNPC_Prefab; // Assign in Editor
    [SerializeField] Sprite previewSprite;
    [SerializeField] int npcDeployValue;

    GameObject previewSpritePrefab;
    bool isDragging = false;
    [SerializeField] bool onPC = false;

    void Start()
    {
        onPC = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // Instantiate the deployable sprite preview when the button is clicked and start Drag & Drop functionality.
        if (Input.touchCount > 0 && onPC == false)
        {
            Touch touch = Input.GetTouch(0);
            CreateDeployablePreviewPrefab();
            isDragging = true;
            Debug.Log("Deploying preview sprite");
        }

        // Testing with mouse on PC.
        if (onPC == true)
        {
            CreateDeployablePreviewPrefab();
            isDragging = true;
            Debug.Log("Button has been clicked"); 
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the deployable preview prefab while dragging
        if (isDragging && Input.touchCount > 0 && onPC == false)
        {
            Touch touch = Input.GetTouch(0);
            previewSpritePrefab.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            Debug.Log("Dragging preview sprite");
        }
        
        // Testing with mouse on PC.
        if (isDragging && onPC == true)
        {
            previewSpritePrefab.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            Debug.Log("Dragging Player NPC image");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Deploy the prefab when the mouse click is released after dragging.
        if (isDragging && Input.touchCount > 0 && onPC == false)
        {
            Touch touch = Input.GetTouch(0);
            isDragging = false;
            Destroy(previewSpritePrefab);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            GameObject npc = Instantiate(deployableNPC_Prefab, worldPosition, Quaternion.identity);
            npc.tag = "Blue";
            Debug.Log("Deploying Player NPC and destroying preview sprite");
        }

        // Testing with mouse on PC.
        if (isDragging && onPC == true)
        {
            isDragging = false;
            Destroy(previewSpritePrefab);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            GameObject npc = Instantiate(deployableNPC_Prefab, worldPosition, Quaternion.identity);
            npc.tag = "Blue";
            Debug.Log("Deploying Player NPC");
        }
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