using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerFlash : MonoBehaviour
{
    [SerializeField]
    private Camera playerView;
    [SerializeField]
    private GameObject enemyObject;
    [SerializeField]
    private LayerMask groundLayer, enemyLayer;
    [SerializeField]
    private Light flashPointLight;
    private Animator flashAnimator;

    //View Variables
    private bool obstructed;
    private bool hitEnemy;
    private Vector3 toEnemy;

    //Flash Variables
    [SerializeField]
    private float flashRange;


    // Start is called before the first frame update
    void Start()
    {
        flashAnimator = flashPointLight.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        obstructed = Physics.Raycast(transform.position, toEnemy, toEnemy.magnitude, groundLayer);
        hitEnemy = Physics.Raycast(transform.position, toEnemy, flashRange, enemyLayer);
    }

    // Update is called once per frame
    void Update()
    {
        toEnemy = enemyObject.transform.position - transform.position;
        Vector3 enemyToCam = playerView.WorldToViewportPoint(enemyObject.transform.position);

        bool leftClick = Input.GetMouseButtonDown(0);

        if (leftClick && InSight(enemyToCam, obstructed) && hitEnemy)
        {
            flashAnimator.Play("New Animation", -1, 0);
            Debug.Log("FLASHED");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, flashRange);
    }

    private bool InSight(Vector3 viewPortPoint, bool obstructed)
    {
        return viewPortPoint.x > 0 &&
            viewPortPoint.x < 1 &&
            viewPortPoint.y > 0 &&
            viewPortPoint.y < 1 &&
            viewPortPoint.z > 0 &&
            !obstructed;
    }
}
