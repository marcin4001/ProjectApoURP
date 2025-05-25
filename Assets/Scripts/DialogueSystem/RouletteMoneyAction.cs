using UnityEngine;

[CreateAssetMenu(fileName = "RouletteMoneyAction", menuName = "Dialogue/RouletteMoneyAction")]
public class RouletteMoneyAction : ActionDialogue
{
    public int money = 0;
    public override void Execute()
    {
        if (AmericanRoulette.instance == null) return;
        AmericanRoulette.instance.SpinWheel(money);
    }
}
