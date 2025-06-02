using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUseHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("🖱️ Item tıklandı: " + eventData.button);

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("🟢 Sağ tık algılandı");

            item healItem = GetComponent<item>();
            if (healItem != null)
            {
                UseHeal(healItem.healAmount);
            }
        }
    }


    void UseHeal(float amount)
    {
        PlayerStats playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.Heal(amount);

            // Slot'tan sil
            Transform parentSlot = transform.parent;
            slot slotScript = parentSlot.GetComponent<slot>();
            if (slotScript != null)
                slotScript.currentItem = null;

            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("❌ PlayerStats bulunamadı.");
        }
    }
}
