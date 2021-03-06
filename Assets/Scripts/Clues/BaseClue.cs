using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Assets.Scripts
{
    public class BaseClue : MonoBehaviour
    {
        public UnityEvent OnClueActivated;
        public bool Activated;
        public bool SetActiveOnStageStart;
        [SerializeField]
        private NotebookEntrySO clueNote;

        public virtual void Activate()
        {
            if (!Activated)
            {
                Activated = true;
                if (clueNote)
                    Notebook.instance.addEntry(clueNote);
                OnClueActivated.Invoke();
            }
        }
    }
}
