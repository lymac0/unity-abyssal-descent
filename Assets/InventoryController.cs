using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

  


    void Start()
    {
        itemDictionary = UnityEngine.Object.FindFirstObjectByType<ItemDictionary>();


    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            slot slot = slotTransform.GetComponent<slot>();

            if (slot == null)
            {
                Debug.LogError("❌ slot component bulunamadı.");
                continue;
            }

            if (slot.currentItem != null)
            {
                item item = slot.currentItem.GetComponent<item>();
                Debug.Log($"📦 Slot dolu: ID = {item.ID}, Slot = {slotTransform.GetSiblingIndex()}");
                invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
            else
            {
                Debug.Log($"🚫 Slot {slotTransform.GetSiblingIndex()} boş (currentItem = null)");
            }
        }
        return invData;
    }


    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        // Mevcut slotları temizle
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Yeni boş slotlar oluştur
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        // Kaydedilen item'ları uygun slotlara yerleştir
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex >= inventoryPanel.transform.childCount)
            {
                Debug.LogWarning($"❗ Slot index {data.slotIndex} panelde yok, atlanıyor.");
                continue;
            }

            slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<slot>();
            GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
            if (itemPrefab != null)
            {
                GameObject item = Instantiate(itemPrefab, slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;

                // 🧠 Ekstra kontrol: item.cs component'i var mı?
                item itemScript = item.GetComponent<item>();
                if (itemScript != null)
                {
                    Debug.Log($"📥 Yükleniyor: itemID = {itemScript.ID}, slotIndex = {data.slotIndex}");
                }
                else
                {
                    Debug.LogWarning($"❌ Yüklenen item prefab'ında item.cs eksik! (ID: {data.itemID})");
                }
            }
            else
            {
                Debug.LogWarning($"❌ itemPrefab bulunamadı! itemID: {data.itemID}");
            }
        }
    }

    public void AddItemToFirstEmptySlot(GameObject itemPrefab)
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            slot slot = inventoryPanel.transform.GetChild(i).GetComponent<slot>();
            if (slot.currentItem == null)
            {
                GameObject item = Instantiate(itemPrefab, slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                item itemScript = item.GetComponent<item>();
                if (itemScript == null)
                {
                    Debug.LogError("❌ Envantere eklenen prefab'ta item.cs yok!");
                }
                else
                {
                    Debug.Log($"🧩 Inventory’ye eklenen item ID: {itemScript.ID}");
                }

                slot.currentItem = item;
                return;
            }
        }

        Debug.LogWarning("⚠ Inventory dolu!");
    }

    public void ClearInventory()
    {
        Debug.Log("🧹 Envanter temizleniyor...");
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void InitializeEmptySlots()
    {
        Debug.Log($"🧱 {slotCount} boş slot oluşturuluyor...");
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
    }


}
