using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Player playerScript { get; set; }
    
    private float maxHeight, maxDist;
    bool playerAdded = false;

    public void PlayerAdded()
    {
        //TODO : manage maxHeight and Dist by the resolution and not by arbitrary values;
        maxHeight = playerScript.MaxHeight;
        maxDist = playerScript.MaxDistHorizontal;
        playerAdded = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerAdded)
        {
            transform.position = new Vector3(playerScript.transform.position.x, playerScript.transform.position.y, -10);
        }
    }

    public void RemovePlayer()
    {
        playerScript = null;
        playerAdded = false;
    }
}
