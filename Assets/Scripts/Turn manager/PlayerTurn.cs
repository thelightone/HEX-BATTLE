using System.Text;

public class PlayerTurn : State
{
    public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
    {
        
    }
    public override void Start()
    {
        BattleSystem.rightFighter.DisLight();
        actions = 0;
        BattleSystem.curPlayer = BattleSystem.leftFighter;

        BattleSystem.leftFighter.HighLight();
    }

    public override void Act()
    {
        actions++;
        if (actions > 1)
        {
           
            BattleSystem.SetState(new EnemyTurn(BattleSystem));
        }

    }
}
