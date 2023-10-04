using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParent : ScriptableObject
{
    public Sprite sprite;
    public string skillName;
    public bool durated;
    public int duration;
    public bool self;
    public UnitFightController invoker;
    public UnitFightController unitAim;
    public HexTile hexAim;

    public void Init(UnitFightController skillInvoker)
    {
        invoker = skillInvoker;

        if (durated)
        {
            BuffManager.Instance.AddBuff(this, duration);
        }

        if (self)
        {
            unitAim = invoker;
        }
        else
        {
            ChooseAim();
        }
        OnActivate();
    }
    public virtual void OnActivate()
    {

    }

    public virtual void OnDeactivate() 
    { 

    }

    public void ChooseAim()
    {
        
    }
}
