using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject plane_prefab;

    [SerializeField]
    private ushort render_radius=2;

    private ushort render_size;
    private List<GameObject> all_plane = new List<GameObject>();
    private ushort curr_list_number_x = 0;
    private ushort curr_list_number_z = 0;
    void Awake()
    {
        render_size = (ushort)(render_radius * 2 + 1);
        for (int j = -render_radius; j <= render_radius; j++)
        {
            for (int i = -render_radius; i <= render_radius; i++)
            {
                GameObject new_plane_block = Instantiate(plane_prefab);
                new_plane_block.name = $"Plane ({i},{j})";
                new_plane_block.transform.position = new Vector3(plane_prefab.transform.localScale[0] * i, 0, plane_prefab.transform.localScale[2] * j);
                all_plane.Add(new_plane_block);
            }
        }
        curr_list_number_x = curr_list_number_z = render_radius;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        switch():;
            case :;
            default:;
        */
    
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "trigger_up":
                
                for(int j=0;j< render_size; j++)
                {
                    int index = ((curr_list_number_z - render_radius) % render_size)*render_size + j;
                    all_plane[index] = Instantiate(plane_prefab);
                    all_plane[index].name = $"Plane ({index-j},{j})";
                    all_plane[index].transform.position = 
                        all_plane[curr_list_number_x * render_size + curr_list_number_z].transform.position 
                        + new Vector3(plane_prefab.transform.localScale[0] * (render_radius+1), 0, plane_prefab.transform.localScale[2] * j);
                }
                curr_list_number_z = (ushort)((curr_list_number_z + 1) % render_size);
                break;
            case "trigger_rigth":
                for (int i = 0; i < render_size; i++)
                {
                    int index = i * render_size + (curr_list_number_x-render_radius)%render_size;
                    all_plane[index] = Instantiate(plane_prefab);
                    all_plane[index].name = $"Plane ({i},{index-i*render_size})";
                    all_plane[index].transform.position =
                        all_plane[curr_list_number_x * render_size + curr_list_number_z].transform.position
                        + new Vector3(plane_prefab.transform.localScale[0] * i, 0, plane_prefab.transform.localScale[2] * (render_radius+1));
                }
                curr_list_number_x = (ushort)((curr_list_number_x + 1) % render_size);
                break;
            case "trigger_left":
                for (int i = 0; i < render_size; i++)
                {
                    int index = i * render_size + (curr_list_number_x + render_radius) % render_size;
                    all_plane[index] = Instantiate(plane_prefab);
                    all_plane[index].name = $"Plane ({i},{index - i * render_size})";
                    all_plane[index].transform.position =
                        all_plane[curr_list_number_x * render_size + curr_list_number_z].transform.position
                        + new Vector3(plane_prefab.transform.localScale[0] * i, 0, plane_prefab.transform.localScale[2] * (-render_radius - 1));
                }
                curr_list_number_x = (ushort)((curr_list_number_x - 1) % render_size);
                break;
            case "trigger_down":
                for (int j = 0; j < render_size; j++)
                {
                    int index = ((curr_list_number_z + render_radius) % render_size) * render_size + j;
                    all_plane[index] = Instantiate(plane_prefab);
                    all_plane[index].name = $"Plane ({index - j},{j})";
                    all_plane[index].transform.position =
                        all_plane[curr_list_number_x * render_size + curr_list_number_z].transform.position
                        + new Vector3(plane_prefab.transform.localScale[0] * (-render_radius - 1), 0, plane_prefab.transform.localScale[2] * j);
                }
                curr_list_number_z = (ushort)((curr_list_number_z - 1) % render_size);
                break;
            default: {
                    Debug.Log("trigger error");
                    break; 
                }

        }
    }
}
