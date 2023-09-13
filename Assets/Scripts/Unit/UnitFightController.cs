using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using static UnitConfig;
using DissolveExample;
using System.Runtime.InteropServices.WindowsRuntime;

public class UnitFightController : MonoBehaviour
{
    public UnitMoveController moveController;
    public UnitConfig unitConfig;
    public UnitFightController enemy;
    public DissolveChilds effects;

    public delegate void HealthChangedDelegate(float newHealth);
    public event HealthChangedDelegate HealthChanged;

    public float maxHealth;
    private float _health;
    public float health 
    {
        get { return _health; } 
        set 
        { 
            if (value < 0) 
                _health = 0;
            else if (value >= maxHealth) 
                _health = maxHealth;
            else 
                _health = value;
        }
    }
    public float damage;
    public float armor;
    public float goRange;

    public float missAbil;
    public float critChance;
    public float critModif;
    public float vamp;
    public bool melee;

    public int cost;
    public int numberUnits;

    public SkillParent[] skills;

    public GameObject model;
    //public Mesh mesh;

    //public AnimatorController animator;
    public Race race;


    private void Awake()
    {
        moveController = GetComponent<UnitMoveController>();

        maxHealth = unitConfig.health;
        health = maxHealth;
        damage = unitConfig.damage;
        armor = unitConfig.armor;
        goRange = unitConfig.goRange;
        vamp = unitConfig.vamp;
        melee = unitConfig.melee;
        cost = unitConfig.cost;
        numberUnits = unitConfig.numberUnits;
        skills = unitConfig.skills;
        race = unitConfig.race;
        missAbil = unitConfig.missAbil;
        critChance = unitConfig.critChance;
        critModif = unitConfig.critModif;

    model = Instantiate(unitConfig.model, transform.position- new Vector3 (0,0.95f,0), transform.rotation, gameObject.transform);
        model.transform.SetAsFirstSibling();
        effects = GetComponentInChildren<DissolveChilds>();
    }

    public void AttackMove(HexTile destTile, HexTile target)
    {
        moveController.Move(destTile, target);
    }

    public void StartFight(HexTile target)
    {
        moveController.animator.SetTrigger("Attack");
        enemy = target.unitOn.fightController;
        enemy.enemy = this;
    }

    public void Hit()
    {
            enemy.ReceiveDamage(CheckCritDamage());
    }

    public void ReceiveDamage(float hitDamage)
    {
        if (Dodge())
            return;

        hitDamage = Random.Range(0.8f * hitDamage, 1.2f * hitDamage)-(hitDamage*(armor/100));
        var prevHealth = health;
        health -= hitDamage;
        HealthChanged?.Invoke(health);

        enemy.Vampirism(prevHealth-health);

        if (health <= 0)
        {
            StartCoroutine("Death");
        }
    }

    public IEnumerator Death()
    {
        moveController.animator.SetTrigger("Death");
        moveController.currentTile.MakeFree();
        yield return new WaitForSeconds(1.5f);
        effects.ResetAndStart();
        yield return new WaitForSeconds(6);
        gameObject.SetActive(false);
        moveController.currentTile.MakeFree();
    }

    public bool Dodge()
    {
        return Random.Range(0,100) < missAbil ? true: false;
    }

    public float CheckCritDamage()
    {
        return Random.Range(0, 100) < critChance ? damage*critModif/100 : damage;
    }

    public void Vampirism(float hitDamage)
    {
        health += hitDamage * vamp / 100;
    }
}

