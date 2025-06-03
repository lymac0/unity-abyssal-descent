using UnityEngine;

public class UIRootManager : MonoBehaviour
{
    private static UIRootManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Aynýsýndan birden fazla varsa sil
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahneler arasýnda yaþamaya devam et
        }
    }
}
