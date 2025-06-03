using System;
using UnityEngine;

public static class PlayerEvents
{
    public static event Action<GameObject> OnPlayerSpawned;

    public static void RaisePlayerSpawned(GameObject player)
    {
        OnPlayerSpawned?.Invoke(player);
    }
}
