using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : State
{ 
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem)
    {

    }

    public override void Start()
    {
        BattleSystem.leftFighter.DisLight();
        actions = 0;
        BattleSystem.curPlayer = BattleSystem.rightFighter;

        BattleSystem.rightFighter.HighLight();
    }

    public override void Act()
    {
        actions++;
        if (actions > 1)
        {
            
            BattleSystem.SetState(new NewRound(BattleSystem));
        }

    }
}

