using UnityEngine;

public class PlayerViewRotator : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            player.Rotate(0, -90f, 0);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            player.Rotate(0, 90f, 0);
        }
    }
}
