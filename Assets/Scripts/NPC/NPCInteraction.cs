using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.E;

    [Header("Dialogue")]
    public string[] dialogueLines; 
    private int currentLineIndex = 0;

    [Header("Quest")]
    public Item requiredItem; 
    public int requiredAmount = 3; 
    public Item rewardItem;
    public string questCompletedText = "¡Gracias! Aquí está tu recompensa.";

    private Transform player;
    private bool playerInRange = false;
    private bool questCompleted = false;
    private bool isShowingDialogue = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        playerInRange = distance <= interactionRange;

        if (playerInRange && !isShowingDialogue)
        {
            NPCDialogue.Instance.ShowInteractionPrompt(true);
        }
        else if (!isShowingDialogue)
        {
            NPCDialogue.Instance.ShowInteractionPrompt(false);
        }
        if (playerInRange && Input.GetKeyDown(interactionKey) && !isShowingDialogue)
        {
            Interact();
        }

        if (isShowingDialogue && (Input.GetKeyDown(interactionKey)))
        {
            NextDialogueLine();
        }
    }

    private void Interact()
    {
        if (questCompleted)
        {
            ShowDialogue("Ya te ayudé, ¡buena suerte en tu aventura!");
            return;
        }
        if (HasRequiredItems())
        {
            CompleteQuest();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines.Length == 0) return;

        isShowingDialogue = true;
        currentLineIndex = 0;
        NPCDialogue.Instance.ShowDialogue(dialogueLines[currentLineIndex]);
        NPCDialogue.Instance.ShowInteractionPrompt(false);
    }

    private void NextDialogueLine()
    {
        if (questCompleted)
        {
            EndDialogue();
            return;
        }
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            NPCDialogue.Instance.ShowDialogue(dialogueLines[currentLineIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowDialogue(string text)
    {
        isShowingDialogue = true;
        NPCDialogue.Instance.ShowDialogue(text);
    }

    private void EndDialogue()
    {
        isShowingDialogue = false;
        NPCDialogue.Instance.HideDialogue();
        currentLineIndex = 0;
    }

    private bool HasRequiredItems()
    {
        if (requiredItem == null) return false;

        int totalCount = 0;
        foreach (InventorySlot slot in InventoryManager.Instance.inventorySlots)
        {
            DraggableItem itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if (itemInSlot != null && itemInSlot.item == requiredItem)
            {
                totalCount += itemInSlot.count;
            }
        }

        return totalCount >= requiredAmount;
    }

    private void CompleteQuest()
    {
        RemoveItemsFromInventory(requiredItem, requiredAmount);
        if (rewardItem != null)
        {
            InventoryManager.Instance.AddItem(rewardItem);
        }

        questCompleted = true;
        isShowingDialogue = true;
        NPCDialogue.Instance.ShowDialogue(questCompletedText);
        NPCDialogue.Instance.ShowInteractionPrompt(false);

        Debug.Log("¡Quest completada!");
    }

    private void RemoveItemsFromInventory(Item item, int amount)
    {
        int remainingToRemove = amount;

        foreach (InventorySlot slot in InventoryManager.Instance.inventorySlots)
        {
            if (remainingToRemove <= 0) break;

            DraggableItem itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                if (itemInSlot.count >= remainingToRemove)
                {
                    itemInSlot.count -= remainingToRemove;
                    remainingToRemove = 0;

                    if (itemInSlot.count <= 0)
                    {
                        Destroy(itemInSlot.gameObject);
                    }
                    else
                    {
                        itemInSlot.RefreshCount();
                    }
                }
                else
                {
                    remainingToRemove -= itemInSlot.count;
                    Destroy(itemInSlot.gameObject);
                }
            }
        }
    }
}
