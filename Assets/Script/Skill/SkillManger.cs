using System.Collections;
using System.Collections.Generic;
using Script.Skill.BlackHole;
using Script.Skill.Clone;
using Script.Skill.Crystal;
using Script.Skill.Sword;
using UnityEngine;

public class SkillManger : MonoBehaviour
{
    public static SkillManger instance;

    public Dash_Skill Dash {get; private set;}
    public Clone_Skill Clone {get; private set;}
    public Sword_Skill Sword {get; private set;}
    public Blackhole_Skill BlackHole {get; private set;}
    public Crystal_Skill Crystal {get; private set;}


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        Dash = GetComponent<Dash_Skill>();
        Clone = GetComponent<Clone_Skill>();
        Sword = GetComponent<Sword_Skill>();
        BlackHole = GetComponent<Blackhole_Skill>();
        Crystal = GetComponent<Crystal_Skill>();
    }
}
