using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour 
{
    public ScriptableBase Item;
    private Image image;

    private void Awake() 
    {
        image = GetComponent<Image>();  
    }
    private void OnEnable() 
    {
        if(Item != null)
        {
            image.sprite = Item.Art;
            image.enabled = true;
        }
    }

    public void AddItem(ScriptableBase item)
    {
        Item = item;
    }

    public void RemoveItem()
    {
        Item = null;
        if(image == null) return;
        image.sprite = null;
        image.enabled = false;
    }

    public bool IsEmpty()
    {
        return Item == null;
    }


}