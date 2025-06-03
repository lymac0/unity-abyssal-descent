using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 0.8f; // animasyon uzunluðu kadar

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
