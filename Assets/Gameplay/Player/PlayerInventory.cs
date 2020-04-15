using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IInventory 
{
    public event EventHandler<InventoryEventArgs> OnInventoryItemChanged;

    public WeaponData StartWeapon;


    private Dictionary<ScriptableBase,int> mInventory = new Dictionary<ScriptableBase, int>();
    private CoreData mActiveCore;
    private WeaponData mActiveWeapon;
    private BulletData mActiveBullet;

    private void Start() 
    {
        mActiveWeapon = StartWeapon;   
        UIManager.Instance.SetInventory(this);
    }

    //---------------
    //  I Inventory
    //---------------

    public int GetItemAmountFor(CollectableType type)
    {
        throw new NotImplementedException("Not neccessary yet!");
    }

    public void AddInventoryItem(CollectableType type)
    {
        Debug.Log("Adding item to inventory: " + type.ToString());
        AddInventoryItem(CollectableFactory.Instance.CreateCollectable(type));
    }

    private void AddInventoryItem(ScriptableBase item,int amount = 1)
    {
        if(item == null) return;

        int newAmount = amount;

        // dodo check if the equals works
        if(mInventory.ContainsKey(item))
        {
            mInventory.TryGetValue(item,out newAmount);
            newAmount += amount;
            mInventory.Remove(item);
        }

        mInventory.Add(item,newAmount);

        OnInventoryItemChanged?.Invoke(this,new InventoryEventArgs(){ Item = item,amount = newAmount});
    }

    // Removes the active item from the inventory and saves it in the local variable
    public void SetActiveItems(BulletData bullet, WeaponData weapon, CoreData core)
    {
        AddInventoryItem(mActiveBullet);
        removeIventoryItem(bullet);

        AddInventoryItem(mActiveWeapon);
        removeIventoryItem(weapon);

        AddInventoryItem(mActiveCore);
        removeIventoryItem(core);

        mActiveBullet = bullet;
        mActiveWeapon = weapon;
        mActiveCore = core;
    }

    private void removeIventoryItem(ScriptableBase item)
    {
        if(item == null) return;
        if(mInventory.ContainsKey(item))
        {
            mInventory.Remove(item);
            OnInventoryItemChanged?.Invoke(this,new InventoryEventArgs(){itemRemoved = true, Item = item, amount = 0});
        }
    }

    public CoreData GetActiveCoreItem()
    {
        return mActiveCore;
    }
    public BulletData GetActiveBulletItem()
    {
        return mActiveBullet;
    }
    public WeaponData GetActiveWeaponItem()
    {
        return mActiveWeapon;
    }



}