using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnitConfig;

public class UnitFightController : MonoBehaviour
{
    public UnitMoveController moveController;

    public UnitConfig unitConfig;

    public int maxHealth;
    public int health;
    public int damage;
    public int armor;
    public int goRange;

    public int missChance;
    public int vamp;
    public bool melee;

    public int cost;
    public int numberUnits;

    public SkillParent[] skills;
    public Mesh mesh;

    public Animator animator;
    public Race race;


    private void Start()
    {
        moveController = GetComponent<UnitMoveController>();

        health = unitConfig.health;
        maxHealth = unitConfig.health;
        damage =unitConfig.damage;
        armor = unitConfig.armor;
        goRange = unitConfig.goRange;
        missChance = unitConfig.missChance;
        vamp =unitConfig.vamp;
        melee=unitConfig.melee;
        cost= unitConfig.cost;
        numberUnits= unitConfig.numberUnits;
        skills = unitConfig.skills;
        mesh = unitConfig.mesh;
        animator = unitConfig.animator;
        race = unitConfig.race;

        gameObject.transform.GetChild(0).GetComponent<MeshFilter>().mesh = mesh;
    }

    
}

