using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private List<GameObject> enemy_prefabs = new List<GameObject>();
    [SerializeField]
    private List<GameObject> exp_spheres = new List<GameObject>();
    [SerializeField]
    private GameObject hp_prefab;

    [SerializeField]
    private int spawn_radius = 70;
    [SerializeField]
    public int destroy_radius = 250;

    public int max_enemies = 20;
    public int curr_enemies = 0;
    private float timer;
    private int seconds = 0;
    private System.Random rnd = new System.Random();

    private Transform player_position;

    private void Awake()
    {
        player_position = GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time;
        for(int i=0; i< max_enemies; i++)
        {
            GameObject new_enemy = Instantiate(enemy_prefabs[rnd.Next(enemy_prefabs.Count)]);
            float alpha = rnd.Next(360);
            new_enemy.GetComponent<enemy>().Set_target(player, destroy_radius);
            new_enemy.transform.position = player_position.position + new Vector3(Mathf.Sin(alpha),Mathf.Cos(alpha),0)*rnd.Next(spawn_radius,(spawn_radius+destroy_radius)/2);
            curr_enemies++;
        }
        for (int i = 0; i < max_enemies*3; i++)
        {
            GameObject exp_sphere = Instantiate(exp_spheres[rnd.Next(exp_spheres.Count)]);
            exp_sphere.GetComponent<objects>().Set_target(player, destroy_radius);
            float alpha = rnd.Next(360);
            exp_sphere.transform.position = player_position.position + new Vector3(Mathf.Sin(alpha), Mathf.Cos(alpha), 0) * rnd.Next(spawn_radius, (spawn_radius + destroy_radius) / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (curr_enemies < max_enemies)
        {
            float rand_new_enemies = rnd.Next(1,Mathf.Min(20,max_enemies-curr_enemies)); 
            for (int i = 0; i < rand_new_enemies; i++)
            {
                GameObject new_enemy = Instantiate(enemy_prefabs[rnd.Next(enemy_prefabs.Count)]);
                float alpha = rnd.Next(360);
                new_enemy.GetComponent<enemy>().Set_target(player, destroy_radius);
                new_enemy.transform.position = player_position.position + new Vector3(Mathf.Sin(alpha), Mathf.Cos(alpha), 0) * rnd.Next(spawn_radius, (spawn_radius + destroy_radius) / 2);
                curr_enemies++;
            }
        }
        if (timer > 5)
        {
            max_enemies += (int)((5+0.2f*max_enemies)/2);
            timer -= 5; 
            for (int i = 0; i < max_enemies / 20; i++)
            {
                GameObject exp_sphere = Instantiate(exp_spheres[rnd.Next(exp_spheres.Count)]);
                float alpha = rnd.Next(360);
                exp_sphere.GetComponent<objects>().Set_target(player,destroy_radius);
                exp_sphere.transform.position = player_position.position + new Vector3(Mathf.Sin(alpha), Mathf.Cos(alpha), 0) * rnd.Next(spawn_radius, (spawn_radius + destroy_radius) / 2);
            }
            seconds += 5;
        }
        if (seconds > 300)
        {
            GameObject hp_sphere = Instantiate(hp_prefab);
            float alpha = rnd.Next(360);
            hp_sphere.GetComponent<objects>().Set_target(player, destroy_radius);
            hp_sphere.transform.position = player_position.position + new Vector3(Mathf.Sin(alpha), Mathf.Cos(alpha), 0) * rnd.Next(spawn_radius, (spawn_radius + destroy_radius) / 2);
            seconds =0;
        }

    }
}
