using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Notebook : MonoBehaviour
{
    public static Notebook instance;
    public GameObject entryContainer;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip openSound;
    [SerializeField]
    private AudioClip addEntrySound;
    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);
        instance = this;
    }

    private List<NotebookEntry> entries = new List<NotebookEntry>();
    [SerializeField]
    private NotebookEntry entryPrefab;

    private void Start()
    {
        playerInput.actions.FindAction("OpenNotebook").performed += _ => OnOpenNotebook();
        gameObject.SetActive(false);
    }
    public void addEntry(NotebookEntrySO entry)
    {
        NotebookEntry newEntry = Instantiate(entryPrefab, entryContainer.transform);
        newEntry.title.text = entry.title;
        newEntry.shortDescription.text = entry.shortDescription;
        entries.Add(newEntry);
        audioSource.PlayOneShot(addEntrySound);
    }

    public void OnOpenNotebook()
    {
        bool state = !gameObject.activeInHierarchy;
        gameObject.SetActive(state);
        audioSource.PlayOneShot(openSound);
        Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.Locked;
        Time.timeScale = state ? 0 : 1;
    }
}
