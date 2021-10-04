using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class W2Player : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public Vector3 destinationPoint;

    public W2Inventory inventory;

    public W2Interractable currentInterractable;
    public MouseInteraction mouseInteraction;
    public W2Door currentDoor;

    public float interractDistance = 0.8f;

    private bool canInput = true;

    public Animator playerAnimator;

    [Header("UI Refrences")]
    public GameObject workBenchUI;
    public GameObject inventoryUI;
    public GameObject dialogueUI;
    public TMP_Text dialogueText;
    public GameObject winUI;
    public TMP_Text winFlavorText;
    public GameObject safeUI;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        destinationPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (canInput)
        {
            MovePlayer();
            ItemDistanceCheck();
            DoorDistanceCheck();
        }
    }

    public void SetNewDestination(Vector3 position)
    {
        navAgent.enabled = true;

        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(position, out myNavHit, 100, -1))
        {
            position = myNavHit.position;
        }

        destinationPoint = position;
    }

    void MovePlayer()
    {
        if (navAgent.enabled && destinationPoint != null)
        {
            navAgent.SetDestination(destinationPoint);

            if (Vector3.Distance(transform.position, destinationPoint) > interractDistance + 0.1f)
            {
                playerAnimator.SetBool("isWalking", true);
            }
            else
            {
                playerAnimator.SetBool("isWalking", false);
                destinationPoint = transform.position;
            }
        }
    }

    void ItemDistanceCheck()
    {
        if(currentInterractable != null)
        {
            if(Vector3.Distance(transform.position, currentInterractable.transform.position) < currentInterractable.interactDistance)
            {
                if (currentInterractable.isItem)
                    CollectItem();
                else
                    Interraction();

                playerAnimator.SetBool("isWalking", false);
            }
            else
            {
                playerAnimator.SetBool("isWalking", true);
            }
        }
    }

    void DoorDistanceCheck()
    {
        if (currentDoor != null)
        {
            if (Vector3.Distance(transform.position, currentDoor.transform.position) < interractDistance)
            {
                UseDoor();

                playerAnimator.SetBool("isWalking", false);
            }
            else
            {
                playerAnimator.SetBool("isWalking", true);
            }
        }
    }

    void UseDoor()
    {
        if(currentDoor != null)
        {
            currentDoor.UseDoor();

            currentDoor = null;
        }
    }

    void CollectItem()
    {
        if (currentInterractable != null)
        {
            inventory.PickUpItem(currentInterractable);

            currentInterractable = null;
        }
    }

    void Interraction()
    {
        if(currentInterractable != null)
        {
            switch (currentInterractable.interractableType.ToString())
            {
                case "frontDoor":
                    if(inventory.SearchForItem("key") != null)
                    {
                        Win();
                    }
                    else
                    {
                        DisplayDialogueUI(0);
                    }
                    break;
                case "window":
                    if (inventory.SearchForItem("axe") != null)
                    {
                        Win();
                    }
                    else
                    {
                        DisplayDialogueUI(0);
                    }
                    break;
                case "highWindow":
                    if (inventory.SearchForItem("ladder") != null)
                    {
                        Win();
                    }
                    else
                    {
                        DisplayDialogueUI(0);
                    }
                    break;
                case "workBench":
                    inventory.CloseItemPickUpDisplay();
                    currentInterractable = null;
                    workBenchUI.SetActive(true);
                    PlayerInterractable(false);
                    break;
                case "barricadedWindow":
                    DisplayDialogueUI(0);
                    break;
                case "safe":
                    inventory.CloseItemPickUpDisplay();
                    currentInterractable = null;
                    safeUI.SetActive(true);
                    PlayerInterractable(false);
                    break;
            }
        }
    }

    public void DisplayDialogueUI(int text)
    {
        if(currentInterractable != null)
        {
            dialogueText.text = currentInterractable.dialogueTexts[text];
            dialogueText.enabled = true;

            dialogueUI.SetActive(true);

            StopCoroutine("HideDialogueUI");
            StartCoroutine("HideDialogueUI");
        }
    }

    IEnumerator HideDialogueUI()
    {
        yield return new WaitForSeconds(7);

        CloseDilogueUI();
    }

    public void CloseDilogueUI()
    {
        dialogueUI.SetActive(false);

        dialogueText.text = "";
        dialogueText.enabled = false;
    }

    public void ShowInventoryUI()
    {
        inventory.CloseItemPickUpDisplay();
        inventoryUI.SetActive(true);
        PlayerInterractable(false);
    }

    public void HideInventoryUI()
    {
        inventoryUI.SetActive(false);
        PlayerInterractable(true);
    }

    public void HideWorkBenchUI()
    {
        workBenchUI.SetActive(false);
        PlayerInterractable(true);
    }
    public void HideSafeUI()
    {
        safeUI.SetActive(false);
        PlayerInterractable(true);
    }

    public void PlayerInterractable(bool value)
    {
        canInput = value;
        mouseInteraction.caninterract = value;
    }

    void Win()
    {
        winFlavorText.text = currentInterractable.dialogueTexts[1];
        winUI.SetActive(true);
        PlayerInterractable(false);
    }
}
