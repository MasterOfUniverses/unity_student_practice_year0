using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class missile : MonoBehaviour
{
    private GameObject target;
    private float damage;
    private Vector3 target_pos;
    private float alpha;

    private Vector3 start_point;
    [SerializeField] 
    private float speed=50;
    public void Set_target(GameObject target,float income)
    {
        this.target = target;
        damage = income;
    }
    // Start is called before the first frame update
    void Start()
    {
        start_point = gameObject.transform.position;
        target_pos = target.gameObject.transform.position.normalized;
        alpha = Mathf.Acos(target_pos.x);
        if (target_pos.y < 0) alpha += 180;
        alpha = (alpha+360) % 360;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, alpha);
        gameObject.transform.position += target_pos * 2;

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += target_pos * Time.deltaTime * speed;
        if ((gameObject.transform.position - start_point).magnitude > 150)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("enemy_"))
        {
            other.gameObject.GetComponent<enemy>().take_damage(damage);
            Destroy(gameObject);
        }
    }
}
