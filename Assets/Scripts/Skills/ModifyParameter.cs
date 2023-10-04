using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ModifyParameterSkill", menuName = "Skills/ModifyParameter", order = 51)]
public class ModifyParameter : SkillParent
{
    public List<Effect> Changes = new List<Effect> ();

    private float parameter;

    public override void OnActivate()
    {
        foreach (Effect change in Changes)
        {
            if (change.changedParameter == UnitParameter.melee)
            {
                unitAim.melee = !unitAim.melee;
            }

            parameter = 0;

            switch (change.changedParameter)
            {
                case UnitParameter.health:
                    parameter = unitAim.health;
                    Change(change);
                    break;
                case UnitParameter.damageMelee:
                    parameter = unitAim.damageMelee;
                    Change(change);
                    unitAim.damageMelee = parameter;
                    break;
                case UnitParameter.damageRange:
                    parameter = unitAim.damageRange;
                    Change(change);
                    unitAim.damageRange = parameter;
                    break;
                case UnitParameter.armor:
                    parameter = unitAim.armor;
                    Change(change);
                    unitAim.armor = parameter;
                    break;
                case UnitParameter.goRange:
                    parameter = unitAim.goRange;
                    Change(change);
                    unitAim.goRange = parameter;
                    break;
                case UnitParameter.missAbil:
                    parameter = unitAim.missAbil;
                    Change(change);
                    unitAim.missAbil = parameter;
                    break;
                case UnitParameter.critChance:
                    parameter = unitAim.critChance;
                    Change(change);
                    unitAim.critChance = parameter;
                    break;
                case UnitParameter.critModif:
                    parameter = unitAim.critModif;
                    Change(change);
                    unitAim.critModif = parameter;
                    break;
                case UnitParameter.vamp:
                    parameter = unitAim.vamp;
                    Change(change);
                    unitAim.vamp = parameter;
                    break;
                case UnitParameter.cost:
                    parameter = unitAim.cost;
                    Change(change);
                    unitAim.cost = (int)parameter;
                    break;
                default:
                    break;
            }
        }
    }

    private void Change(Effect change)
    {
        parameter += change.IncreaseBy;
        parameter *= change.MultiplyBy;
    }

    public enum UnitParameter
    {
        health,
        damageMelee,
        damageRange,
        armor,
        goRange,
        missAbil,
        critChance,
        critModif,
        vamp,
        cost,
        melee
    }

    [System.Serializable]
    public struct Effect
    {
        public UnitParameter changedParameter ;
        public float IncreaseBy;
        public float MultiplyBy;
    }
}
