using System.Collections.Generic;
using UnityEngine;

public enum BetType
{
    color, evenOdd, lowHigh
}

public enum ColorBet
{
    red, black
}

public enum EvenOddBet
{
    even, odd
}

public enum LowHighBet
{
    low, high
}

public class AmericanRoulette : MonoBehaviour
{
    public static AmericanRoulette instance;
    [SerializeField] private BetType type = BetType.color;
    [SerializeField] private ColorBet colorBet = ColorBet.black;
    [SerializeField] private EvenOddBet evenOddBet = EvenOddBet.even;
    [SerializeField] private LowHighBet lowHighBet = LowHighBet.low;
    [SerializeField] private int money = 0;
    [SerializeField] private List<string> colorList = new List<string>();
    private int idMoney = 202;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i <= 36; i++)
        {
            if (i == 0)
                colorList.Add("green");
            else if ((i >= 1 && i <= 10) || (i >= 19 && i <= 28))
                colorList.Add(i % 2 == 0 ? "black" : "red");
            else
                colorList.Add(i % 2 == 0 ? "red" : "black");
        }
        colorList.Add("green");
    }

    public void SetOption(BetType _type, ColorBet _colorBet, EvenOddBet _evenOddBet, LowHighBet _lowHighBet)
    {
        type = _type;
        colorBet = _colorBet;
        evenOddBet = _evenOddBet;
        lowHighBet = _lowHighBet;
    }

    public void SpinWheel(int _money)
    {
        money = _money;
        Item dollarItem = ItemDB.instance.GetItemById(idMoney);
        SlotItem dollarSlot = new SlotItem(dollarItem, money);
        DebugInfo();
        if (!Inventory.instance.PlayerHaveItem(dollarSlot))
        {
            DialogueUI.instance.SetReply("You don’t have enough money to place a bet. Come back later.");
            return;
        }
        if (type == BetType.color)
            SpinColorBet();
        else if(type == BetType.evenOdd)
            SpinEvenOddBet();
        else if(type == BetType.lowHigh)
            SpinLowHighBet();
    }

    public void SpinColorBet()
    {
        int randomNum = Random.Range(0, 38);
        string colorName = colorList[randomNum];
        string numberName = (randomNum == 37) ? "00": randomNum.ToString();
        if(colorName == "green")
        {
            DialogueUI.instance.SetReply($"Unfortunately, it's {numberName} ({colorName}) - you lose {money} dollars");
            UpdateBalance(false);
            return;
        }
        ColorBet currentBet = (colorName == "red") ? ColorBet.red : ColorBet.black;
        if(currentBet == colorBet)
            DialogueUI.instance.SetReply($"Congratulations! It's {numberName} ({colorName}) - you win {money} dollars!");
        else
            DialogueUI.instance.SetReply($"Unfortunately, it's {numberName} ({colorName}) - you lose {money} dollars");
        UpdateBalance(currentBet == colorBet);
    }

    public void SpinEvenOddBet()
    {
        int randomNum = Random.Range(0, 38);
        string numberName = (randomNum == 37) ? "00" : randomNum.ToString();
        if (randomNum == 0 || randomNum == 37)
        {
            DialogueUI.instance.SetReply($"Unfortunately, it's {numberName} – you lose {money} dollars, zero is neither even nor odd.");
            UpdateBalance(false);
            return;
        }
        EvenOddBet currentBet = (randomNum % 2 == 0) ? EvenOddBet.even : EvenOddBet.odd;
        if(currentBet == evenOddBet)
            DialogueUI.instance.SetReply($"Congratulations! It's {numberName} ({currentBet}) - you win {money} dollars!");
        else
            DialogueUI.instance.SetReply($"Unfortunately, it's {numberName} ({currentBet}) - you lose {money} dollars");
        UpdateBalance(currentBet == evenOddBet);
    }

    public void SpinLowHighBet()
    {
        int randomNum = Random.Range(0, 38);
        string numberName = (randomNum == 37) ? "00" : randomNum.ToString();
        if (randomNum == 0 || randomNum == 37)
        {
            DialogueUI.instance.SetReply($"Unfortunately, it's {numberName} (out of range) - you lose {money} dollars, zero is neither low nor high.");
            UpdateBalance(false);
            return;
        }
        LowHighBet currentBet = (randomNum <= 18) ? LowHighBet.low : LowHighBet.high;
        if (currentBet == lowHighBet)
            DialogueUI.instance.SetReply($"Congratulations! It's {numberName} ({currentBet}) - you win {money} dollars!");
        else
            DialogueUI.instance.SetReply($"Unfortunately, it's {numberName} ({currentBet}) - you lose {money} dollars");
        UpdateBalance(currentBet == lowHighBet);
    }

    public void UpdateBalance(bool win)
    {
        Item dollarItem = ItemDB.instance.GetItemById(idMoney);
        SlotItem dollarSlot = new SlotItem(dollarItem, money);
        if(win)
        {
            Inventory.instance.AddItem(dollarSlot);
        }
        else
        {
            Inventory.instance.RemoveItem(dollarSlot);
        }
    }
    public void DebugInfo()
    {
        if (type == BetType.color)
            Debug.Log($"Type: {type} Opt: {colorBet} Money: {money}");
        else if (type == BetType.evenOdd)
            Debug.Log($"Type: {type} Opt: {evenOddBet} Money: {money}");
        else if (type == BetType.lowHigh)
            Debug.Log($"Type: {type} Opt: {lowHighBet} Money: {money}");
    }
}
