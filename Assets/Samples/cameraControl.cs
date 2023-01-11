using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    void Update()
    {
        transform.position=new Vector3(player.position.x,player.position.y,-10f);
    }
}
