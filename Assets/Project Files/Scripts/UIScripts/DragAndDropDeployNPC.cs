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
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        CreateDeployablePreviewPrefab();
        isDragging = true;
        Debug.Log("Deploying preview sprite");
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
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (isDragging)
        {
            previewSpritePrefab.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            Debug.Log("Dragging preview sprite");
        } 
#endif
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Deploy the prefab when the mouse click is released after dragging.
#if UNITY_ANDROID
        if (isDragging && Input.touchCount > 0)
        {
            if (UI_ManagerBattleScene.uiManagerBattleScene.CheckIfEnoughMoneyToDeployNPC(deployableNPC_Prefab.GetComponent<NPC2D>().GetNPCDeployValue()) == false) { Destroy(previewSpritePrefab); return; }
            
            Touch touch = Input.GetTouch(0);
            isDragging = false;
            Destroy(previewSpritePrefab);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            if (CheckForPlayerDeploymentZone(worldPosition) == false)
            {
                //Debug.Log("Cannot deploy NPC here, not in player deployment zone");
                return;
            }
            UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(-deployableNPC_Prefab.GetComponent<NPC2D>().GetNPCDeployValue());
            GameObject npc = Instantiate(deployableNPC_Prefab, worldPosition, Quaternion.identity);
            npc.GetComponent<NPC2D>().SetState(NPC2D.CurrentState.SuspendStateMachine);
            WaveManager.waveManager.AddPlayerToList(npc);
            npc.tag = "Blue";
            Debug.Log("Deploying Player NPC and destroying preview sprite");
        }
#endif  // Testing with mouse on PC. Oh this actually still works on tablet, even though I'm not using touch controls because UnityEvents PointEventData is a generic type.
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (isDragging)
        {
            if (UI_ManagerBattleScene.uiManagerBattleScene.CheckIfEnoughMoneyToDeployNPC(deployableNPC_Prefab.GetComponent<NPC2D>().GetNPCDeployValue()) == false) { Destroy(previewSpritePrefab); return; }

            isDragging = false;
            Destroy(previewSpritePrefab);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            if (CheckForPlayerDeploymentZone(worldPosition) == false)
            {
                //Debug.Log("Cannot deploy NPC here, not in player deployment zone");
                return;
            }
            UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(-deployableNPC_Prefab.GetComponent<NPC2D>().GetNPCDeployValue());
            GameObject npc = Instantiate(deployableNPC_Prefab, worldPosition, Quaternion.identity);
            npc.GetComponent<NPC2D>().SetState(NPC2D.CurrentState.SuspendStateMachine);
            WaveManager.waveManager.AddPlayerToList(npc);
            npc.tag = "Blue";
            Debug.Log("Deploying Player NPC and destroying preview sprite");
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

    bool CheckForPlayerDeploymentZone(Vector3 position)
    {
        bool isWithinDeploymentZone = false;

        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null && hit.collider.name == "PlayerDeploymentZone")
        {
            Debug.Log("NPC is within deployment zone");
            isWithinDeploymentZone = true;
        }
        else
        {
            Debug.Log("NPC is not within deployment zone");
            isWithinDeploymentZone = false;
        }
        return isWithinDeploymentZone;
    }
}