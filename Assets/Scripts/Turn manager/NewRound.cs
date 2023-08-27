using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRound : State
{
    public NewRound(BattleSystem battleSystem) : base(battleSystem)
    {

    }

    public override void Start()
    {

        BattleSystem.round++;

        BattleSystem.SetState(new PlayerTurn(BattleSystem));
    }

}

