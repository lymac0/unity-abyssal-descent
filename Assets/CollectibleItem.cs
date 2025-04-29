using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public int itemID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryController inventory = Object.FindFirstObjectByType<InventoryController>();
            GameObject prefab = inventory.itemPrefabs[itemID - 1]; // ID'ler 1'den başlıyor varsayımı
            inventory.AddItemToFirstEmptySlot(prefab);

            Debug.Log($"🎁 Oyuncu item aldı: ID = {itemID}");
            Destroy(gameObject); // item sahneden silinir
        }
    }
}
