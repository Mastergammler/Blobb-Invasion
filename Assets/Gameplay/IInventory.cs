using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory 
{
    event EventHandler<InventoryEventArgs> OnInventoryItemChanged;
    int GetItemAmountFor(CollectableType type);
    void AddInventoryItem(ScriptableBase item,int amount);

    CoreData GetActiveCoreItem();
    BulletData GetActiveBulletItem();
    WeaponData GetActiveWeaponItem();

    void SetActiveItems(BulletData bullet, WeaponData weapon, CoreData core);
}

public class InventoryEventArgs : EventArgs
{
    public ScriptableBase Item;
    public int amount;
}