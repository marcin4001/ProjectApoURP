using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineUI : MonoBehaviour
{
    public static SlotMachineUI instance;
    [SerializeField] private Image slot1Img;
    [SerializeField] private Image slot2Img;
    [SerializeField] private Image slot3Img;

    [SerializeField] private Sprite[] slotSprites;
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        panel.SetActive(false);
    }

    public void Draw(SlotItem moneySlot)
    {
        panel.SetActive(true);
        int index1 = 0;
        int index2 = 0;
        int index3 = 0;
        int randomNumWin = Random.Range(0, 101);
        if (randomNumWin <= 50)
        {
            int randomIndex = Random.Range(0, slotSprites.Length);
            index1 = randomIndex;
            index2 = randomIndex;
            index3 = randomIndex;
            HUDController.instance.AddConsolelog("You won 10 dollars.");
            Inventory.instance.AddItem(moneySlot);
        }
        else
        {
            index1 = Random.Range(0, slotSprites.Length);
            index2 = Random.Range(0, slotSprites.Length);
            index3 = Random.Range(0, slotSprites.Length);
            index3 += 1;
            if (index3 >= slotSprites.Length)
                index3 = 0;
            HUDController.instance.AddConsolelog("You lost 5 dollars.");
            Inventory.instance.RemoveItem(moneySlot);
        }
        StartCoroutine(DrawSlot(slot1Img, index1, 1));
        StartCoroutine(DrawSlot(slot2Img, index2, 2));
        StartCoroutine(DrawSlot(slot3Img, index3, 3));
        StartCoroutine(EndDraw());
    }

    private IEnumerator DrawSlot(Image slot, int validIndex, float time)
    {
        float counter = 0;
        int index = Random.Range(0, slotSprites.Length);
        while(counter < time)
        {
            slot.sprite = slotSprites[index];
            yield return new WaitForSeconds(0.05f);
            index++;
            if(index == slotSprites.Length)
                index = 0;
            counter += 0.05f;
        }
        yield return new WaitForSeconds(0.05f);
        slot.sprite = slotSprites[validIndex];
    }

    private IEnumerator EndDraw()
    {
        yield return new WaitForSeconds(5f);
        panel.SetActive(false);
    }
}
