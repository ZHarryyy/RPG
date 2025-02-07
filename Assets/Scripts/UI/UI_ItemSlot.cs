using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    [SerializeField] protected GameObject itemAmountBackground;

    protected UI ui;
    protected Level0UI level0UI;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
        level0UI = GetComponentInParent<Level0UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.itemIcon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
                itemAmountBackground.SetActive(true);
            }
            else
            {
                itemText.text = "";
                itemAmountBackground.SetActive(false);
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment) Inventory.instance.EquipItem(item.data);

        if (PlayerManager.instance.player.isRed) level0UI.itemToolTip.HideToolTip();
        else ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        if (PlayerManager.instance.player.isRed) level0UI.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
        else ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);

        Vector2 mousePosition = Input.mousePosition;
        float xOffset;
        if (mousePosition.x > Screen.width * .5f) xOffset = Screen.width * .12f * -1f;
        else xOffset = Screen.width * .12f;
        float yOffset;
        if (mousePosition.y > Screen.height * .5f) yOffset = Screen.height * .1f * -1;
        else yOffset = Screen.height * .1f;

        if (PlayerManager.instance.player.isRed) level0UI.itemToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
        else ui.itemToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;

        if (PlayerManager.instance.player.isRed) level0UI.itemToolTip.HideToolTip();
        else ui.itemToolTip.HideToolTip();
    }
}
