using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFellow : MonoBehaviour
{
    private Vector3 offset;
    public Transform player;
 // Start is called before the first frame update
    void Start()
    {
        offset = player.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
         transform.position = Vector3.Lerp(transform.position, player.position - offset,Time.deltaTime*5);
    }
}