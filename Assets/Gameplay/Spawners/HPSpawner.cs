using System.Reflection.Emit;
using UnityEngine;
using System;

public class HPSpawner : MonoBehaviour, IHpSpawner,ICollectionCallback
{
    public GameObject CollectableToSpawn;

    public event EventHandler OnItemCollected;

    public void Spawn()
    {
        var gObj = Instantiate(CollectableToSpawn,transform.position,Quaternion.identity);
        CollectableBase cb = gObj.GetComponent<CollectableBase>();
        if(cb == null) Debug.LogWarning("Collectable does not extend from collectable base! No callback possible!");
        else cb.RequestCallback(this);
    }

    public void HasBeenCollected()
    {
        OnItemCollected?.Invoke(this,new EventArgs());
    } 

}