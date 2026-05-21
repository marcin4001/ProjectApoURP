using Steamworks;
using UnityEngine;

public static class SteamAchievements
{
    public static void Add(string Id)
    {
        if(!SteamManager.Initialized)
            return;
        Debug.Log(Id);
        SteamUserStats.SetAchievement(Id);
        SteamUserStats.StoreStats();
    }
}
