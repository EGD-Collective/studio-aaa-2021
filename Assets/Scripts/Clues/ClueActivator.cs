using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class ClueActivator : MonoBehaviour
    {
        [SerializeField]
        private GameObject BeamGO;
        private Light Beam;
        [SerializeField]
        private float radius;
        [SerializeField]
        private GameObject origin;
        [SerializeField]
        private LayerMask mask;
        [SerializeField]
        private float attackDamage;
        [SerializeField]
        private Animator handAnim;
        // Use this for initialization
        void Start()
        {
            Beam = BeamGO.GetComponentInChildren<Light>();
        }
        void OnActivateBeam()
        {
            BeamGO.SetActive(!BeamGO.activeInHierarchy);
            handAnim.SetBool("Activate amulet", BeamGO.activeInHierarchy);
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (Beam.enabled && Physics.SphereCast(origin.transform.position, radius, origin.transform.forward, out RaycastHit hit, Beam.range, ~mask))
            {
                //Debug.Log(hit.collider.name);
                if (hit.collider.TryGetComponent<BaseClue>(out BaseClue clue))
                {
                    clue.Activate();
                }
                if (hit.collider.TryGetComponent<WeakPoint>(out WeakPoint weakPoint))
                {
                    weakPoint.Hit(attackDamage);
                }
            }
        }
    }
}