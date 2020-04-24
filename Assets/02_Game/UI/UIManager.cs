using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BlobbInvasion.UI;
using BlobbInvasion.Utilities;

namespace BlobbInvasion.UI
{
    public class UIManager : MonoBehaviour
    {
        //#################
        //##  INSPECTOR  ##
        //#################

        public GameObject uiCanvas;
        public static UIManager Instance { private set; get; }

        //#################
        //##  CONSTANTS  ##
        //#################

        private const int BULLET_BUTTON_CHILD_NO = 1;
        private const int WEAPON_BUTTON_CHILD_NO = 2;
        private const int CORE_BUTTON_CHILD_NO = 3;

        private const int BULLET_SLOTS_CHILD_NO = 0;
        private const int WEAPON_SLOTS_CHILD_NO = 1;
        private const int CORE_SLOTS_CHILD_NO = 2;

        //###############
        //##  MEMBERS  ##
        //###############

        private IInventory mInventory;
        private Transform inventoryPanel;
        private Transform craftingPanel;

        //#############
        //##  MONO  ##
        //############

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            inventoryPanel = uiCanvas.transform.GetChild(0);
            craftingPanel = uiCanvas.transform.GetChild(1);
        }

        //#################
        //##  INTERFACE  ##
        //#################

        // Sets the inventory to the ui manager that the interactions work
        // Hos to be set by the inventory right at the start
        public void SetInventory(IInventory inventory)
        {
            mInventory = inventory;
            inventory.OnInventoryItemChanged += OnInventoryChanged;
        }

        // Changed Item when they get moved around in the inventory
        public void ChangeInventoryItem(ItemSlot slot)
        {
            CraftingType ct = Utils.ConvertCollectableType(slot.Item.Type);
            BulletData bullet = mInventory.GetActiveBulletItem();
            WeaponData weapon = mInventory.GetActiveWeaponItem();
            CoreData core = mInventory.GetActiveCoreItem();

            ScriptableObject slotItem = slot.Item;
            if (!slot.IsInventorySlot) slotItem = null;

            switch (ct)
            {
                case CraftingType.BULLET: bullet = (BulletData)slotItem; break;
                case CraftingType.WEAPON: weapon = (WeaponData)slotItem; break;
                case CraftingType.CORE: core = (CoreData)slotItem; break;
            }

            mInventory.SetActiveItems(bullet, weapon, core);
            updateCraftingItems();
        }

        //------------------
        //  Input Handler 
        //------------------

        public void Inventory(InputAction.CallbackContext context)
        {
            GameManager.Instance.PauseResume();

            if (uiCanvas.activeSelf)
            {
                uiCanvas.SetActive(false);
                MusicManager.Instance.DuckMusic(false);
            }
            else
            {
                ActivateCanvas();
            }
        }

        //##############
        //##  HELPER  ##
        //##############

        private void ActivateCanvas()
        {
            MusicManager.Instance.DuckMusic(true);
            uiCanvas.SetActive(true);
            GameObject firstInventoryButton = uiCanvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            if (firstInventoryButton == null) Debug.Log("Something went wrong");
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstInventoryButton);
            UpdateUiFromInventory();
        }
        private void UpdateUiFromInventory()
        {
            ScriptableBase currentWeapon = mInventory.GetActiveWeaponItem();
            craftingPanel.GetChild(WEAPON_BUTTON_CHILD_NO).GetComponentInChildren<ItemSlot>().AddItem(currentWeapon);
            ScriptableBase curBullet = mInventory.GetActiveBulletItem();
            craftingPanel.GetChild(BULLET_BUTTON_CHILD_NO).GetComponentInChildren<ItemSlot>().AddItem(curBullet);
            ScriptableBase curCore = mInventory.GetActiveCoreItem();
            craftingPanel.GetChild(CORE_BUTTON_CHILD_NO).GetComponentInChildren<ItemSlot>().AddItem(curCore);
        }

        private void OnInventoryChanged(object sender, InventoryEventArgs e)
        {
            updateCraftingItems();
            updateInventoryItem(e.Item, e.itemRemoved);
        }
        private void updateCraftingItems()
        {
            BulletData bullet = mInventory.GetActiveBulletItem();
            WeaponData weapon = mInventory.GetActiveWeaponItem();
            CoreData core = mInventory.GetActiveCoreItem();

            addOrRemoveCraftingItem(BULLET_BUTTON_CHILD_NO, bullet);
            addOrRemoveCraftingItem(WEAPON_BUTTON_CHILD_NO, weapon);
            addOrRemoveCraftingItem(CORE_BUTTON_CHILD_NO, core);
        }
        private void addOrRemoveCraftingItem(int childNo, ScriptableBase item)
        {
            ItemSlot slot = craftingPanel.GetChild(childNo).GetComponentInChildren<ItemSlot>();

            if (item == null) slot.RemoveItem();
            else slot.AddItem(item);
        }
        private void updateInventoryItem(ScriptableBase item, bool removeItem)
        {
            if (item == null)
            {
                Debug.LogWarning("The item you try to add to the UI is null!");
                return;
            }

            int childNo = 0;

            switch (Utils.ConvertCollectableType(item.Type))
            {
                case CraftingType.BULLET:
                    childNo = BULLET_SLOTS_CHILD_NO;
                    break;
                case CraftingType.WEAPON:
                    childNo = WEAPON_SLOTS_CHILD_NO;
                    break;
                case CraftingType.CORE:
                    childNo = CORE_SLOTS_CHILD_NO;
                    break;
            }

            ItemSlot[] itemSlots = inventoryPanel.GetChild(childNo).GetComponentsInChildren<ItemSlot>();

            if (removeItem)
            {
                foreach (ItemSlot slot in itemSlots)
                {
                    if (slot.Item.Equals(item))
                    {
                        slot.RemoveItem();
                        return;
                    }
                }
                Debug.LogWarning("Asked to remove item, but item not found in the inventory slots ....");
            }
            else
            {
                foreach (ItemSlot slot in itemSlots)
                {
                    if (slot.IsEmpty())
                    {
                        slot.AddItem(item);
                        return;
                    }
                }
                Debug.LogError("No empty slots found");
            }
        }
    }

    public enum CraftingType
    {
        BULLET, WEAPON, CORE
    }
}

