using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesMaster : MonoBehaviour {

    public Transform player;
    public bool counting;

    public virtual void DestroyMe()
    {
        Destroy(gameObject, 1);
    }
}
