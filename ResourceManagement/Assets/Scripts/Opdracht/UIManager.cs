using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Player player;

    public Text woodText;
    public Text stoneText;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update () {

        woodText.text = player.woodCount.ToString();
        stoneText.text = player.stoneCount.ToString();

    }
}
