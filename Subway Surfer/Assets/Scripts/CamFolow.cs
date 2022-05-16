using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFolow : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Player;

    public float ZOffset;
    public float YOffset;

    
    // Update is called once per frame
    void Update()
    {
        Camera.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + YOffset, Player.transform.position.z + ZOffset);
    }
}
