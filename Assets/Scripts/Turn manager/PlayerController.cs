using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    public List <UnitMoveController> units;
    public Player player;

    public enum Player
    {
        left,
        right
    }

    void Start()
    {
        foreach (var unit in units)
        {
            unit.player = this;
        }
    }

    private void ReceiveDamage(float damage, float damageArmor, float damageVamp)
    {

    }

    private void HealthVamp(float netDamage)
    {

    }

    private void Co()
    {

    }

    public void HighLight()
    {
        foreach (var unit in units)
        {
            unit.active.SetActive(true);
            unit.beAim.DislightAim(1);
        }
    }

    public void DisLight()
    {
        foreach (var unit in units)
        {
            unit.active.SetActive(false);
        }
    }

    public void Act()
    {

    }

    public void ActiveUnits()
    {

    }
}
