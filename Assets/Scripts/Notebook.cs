using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Notebook : MonoBehaviour
{
    public static Notebook instance;
    public GameObject entryContainer;
    private PlayerInput.PlayerActions input;
    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);
        instance = this;
    }

    private List<NotebookEntry> entries;
    [SerializeField]
    private NotebookEntry entryPrefab;

    private void Start()
    {
        input = new PlayerInput().Player;
        input.Enable();
        input.OpenNotebook.performed += OnOpenNotebook;
    }
    public void addEntry(NotebookEntrySO entry)
    {
        NotebookEntry newEntry = Instantiate(entryPrefab, entryContainer.transform);
        newEntry.title.text = entry.title;
        newEntry.shortDescription.text = entry.shortDescription;
        entries.Add(newEntry);
    }

    public void OnOpenNotebook(InputAction.CallbackContext _)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
