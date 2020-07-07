using UnityEngine;
using UnityEngine.UI;
using Kryz.CharacterStats;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
	public int Health = 50;

	[Header("Stats")]
	public CharacterStat Strength;
	public CharacterStat Agility;
	public CharacterStat Intelligence;
	public CharacterStat Vitality;

	[Header("Public")]
	public Inventory Inventory;
	public EquipmentPanel EquipmentPanel;
	public SkillWindow SkillWindow;

	[Header("Serialize Field")]
	[SerializeField] CraftingWindow craftingWindow;
	[SerializeField] StatPanel statPanel;
	[SerializeField] ItemTooltip itemTooltip;
	[SerializeField] Image draggableIcon;
	[SerializeField] DropItemArea dropItemArea;
	[SerializeField] QuestionDialog reallyDropItemDialog;
	[SerializeField] ItemSaveManager itemSaveManager;
	[SerializeField] WeaponController weaponController;
	[SerializeField] EquipmentSpriteController equipmentSpriteController;
	[SerializeField] int playerNumber;

	private BaseItemSlot dragItemSlot;
	private BaseSkillSlot dragSkillSlot;

	private void OnValidate()
	{
		if (itemTooltip == null)
			itemTooltip = FindObjectOfType<ItemTooltip>();
	}

	private void Awake()
	{
		statPanel.SetStats(Strength, Agility, Intelligence, Vitality);
		statPanel.UpdateStatValues();

		// Setup Events:
		// Right Click
		Inventory.OnRightClickEvent += InventoryRightClick;
		EquipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
		// Pointer Enter
		Inventory.OnPointerEnterEvent += ShowTooltip;
		EquipmentPanel.OnPointerEnterEvent += ShowTooltip;
		craftingWindow.OnPointerEnterEvent += ShowTooltip;
		// Pointer Exit
		Inventory.OnPointerExitEvent += HideTooltip;
		EquipmentPanel.OnPointerExitEvent += HideTooltip;
		craftingWindow.OnPointerExitEvent += HideTooltip;
		// Begin Drag
		Inventory.OnBeginDragEvent += BeginDrag;
		SkillWindow.OnBeginDragEvent += BeginDrag;
		EquipmentPanel.OnBeginDragEvent += BeginDrag;
		// End Drag
		Inventory.OnEndDragEvent += EndDrag;
		SkillWindow.OnEndDragEvent += EndDrag;
		EquipmentPanel.OnEndDragEvent += EndDrag;
		// Drag
		Inventory.OnDragEvent += Drag;
		SkillWindow.OnDragEvent += Drag;
		EquipmentPanel.OnDragEvent += Drag;
		// Drop
		Inventory.OnDropEvent += Drop;
		SkillWindow.OnDropEvent += Drop;
		EquipmentPanel.OnDropEvent += Drop;
		dropItemArea.OnDropEvent += DropItemOutsideUI;
	}

	private void Start()
	{
		if (itemSaveManager != null)
		{
			itemSaveManager.LoadEquipment(this);
			itemSaveManager.LoadInventory(this);
		}
		playerNumber = GameHandler.Instance.AddCharacter(this);	
	}

	public int GetPlayerNumber(){
		return playerNumber;
	}

	private void OnDestroy()
	{
		if (itemSaveManager != null)
		{
			itemSaveManager.SaveEquipment(this);
			itemSaveManager.SaveInventory(this);
		}
	}

	private void InventoryRightClick(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item is EquippableItem)
		{
			Equip((EquippableItem)itemSlot.Item);
		}
		else if (itemSlot.Item is UsableItem)
		{
			UsableItem usableItem = (UsableItem)itemSlot.Item;
			usableItem.Use(this);

			if (usableItem.IsConsumable)
			{
				itemSlot.Amount--;
				usableItem.Destroy();
			}
		}
	}

	private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item is EquippableItem)
		{
			Unequip((EquippableItem)itemSlot.Item);
		}
	}

	private void ShowTooltip(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item != null)
		{
			itemTooltip.ShowTooltip(itemSlot.Item);
		}
	}

	private void HideTooltip(BaseItemSlot itemSlot)
	{
		if (itemTooltip.gameObject.activeSelf)
		{
			itemTooltip.HideTooltip();
		}
	}

	private void BeginDrag(UISlot uiSlot)
	{
		if(uiSlot is BaseItemSlot){
			BaseItemSlot itemSlot = uiSlot as BaseItemSlot;
			if (itemSlot.Item != null)
			{
				dragItemSlot = itemSlot;
				draggableIcon.sprite = itemSlot.Item.Icon;
				draggableIcon.transform.position = Input.mousePosition;
				draggableIcon.gameObject.SetActive(true);
			}
		}
		else if (uiSlot is BaseSkillSlot){
			BaseSkillSlot skillSlot = uiSlot as BaseSkillSlot;
			if (skillSlot.Skill != null)
			{
				dragSkillSlot = skillSlot;
				draggableIcon.sprite = skillSlot.Skill.Icon;
				draggableIcon.transform.position = Input.mousePosition;
				draggableIcon.gameObject.SetActive(true);
			}
		}	
	}

	private void Drag(UISlot itemSlot)
	{
		draggableIcon.transform.position = Input.mousePosition;
	}

	private void EndDrag(UISlot itemSlot)
	{
		dragItemSlot = null;
		dragSkillSlot = null;
		draggableIcon.gameObject.SetActive(false);
	}

	private void Drop(UISlot dropUISlot)
	{
		if(dropUISlot is BaseItemSlot){
			BaseItemSlot dropItemSlot = dropUISlot as BaseItemSlot;
			if (dragItemSlot == null) return;

			if (dropItemSlot.CanAddStack(dragItemSlot.Item))
			{
				AddStacks(dropItemSlot);
			}
			else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
			{
				SwapItems(dropItemSlot);
			}
		}
		else if(dropUISlot is BaseSkillSlot){
			BaseSkillSlot dropSkillSlot = dropUISlot as BaseSkillSlot;
			if(dragSkillSlot == null) return;

			if(dropSkillSlot is UsableSkillSlot){
				ActiveSkill holdSkill = dragSkillSlot.Skill as ActiveSkill;
				if(dragSkillSlot is UsableSkillSlot){
					SkillWindow.RemoveSkill(holdSkill);
					if(dropSkillSlot.Skill)
						SkillWindow.EquipSkill(dropSkillSlot.Skill as ActiveSkill, 0, dragSkillSlot as UsableSkillSlot);
				}
				SkillWindow.EquipSkill(holdSkill, 0, dropUISlot as UsableSkillSlot);
			}
		}
	}

	private void AddStacks(BaseItemSlot dropItemSlot)
	{
		int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
		int stacksToAdd = Mathf.Min(numAddableStacks, dragItemSlot.Amount);

		dropItemSlot.Amount += stacksToAdd;
		dragItemSlot.Amount -= stacksToAdd;
	}

	private void SwapItems(BaseItemSlot dropItemSlot)
	{
		EquippableItem dragEquipItem = dragItemSlot.Item as EquippableItem;
		EquippableItem dropEquipItem = dropItemSlot.Item as EquippableItem;

		if (dropItemSlot is EquipmentSlot)
		{
			if (dragEquipItem != null){
				dragEquipItem.Equip(this);
				UpdateEquipItemController(dragEquipItem);
			} 
			if (dropEquipItem != null) dropEquipItem.Unequip(this);
			
		}
		if (dragItemSlot is EquipmentSlot)
		{
			if (dragEquipItem != null) dragEquipItem.Unequip(this);
			if (dropEquipItem != null){
				dropEquipItem.Equip(this);
				UpdateEquipItemController(dropEquipItem);
			} 
			else UpdateUnequipItemController(dragEquipItem);
		}
		statPanel.UpdateStatValues();

		Item draggedItem = dragItemSlot.Item;
		int draggedItemAmount = dragItemSlot.Amount;

		dragItemSlot.Item = dropItemSlot.Item;
		dragItemSlot.Amount = dropItemSlot.Amount;

		dropItemSlot.Item = draggedItem;
		dropItemSlot.Amount = draggedItemAmount;
	}

	private void DropItemOutsideUI()
	{
		if (dragItemSlot == null && dragSkillSlot == null) return;
		if(dragItemSlot){
			//if dragging item, ask if you want to destroy it
			reallyDropItemDialog.Show();
			BaseItemSlot slot = dragItemSlot;
			reallyDropItemDialog.OnYesEvent += () => DestroyItemInSlot(slot);
		}
		else if(dragSkillSlot){
			//if dragging skill from usable skill slot, unequip it
			if(dragSkillSlot is UsableSkillSlot)
				SkillWindow.RemoveSkill(dragSkillSlot.Skill as ActiveSkill);
		}
	}

	private void DestroyItemInSlot(BaseItemSlot itemSlot)
	{
		// If the item is equiped, unequip first
		if (itemSlot is EquipmentSlot)
		{
			EquippableItem equippableItem = (EquippableItem)itemSlot.Item;
			equippableItem.Unequip(this);
			UpdateUnequipItemController(equippableItem);
		}

		itemSlot.Item.Destroy();
		itemSlot.Item = null;
	}

	public void Equip(EquippableItem item)
	{
		if (Inventory.RemoveItem(item))
		{
			EquippableItem previousItem;
			if (EquipmentPanel.AddItem(item, out previousItem))
			{
				if (previousItem != null)
				{
					Inventory.AddItem(previousItem);
					previousItem.Unequip(this);
					statPanel.UpdateStatValues();
				}
				item.Equip(this);
				UpdateEquipItemController(item);
				
				statPanel.UpdateStatValues();
			}
			else
			{
				Inventory.AddItem(item);
			}
		}
	}

	public void Unequip(EquippableItem item)
	{
		if (Inventory.CanAddItem(item) && EquipmentPanel.RemoveItem(item))
		{
			item.Unequip(this);
			statPanel.UpdateStatValues();
			Inventory.AddItem(item);
			UpdateUnequipItemController(item);
		}
	}

	public void UpdateEquipItemController(EquippableItem item){
		if((int)item.EquipmentType == 4){
			weaponController.EquipPrimaryWeapon(item.itemGameObject);
		}
		else if((int)item.EquipmentType == 5){
			weaponController.EquipSecondaryWeapon(item.itemGameObject);
		}
		else{
			equipmentSpriteController.EquipItem(item);
		}
	}

	public void UpdateUnequipItemController(EquippableItem item){
		if((int)item.EquipmentType == 4){
			weaponController.UnequipPrimaryWeapon();
		}
		else if((int)item.EquipmentType == 5){
			weaponController.UnequipSecondaryWeapon();
		}
		else{
			equipmentSpriteController.UnequipItem(item);
		}
	}

	private ItemContainer openItemContainer;

	private void TransferToItemContainer(BaseItemSlot itemSlot)
	{
		Item item = itemSlot.Item;
		if (item != null && openItemContainer.CanAddItem(item))
		{
			Inventory.RemoveItem(item);
			openItemContainer.AddItem(item);
		}
	}

	private void TransferToInventory(BaseItemSlot itemSlot)
	{
		Item item = itemSlot.Item;
		if (item != null && Inventory.CanAddItem(item))
		{
			openItemContainer.RemoveItem(item);
			Inventory.AddItem(item);
		}
	}

	public void OpenItemContainer(ItemContainer itemContainer)
	{
		openItemContainer = itemContainer;

		Inventory.OnRightClickEvent -= InventoryRightClick;
		Inventory.OnRightClickEvent += TransferToItemContainer;

		itemContainer.OnRightClickEvent += TransferToInventory;

		itemContainer.OnPointerEnterEvent += ShowTooltip;
		itemContainer.OnPointerExitEvent += HideTooltip;
		itemContainer.OnBeginDragEvent += BeginDrag;
		itemContainer.OnEndDragEvent += EndDrag;
		itemContainer.OnDragEvent += Drag;
		itemContainer.OnDropEvent += Drop;
	}

	public void CloseItemContainer(ItemContainer itemContainer)
	{
		openItemContainer = null;

		Inventory.OnRightClickEvent += InventoryRightClick;
		Inventory.OnRightClickEvent -= TransferToItemContainer;

		itemContainer.OnRightClickEvent -= TransferToInventory;

		itemContainer.OnPointerEnterEvent -= ShowTooltip;
		itemContainer.OnPointerExitEvent -= HideTooltip;
		itemContainer.OnBeginDragEvent -= BeginDrag;
		itemContainer.OnEndDragEvent -= EndDrag;
		itemContainer.OnDragEvent -= Drag;
		itemContainer.OnDropEvent -= Drop;
	}

	public void UpdateStatValues()
	{
		statPanel.UpdateStatValues();
	}
	
	public float FetchStatOnEnum(int idx){
		switch(idx){
			case 0:
				return Strength.Value;
			case 1:
				return Agility.Value;
			case 2:
				return Intelligence.Value;
			case 3:
				return Vitality.Value;
			default:
				return 0;
		}
	}
}
