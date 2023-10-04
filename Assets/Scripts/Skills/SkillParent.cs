using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParent : ScriptableObject
{
    public Sprite sprite;
    public string skillName;
    public int duration;
    public UnitFightController invoker;
    public List<UnitFightController> unitAims;
    public List<HexTile> hexAims;

    public AimType aimType = new AimType();
    public AimPlayer aimPlayer = new AimPlayer();
    public DurationType durationType = new DurationType();

    public void Init(UnitFightController skillInvoker)
    {
        TileManager.Instance.currentSkill = this;

        invoker = skillInvoker;
        unitAims.Clear();

        if (durationType != DurationType.oneTime)
        {
            BuffManager.Instance.AddBuff(this, duration);
        }

        if (aimType == AimType.selfUnit)
        {
            unitAims.Add(invoker);
        }
        else if (aimType == AimType.player)
        {              
            if ((aimPlayer == AimPlayer.enemy && invoker.moveController.player == BattleSystem.Instance.rightFighter) 
                || aimPlayer == AimPlayer.self && invoker.moveController.player == BattleSystem.Instance.leftFighter)
            {
                unitAims.AddRange(BattleSystem.Instance.leftFighter.FightControllers());
            }
            else if ((aimPlayer == AimPlayer.enemy && invoker.moveController.player == BattleSystem.Instance.leftFighter) 
                || (aimPlayer == AimPlayer.self && invoker.moveController.player == BattleSystem.Instance.rightFighter))
            {
                unitAims.AddRange(BattleSystem.Instance.rightFighter.FightControllers());
            }
            else if (aimPlayer == AimPlayer.both)
            {
                unitAims.AddRange(BattleSystem.Instance.leftFighter.FightControllers());
                unitAims.AddRange(BattleSystem.Instance.rightFighter.FightControllers());
            }
        }
        else
        {
            ChooseAim();
            return;
        }

        OnActivate();
    }
    public virtual void OnActivate()
    {

    }

    public virtual void OnReactivate()
    {

    }

    public virtual void OnDeactivate() 
    { 

    }

    public void ChooseAim()
    {
        if (aimType == AimType.oneTimeHex || aimType == AimType.oneTimeHexes || aimType == AimType.staticHex || aimType == AimType.staticHexes)
        {
            CameraRaycast.turnState = TurnState.ChooseSkillHex;
        }
        else if (aimType == AimType.unit || aimType == AimType.units)
        {
            CameraRaycast.turnState = TurnState.ChooseSkillUnit;
        }
    }

    public enum AimType
    {
        selfUnit,
        unit,
        units,
        player,
        oneTimeHex,
        staticHex,
        oneTimeHexes,
        staticHexes
    }

    public enum AimPlayer
    {
        self,
        enemy,
        both
    }

    public enum DurationType
    {
        oneTime,
        everyTurn, 
        severalTurns
    }
}
