using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : ResourcesMaster {

    public override void DestroyMe()
    {
        if (counting)
        {
            player.GetComponent<Player>().stoneCount += 1;
            base.DestroyMe();
            counting = false;
        }
    }
}
