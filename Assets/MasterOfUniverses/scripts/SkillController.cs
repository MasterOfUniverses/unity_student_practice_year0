using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillController : MonoBehaviour
{

    protected class Skill
    {
        int level = 0;
        string name = "default_skill";
        float damage = 10;
        float count = 0;
        float cooldown = 10;
        GameObject animator;
        public Skill(string name, GameObject animator, int count, float cooldown,float damage)
        {
            this.damage = damage;
            this.name = name;
            this.animator = animator;
            this.count = count;
            this.cooldown = cooldown;
        }
        public void new_lvl()
        {
            level += 1;
            if (level % 3 == 0)
            {
                count *= 1.3f;
            }
            else
            {
                damage *= 1.4f;
            }
        }
        public int get_lvl()
        {
            return level;
        }
        public string get_name() { return name; }
        public float get_cooldown() { return cooldown; }

        public float get_count() { return count;}

        public GameObject get_animator() { return animator; }

        public float get_damage() { return damage; }

    }
    [SerializeField]
    private Canvas skill_menu;
    [SerializeField]
    private int max_choice = 1;
    [SerializeField]
    private List<GameObject> skills_anim;
    [SerializeField]
    private List<string> skills_name;
    [SerializeField]
    private List<int> skills_count;
    [SerializeField]
    private List<float> skills_cooldown;
    [SerializeField]
    private List<float> skills_damage;

    private List<float> curr_cooldown= new List<float>();
    protected List<Skill> skills=new List<Skill>();
    private List<TMP_Text> skill_buttons;
    private System.Random rnd = new System.Random();
    private List<int> numbers_for_buttons = new List<int>();
    private bool is_max_lvl = false;
    private void Awake()
    {
        int number_of_skills = Mathf.Min(new int[] {skills_anim.Count,skills_cooldown.Count,skills_count.Count,skills_damage.Count,skills_name.Count});
        for (int i = 0; i < number_of_skills; i++)
        {
            Skill new_skill = new Skill(skills_name[i], skills_anim[i], skills_count[i], skills_cooldown[i], skills_damage[i]);
            skills.Add(new_skill);
            curr_cooldown.Add(skills_cooldown[i]);
        }
        skill_menu.enabled = false;
        skill_buttons = new List<TMP_Text>(skill_menu.GetComponentsInChildren<TMP_Text>());
        for(int i=0;i< skill_buttons.Count; i++)
        {
            //Debug.Log(i+"  "+skill_buttons[i].text);
        }
        if(skill_buttons.Count < max_choice)
        {
            Debug.Log("error: new_skill : not enough buttons");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<skills.Count;i++)
        {
            if (skills[i].get_lvl() != 0)
            {
                curr_cooldown[i] -= Time.deltaTime;
                if (curr_cooldown[i] <= 0)
                {
                    curr_cooldown[i] = skills[i].get_cooldown();
                    GameObject[] targets = GameObject.FindGameObjectsWithTag("enemy");
                    for (int j = 0; j < skills[i].get_count() && j<targets.Length; j++)
                    {
                        GameObject this_missile = Instantiate(skills[i].get_animator());
                        this_missile.gameObject.transform.position = gameObject.transform.position;
                        this_missile.GetComponent<missile>().Set_target(targets[rnd.Next(0, targets.Length)], skills[i].get_damage());
                        
                    }
                }
            }
        }
    }

    public void Show_new_skills()
    {
        if (is_max_lvl) return;
        //Debug.Log("Show new skills");
        List<int> vacation_skills=new List<int>();
        numbers_for_buttons.Clear();
        for(int i = 0; i < skill_buttons.Count/2; i++)
        {
            skill_buttons[i * 2].text = "wrong button";
            skill_buttons[i * 2 + 1].text = $"lvl 99";
        }
        for(int i=0;i<skills.Count;i++)
        {
            if (skills[i].get_lvl() != 7)
            {
                vacation_skills.Add(i);
            }
        }
        if (vacation_skills.Count == 0) {
            is_max_lvl = true; 
            return; 
        }
        int skill_number = rnd.Next(0, vacation_skills.Count);
        for (int i = 0; i < skill_buttons.Count && i < vacation_skills.Count; i++)
        {
            skill_number = rnd.Next(0, vacation_skills.Count);
            numbers_for_buttons.Add(vacation_skills[skill_number]);
            vacation_skills.RemoveAt(skill_number);
            if (vacation_skills.Count == 0) {
                break; 
            }
        }
        
        Time.timeScale = 0;
        int max_vacation_number = 0;
        for(max_vacation_number = 0; max_vacation_number < (int)(numbers_for_buttons.Count); max_vacation_number++)
        {
            skill_buttons[max_vacation_number * 2].text = skills[numbers_for_buttons[max_vacation_number]].get_name();
            skill_buttons[max_vacation_number * 2 + 1].text = $"lvl {skills[numbers_for_buttons[max_vacation_number]].get_lvl()+1}";
        }
        for(; max_vacation_number<(int)(skill_buttons.Count/2); max_vacation_number++)
        {
            //Debug.Log("disabled "+max_vacation_number+" "+skill_buttons.Count);
            //skill_buttons[max_vacation_number].SetEnabled(false);
        }
        skill_menu.enabled=true;
    }
    
    public void Add_new_skill_from_button(int button_number)
    {
        if (button_number >= numbers_for_buttons.Count)
        {
            Debug.Log("wrong button");
            return;
        }
        int skill_number = numbers_for_buttons[button_number];
        if (skill_number < skills.Count)
        {
            skills[skill_number].new_lvl();
        }
        skill_menu.enabled=false;
        Time.timeScale = 1;
    }
}
