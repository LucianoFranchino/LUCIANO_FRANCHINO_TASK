using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPanel : MonoBehaviour
{
    public static VictoryPanel Instance { get; private set; }

    [Header("UI")]
    public GameObject victoryPanel;

    [Header("Settings")]
    public float delayBeforeRestart = 3f;

    [SerializeField] AudioClip winSound;
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

        victoryPanel.SetActive(false);
    }

    public void ShowVictory()
    {
        StartCoroutine(VictorySequence());
    }

    private IEnumerator VictorySequence()
    {
        victoryPanel.SetActive(true);
        AudioManager.instance.PlayAudio(winSound);

        yield return new WaitForSeconds(delayBeforeRestart);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
