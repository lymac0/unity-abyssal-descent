using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 0.8f; // animasyon uzunlu�u kadar

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
