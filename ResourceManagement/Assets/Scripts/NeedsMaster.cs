using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsMaster : MonoBehaviour
{
    public float scanRadius;

    public Pathfinder pathfinder;

    public LayerMask ironMask;
    public LayerMask woodMask;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetResource(LayerMask resource)
    {
        Collider[] colliderTemp = (Physics.OverlapSphere(transform.position, scanRadius, resource));

        if (colliderTemp != null)
        {
            pathfinder.target = colliderTemp[0].transform;
        }
    }
}
