using UnityEngine;

public class UIRootManager : MonoBehaviour
{
    private static UIRootManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Ayn�s�ndan birden fazla varsa sil
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahneler aras�nda ya�amaya devam et
        }
    }
}
