using UnityEngine;
using System;

public class PlayerEvents : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerSpawned;

    public static void RaisePlayerSpawned(GameObject Player)
    {
        OnPlayerSpawned?.Invoke(Player);
    }
}
