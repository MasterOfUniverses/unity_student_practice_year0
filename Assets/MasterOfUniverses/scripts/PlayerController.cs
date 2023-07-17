using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private Animator player_animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player_animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(inputHorizontal, inputVertical,0) * speed;
        player_animator.SetFloat("v_speed", rb.velocity[1]);
        player_animator.SetFloat("h_speed", rb.velocity[0]);
    }
}
