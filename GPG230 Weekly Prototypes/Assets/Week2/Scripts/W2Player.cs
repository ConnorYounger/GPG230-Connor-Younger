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
    public GameObject toolHud;
    public GameObject workBenchUI;
    public GameObject inventoryUI;
    public GameObject dialogueUI;
    public TMP_Text dialogueText;
    public GameObject winUI;
    public TMP_Text winFlavorText;
    public GameObject safeUI;
    public GameObject mapUI;
    public W2Map mapManager;
    public Animator darknessFade;

    [Header("Footstep Sounds")]
    public RoomManager roomManager;
    public AudioClip[] woodSteps;
    public AudioClip[] stoneSteps;
    public AudioSource audioSource;
    public float stepTime = 0.5f;
    private bool isPlayingSound;

    [Header("Inventory Audio")]
    public AudioSource bagOpenAudioSource;

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

        NavMeshPath navMeshPath = new NavMeshPath();
        if (navAgent.CalculatePath(position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            destinationPoint = position;
        }
        else
        {
            navAgent.enabled = false;
        }
    }

    void MovePlayer()
    {
        if (navAgent.enabled && destinationPoint != null)
        {
            navAgent.SetDestination(destinationPoint);

            if (Vector3.Distance(transform.position, destinationPoint) > interractDistance + 0.1f)
            {
                playerAnimator.SetBool("isWalking", true);
                PlayStepSound(true);
            }
            else
            {
                playerAnimator.SetBool("isWalking", false);
                PlayStepSound(false);
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
                PlayStepSound(false);
            }
            else
            {
                playerAnimator.SetBool("isWalking", true);

                PlayStepSound(true);
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
                PlayStepSound(false);
            }
            else
            {
                playerAnimator.SetBool("isWalking", true);
                PlayStepSound(true);
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

            HideSafeUI();
            HideWorkBenchUI();
            mapUI.SetActive(false);
            HideInventoryUI();

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
        HideSafeUI();
        HideWorkBenchUI();
        HideDialogueUI();
        mapUI.SetActive(false);

        mapUI.SetActive(false);
        inventoryUI.SetActive(true);
        bagOpenAudioSource.Play();
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

    public void ShowMapUI()
    {
        if (!mapUI.active)
        {
            inventory.CloseItemPickUpDisplay();
            HideSafeUI();
            HideWorkBenchUI();
            HideDialogueUI();
            HideInventoryUI();
            mapUI.SetActive(true);
            mapManager.OpenMap();
        }
        else
            HideMapUI();
    }

    public void HideMapUI()
    {
        mapUI.SetActive(false);
        mapManager.CloseMap();
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
        toolHud.SetActive(false);
        darknessFade.SetBool("fadeOut", true);

        StartCoroutine("WinningAnimation");
    }

    IEnumerator WinningAnimation()
    {
        yield return new WaitForSeconds(3);

        Time.timeScale = 0;
    }

    void PlayStepSound(bool value)
    {
        if(value && !isPlayingSound)
        {
            isPlayingSound = true;

            StartCoroutine("PlayFootStepSound");
        }

        if (!value)
        {
            StopCoroutine("PlayFootStepSound");

            isPlayingSound = false;
        }
    }

    IEnumerator PlayFootStepSound()
    {
        yield return new WaitForSeconds(stepTime / 2);

        if (roomManager.currentRoomIndex == 12)
        {
            int rand = Random.Range(0, stoneSteps.Length);

            audioSource.clip = stoneSteps[rand];
        }
        else
        {
            int rand = Random.Range(0, woodSteps.Length);

            audioSource.clip = woodSteps[rand];
        }

        audioSource.Play();

        yield return new WaitForSeconds(stepTime / 2);

        StartCoroutine("PlayFootStepSound");
    }
}
