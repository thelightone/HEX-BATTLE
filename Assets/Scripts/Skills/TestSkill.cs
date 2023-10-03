using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : SkillParent
{
    public BuffManager BuffManager;
public void Start()
    {
        BuffManager.curBuffs.Add(this);
    }
}
