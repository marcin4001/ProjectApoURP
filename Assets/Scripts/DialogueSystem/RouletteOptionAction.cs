using UnityEngine;

[CreateAssetMenu(fileName = "RouletteOptionAction", menuName = "Dialogue/RouletteOptionAction")]
public class RouletteOptionAction : ActionDialogue
{
    public BetType type = BetType.color;
    public ColorBet colorBet = ColorBet.black;
    public EvenOddBet evenOddBet = EvenOddBet.even;
    public LowHighBet lowHighBet = LowHighBet.low;
    public override void Execute()
    {
        if(AmericanRoulette.instance == null) return;
        AmericanRoulette.instance.SetOption(type, colorBet, evenOddBet, lowHighBet);
    }
}
