using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : ResourcesMaster {

    public override void DestroyMe()
    {
        if (counting)
        {
            player.GetComponent<Player>().woodCount += 1;
            base.DestroyMe();
            counting = false;
        }
    }
}
