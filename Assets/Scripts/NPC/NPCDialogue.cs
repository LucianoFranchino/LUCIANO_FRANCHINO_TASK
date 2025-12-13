using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public static NPCDialogue Instance { get; private set; }

    [Header("UI References")]
    public GameObject dialoguePanel; 
    public TextMeshProUGUI dialogueText; 
    public GameObject interactionPrompt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        HideDialogue();
        ShowInteractionPrompt(false);
    }

    public void ShowDialogue(string text)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = text;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowInteractionPrompt(bool show)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(show);
        }
    }
}

