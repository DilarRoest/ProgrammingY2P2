using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {

    public enum State {Idle, Moving, Scavenging, Building}
    public State state;

    NavMeshAgent navMeshAgent;

    private Transform target;
    private Collider coll;

    public LayerMask stoneMask;
    public LayerMask woodMask;
    private LayerMask targetMask;

    public Vector3 spawn;

    private Collider[] colls;
    private Collider interactObject;
    public GameObject house;
    
    public int stoneCount;
    public int woodCount;

    public bool active = false;
    private bool houseNotBuilt = true;

    public float interactableRange;
    public float searchRadius;
    private float walkDistance;


    // Use this for initialization
    void Start () {

        spawn = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

        if (active)
            state = State.Moving;
        else
            state = State.Idle;
        
        if (stoneCount <= woodCount)
        {
            targetMask = stoneMask;
        }
        else
        {
            targetMask = woodMask;
        }

        Collider[] h = Physics.OverlapSphere(transform.position, interactableRange, targetMask);

        if (h != null)
        {
            foreach (Collider c in h)
            {
                if (target != null)
                {
                    if (c == target.GetComponent<Collider>())
                    {
                        state = State.Scavenging;
                        interactObject = c;
                    }
                }
            }
        }

        if (state == State.Moving)
        {
            MoveIt(targetMask);
            if (target != null)
                if (state != State.Idle)
                    navMeshAgent.SetDestination(target.position);
        }

        if (state == State.Idle)
            navMeshAgent.ResetPath();

        if (state == State.Scavenging)
            Interacting();
        
        if (state == State.Building && houseNotBuilt)
        {
            returnHome();
        }
    }

    void MoveIt(LayerMask targetedMask){

        colls = new Collider[10];

        colls = Physics.OverlapSphere(transform.position, searchRadius, targetedMask);

        if (colls != null)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                if (target == null)
                    target = colls[i].transform;

                if (Vector3.Distance(transform.position, colls[i].transform.position) < Vector3.Distance(transform.position, target.position))
                    target = colls[i].transform;
                
            }
        }
        coll = target.GetComponent<Collider>();
    }

    void Interacting()
    {
        navMeshAgent.ResetPath();

        if (targetMask == stoneMask)
        {
            Stone stone = interactObject.GetComponent<Stone>();
            stone.counting = true;
            stone.DestroyMe();
        }
        if (targetMask == woodMask)
        {
            Wood wood = interactObject.GetComponent<Wood>();
            wood.counting = true;
            wood.DestroyMe();
        }

        state = State.Idle;
    }

    void returnHome()
    {
        navMeshAgent.SetDestination(spawn);
    }
}
