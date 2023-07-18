using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class objects : MonoBehaviour
{
    private GameObject player;

    private Transform this_position;
    private Transform player_position;
    private PlayerController player_controller;
    private float magnet_length = 30;
    private bool is_set = false;
    private int magnet_speed = 25;
    //private NavMeshAgent nav_agent;
    private float destroy_len = 150;
    public void Set_target(GameObject target, float dl)
    {
        player = target;
        player_controller = player.GetComponent<PlayerController>();
        player_position = player.GetComponent<Transform>();
        magnet_length = player_controller.get_magnet_length();
        destroy_len = dl;
        is_set = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        this_position = GetComponent<Transform>();
        //nav_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_set)
        {
            
            float length = (this_position.position - player_position.position).magnitude;
            if (length < magnet_length)
            {
                this_position.position += magnet_speed * Time.deltaTime * (player_position.position - this_position.position)/length;
            }
            if (length > destroy_len)
            {
                Destroy(gameObject);
            }
            
            /*if ((this_position.position - player_position.position).magnitude < magnet_length)
            {
                nav_agent.destination = player_position.position;
            }*/
        }
    }
}
