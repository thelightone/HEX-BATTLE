using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
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

    public GameObject model;
    //public Mesh mesh;

    //public AnimatorController animator;
    public Race race;


    private void Start()
    {
        moveController = GetComponent<UnitMoveController>();

        health = unitConfig.health;
        maxHealth = unitConfig.health;
        damage = unitConfig.damage;
        armor = unitConfig.armor;
        goRange = unitConfig.goRange;
        missChance = unitConfig.missChance;
        vamp = unitConfig.vamp;
        melee = unitConfig.melee;
        cost = unitConfig.cost;
        numberUnits = unitConfig.numberUnits;
        skills = unitConfig.skills;
        race = unitConfig.race;
        model = Instantiate(unitConfig.model, transform.position- new Vector3 (0,0.95f,0), transform.rotation, gameObject.transform);
        model.transform.SetAsFirstSibling();
    }

    public void AttackMove(HexTile destTile, HexTile target)
    {
        moveController.Move(destTile, target);
    }

    public void DoDamage(HexTile target)
    {
        var enemy = target.unitOn.fightController;
        enemy.Death();
    }

    public void ReceiveDamage()
    {
        Death();
    }

    public void Death()
    {
        gameObject.SetActive(false);
        moveController.currentTile.MakeFree();
    }

}

