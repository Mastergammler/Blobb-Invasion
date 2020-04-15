using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour 
{
    public ScriptableBase Item;
    private Image image;

    private void Start() 
    {
        image = GetComponent<Image>();   
    }

    public void AddItem(ScriptableBase item)
    {
        image.sprite = item.Art;
        image.enabled = true;
    }

    public void RemoveItem()
    {
        Item = null;
        image.sprite = null;
        image.enabled = false;
    }

    public bool IsEmpty()
    {
        return Item == null;
    }


}