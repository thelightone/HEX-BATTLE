using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

[CreateAssetMenu(fileName = "ModifyParameterSkill", menuName = "Skills/ModifyParameter", order = 51)]
public class ModifyParameterSkill : SkillParent
{
    public List<Effect> Changes = new List<Effect>();

    private float parameter;

    public override void OnActivate()
    {
        animator.SetTrigger("Skill");
    }

    public override void OnReactivate()
    {
        if (unitAims.Count == 0 && hexAims.Count > 0)
        {
            unitAims = HexToUnitConverter(hexAims);
        }

        Modification();

        if (aimType == AimType.staticHex || aimType == AimType.staticHexes)
        {
            unitAims.Clear();
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
        public UnitParameter changedParameter;
        public float IncreaseBy;
        public float MultiplyBy;
    }

    public List<UnitFightController> HexToUnitConverter(List<HexTile> hexes)
    { 
        var units = new List<UnitFightController>();
        foreach (var hex in hexes)
        {
            if (hex.busy)
            {
                if ((aimPlayer == AimPlayer.self && hex.unitOn.player == invoker.moveController.player)
                || (aimPlayer == AimPlayer.enemy && hex.unitOn.player != invoker.moveController.player)
                || aimPlayer == AimPlayer.both)
                {
                    units.Add(hex.unitOn.fightController);
                }
            }
        }
        return units;
    }

    public void Modification()
    {
        foreach (var unitAim in unitAims)
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
    }
}
