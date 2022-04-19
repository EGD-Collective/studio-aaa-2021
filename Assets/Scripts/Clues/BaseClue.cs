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
<<<<<<< HEAD:Assets/Scripts/Clues/BaseClue.cs
=======
        [SerializeField]
        private NotebookEntrySO clueNote;

>>>>>>> feature/EX-103/Note-book:Assets/Scripts/BaseClue.cs
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
