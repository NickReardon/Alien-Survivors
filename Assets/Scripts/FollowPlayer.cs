using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] private bool trailingCamera = false;

    [SerializeField] [Range(0, 5)]
    private float trailingSpeed = 0.1f;

    [SerializeField] Transform player;
    [SerializeField] private float z_offset = 10f;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(trailingCamera)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, z_offset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, trailingSpeed * Time.deltaTime);
        }   
        else
        {
            transform.position = new Vector3(player.position.x, player.position.y, z_offset);
        }
    }
}
