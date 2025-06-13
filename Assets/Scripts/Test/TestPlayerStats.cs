using UnityEngine;

public class TestPlayerStats : MonoBehaviour
{
    private PlayerStats playerStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameParam.instance.inDev)
            return;
        if (Input.GetKeyUp(KeyCode.F1))
            playerStats.AddHealthPoint(5);
        if (Input.GetKeyUp(KeyCode.F2))
            playerStats.RemoveHealthPoint(5);
        if (Input.GetKeyUp(KeyCode.F3))
            playerStats.AddOneRadLevel();
        if (Input.GetKeyUp(KeyCode.F4))
            playerStats.RemoveOneRadLevel();
        if(Input.GetKeyUp(KeyCode.F5))
            playerStats.RemoveAllLevelsRad();
    }
}
