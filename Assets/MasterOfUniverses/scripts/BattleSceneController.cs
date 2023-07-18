using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject plane_prefab;

    [SerializeField]
    private ushort render_radius=2;
    
    private Rigidbody2D rb;
    
    private int render_size;
    private GameObject[,] all_plane;
    private int curr_list_number_x = 0;
    private int curr_list_number_z = 0;
    private Vector3 plane_size;
    private Queue<string> trigger_queue = new Queue<string>();

    private SkillController skillController;
    private int RightMathModule(int a, int b)
    {
        int res = a % b;
        if (res < 0) res = b + res;
        return res;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        render_size = (render_radius * 2 + 1);
        plane_size = plane_prefab.transform.localScale;
        all_plane = new GameObject[render_size, render_size];
        for (int j = -render_radius; j <= render_radius; j++)
        {
            for (int i = -render_radius; i <= render_radius; i++)
            {
                GameObject new_plane_block = Instantiate(plane_prefab);
                new_plane_block.name = $"Plane ({i+ render_radius},{j + render_radius})";
                new_plane_block.transform.position = new Vector3(plane_size[0] * i, plane_size[1] * j, 0);
                all_plane[i+render_radius,j+render_radius] = new_plane_block;
            }
        }
        curr_list_number_x = curr_list_number_z = render_radius;

        skillController = GetComponent<SkillController>();
    }
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        while (trigger_queue.Count > 0)
        {
            string trigger_name = trigger_queue.Dequeue();
            switch (trigger_name)
            { 
                case "trigger_up":
                    if (rb.velocity[1] <= 0) break;
                    //Debug.Log("trigger up");
                    skillController.Show_new_skills();
                    for (int i = -render_radius; i <= render_radius; i++)
                    {
                        int index = RightMathModule(curr_list_number_z - render_radius, render_size);
                        Destroy(all_plane[i + render_radius, index]);
                        all_plane[i + render_radius, index] = Instantiate(plane_prefab);
                        all_plane[i + render_radius, index].name = $"Plane ({i + render_radius},{index})";
                        all_plane[i + render_radius, index].transform.position = all_plane[i + render_radius, curr_list_number_z].transform.position
                            + new Vector3(0, plane_size[1] * (render_radius + 1), 0);
                    }
                    curr_list_number_z = RightMathModule(curr_list_number_z + 1, render_size);
                    break;
                case "trigger_right":
                    if (rb.velocity[0] <= 0) break;
                    //Debug.Log("trigger right");

                    for (int j = -render_radius; j <= render_radius; j++)
                    {
                        int index = RightMathModule(curr_list_number_x - render_radius, render_size);
                        Destroy(all_plane[index, j + render_radius]);
                        all_plane[index, j + render_radius] = Instantiate(plane_prefab);
                        all_plane[index, j + render_radius].name = $"Plane ({index},{j + render_radius})";
                        all_plane[index, j + render_radius].transform.position = all_plane[curr_list_number_x, j + render_radius].transform.position
                            + new Vector3(plane_size[0] * (render_radius + 1), 0, 0);
                    }
                    curr_list_number_x = RightMathModule((curr_list_number_x + 1), render_size);
                    break;
                case "trigger_left":
                    if (rb.velocity[0] >= 0) break;
                    //Debug.Log("trigger left");

                    for (int j = -render_radius; j <= render_radius; j++)
                    {
                        int index = RightMathModule(curr_list_number_x + render_radius, render_size);
                        Destroy(all_plane[index, j + render_radius]);
                        all_plane[index, j + render_radius] = Instantiate(plane_prefab);
                        all_plane[index, j + render_radius].name = $"Plane ({index},{j + render_radius})";
                        all_plane[index, j + render_radius].transform.position = all_plane[curr_list_number_x, j + render_radius].transform.position
                            + new Vector3(plane_size[0] * (-render_radius - 1), 0, 0);
                    }
                    curr_list_number_x = RightMathModule((curr_list_number_x - 1), render_size);
                    break;
                case "trigger_down":
                    if (rb.velocity[1] >= 0) break;
                    //Debug.Log("trigger down");

                    for (int i = -render_radius; i <= render_radius; i++)
                    {
                        int index = RightMathModule(curr_list_number_z + render_radius, render_size);
                        Destroy(all_plane[i + render_radius, index]);
                        all_plane[i + render_radius, index] = Instantiate(plane_prefab);
                        all_plane[i + render_radius, index].name = $"Plane ({i + render_radius},{index})";
                        all_plane[i + render_radius, index].transform.position = all_plane[i + render_radius, curr_list_number_z].transform.position
                            + new Vector3(0, plane_size[1] * (-render_radius - 1), 0);

                    }
                    curr_list_number_z = RightMathModule((curr_list_number_z - 1), render_size);
                    break;
                default:
                    //Debug.Log($"trigger error: trigger name = {trigger_name}");
                    break;

            }
        }
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"enter: {other.gameObject.name}");
        //skillController.Show_new_skills();
        if (other.GetType() == (new BoxCollider2D()).GetType())
        {
            trigger_queue.Enqueue(other.gameObject.name);
            yield return null;
        }
        else
        {
            /*if (other.gameObject.name.Contains("exp_"))
            {

            }*/
        }
        yield return null;
    }
}
