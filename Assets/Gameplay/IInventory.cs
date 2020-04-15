using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory 
{
    // Called when items are added or removed from the inventory
    event EventHandler<InventoryEventArgs> OnInventoryItemChanged;
    int GetItemAmountFor(CollectableType type);
    void AddInventoryItem(CollectableType type);

    CoreData GetActiveCoreItem();
    BulletData GetActiveBulletItem();
    WeaponData GetActiveWeaponItem();

    void SetActiveItems(BulletData bullet, WeaponData weapon, CoreData core);
}

public class InventoryEventArgs : EventArgs
{
    public bool itemRemoved = false;
    public ScriptableBase Item;
    public int amount;
}