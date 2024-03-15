using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null) return;

        item.data = _data;

        itemImage.sprite = _data.itemIcon;
        itemText.text = _data.itemName;

        if (itemText.text.Length > 15) itemText.fontSize = itemText.fontSize * .7f;
        else itemText.fontSize = 50;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(PlayerManager.instance.player.isRed) level0UI.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
        else ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
