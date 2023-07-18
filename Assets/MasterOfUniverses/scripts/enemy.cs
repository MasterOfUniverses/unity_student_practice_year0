using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemy : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private List<GameObject> bonus_spheres;

    private Transform this_position;
    private Transform player_position;
    private PlayerController player_controller;
    private bool is_set = false;
    private float speed = 10;
    private float max_hp = 30;
    private float curr_hp = 30;
    private System.Random random = new System.Random();
    private float destroy_len = 150;
    public void Set_target(GameObject target, float dl)
    {
        
        player = target;
        player_controller = player.GetComponent<PlayerController>();
        player_position = player.GetComponent<Transform>();
        destroy_len = dl;
        is_set = true;
    }
    public void take_damage(float income)
    {
        curr_hp -= income;
        if (curr_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        this_position = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_set)
        {
            float length = (this_position.position - player_position.position).magnitude;
            Vector3 delta_vector_speed = speed * Time.deltaTime * (player_position.position - this_position.position) / length;
            if (length > delta_vector_speed.magnitude/10)
            {
                this_position.position += delta_vector_speed;
            }
            if (length > destroy_len)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        //Debug.Log($"{gameObject.name} was destroyed");
        if (curr_hp <= 0)
        {
            //Debug.Log($"{gameObject.name} was killed");
            if (random.Next(0, 100) > 90)
            {
                GameObject bonus = Instantiate(bonus_spheres[random.Next(0, bonus_spheres.Count)]);
                bonus.transform.position = gameObject.GetComponent<Transform>().position;
            }
        }
    }
}
