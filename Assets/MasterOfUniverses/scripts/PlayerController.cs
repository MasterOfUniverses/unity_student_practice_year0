using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private Animator player_animator;

    [SerializeField]
    private TMP_Text time_text;

    [SerializeField]
    private RectTransform exp_curr_image;

    private int level = 1;
    private float exp_to_next = 100;
    private float exp_current = 0;
    private float hp_max = 200;
    private float hp_current = 200;
    private SkillController skills_player;
    float timer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player_animator = GetComponent<Animator>();
        skills_player = GetComponent<SkillController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        exp_to_next = 100;
        exp_current = 0;
        hp_max = 200;
        hp_current = 200;
        timer = Time.time;
        float curr_screen_length = 1920;
        exp_curr_image.sizeDelta = new Vector2(curr_screen_length * exp_current / exp_to_next, 30);
        exp_curr_image.localPosition = new Vector3(((curr_screen_length * exp_current / exp_to_next) - (curr_screen_length)) / 2, 525, 0);

    }

    // Update is called once per frame
    void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(inputHorizontal, inputVertical,0) * speed;
        player_animator.SetFloat("v_speed", rb.velocity[1]);
        player_animator.SetFloat("h_speed", rb.velocity[0]);
        timer += Time.deltaTime;
        time_text.text = new DateTime(0).AddSeconds((int)timer).ToString("mm:ss");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.name.Contains("exp_"))
        {
            if(other.gameObject.name.Contains("exp_small"))
            {
                exp_current += 20;
            }else if (other.gameObject.name.Contains("exp_medium"))
            {
                exp_current += 50;
            }else if (other.gameObject.name.Contains("exp_large"))
            {
                exp_current += 150;
            }
            else { Debug.Log(other.gameObject.name); }
            while (exp_current > exp_to_next)
            {
                level += 1;
                skills_player.Show_new_skills();
                exp_current -= exp_to_next;
                exp_to_next *= 1.25f;
            }
            float curr_screen_length = 1920;
            exp_curr_image.sizeDelta = new Vector2(curr_screen_length * exp_current / exp_to_next, 30);
            //exp_curr_image.anchoredPosition.Set(0, 525);//((curr_screen_length * exp_current / exp_to_next) - (curr_screen_length)) / 2
            exp_curr_image.localPosition = new Vector3(((curr_screen_length * exp_current / exp_to_next) - (curr_screen_length)) / 2, 525,0);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("enemy"))
        {

        }else if (other.gameObject.name.Contains("hp"))
        {
            hp_current = Mathf.Min(new float[] { hp_current + 50, hp_max});
            Destroy(other.gameObject);
        }
    }
}
