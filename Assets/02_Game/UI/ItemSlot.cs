using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using BlobbInvasion.Gameplay.Items.Crafting;

namespace BlobbInvasion.UI
{
    public class ItemSlot : MonoBehaviour
    {
        public ScriptableBase Item;

        public bool IsInventorySlot;

        private Image image;
        private Button parentButton;

        private void Awake()
        {
            image = GetComponent<Image>();
            parentButton = transform.parent.GetComponent<Button>();
            parentButton.onClick.AddListener(buttonClickEvent);
        }

        private void buttonClickEvent()
        {
            if (Item != null)
            {
                UIManager.Instance.ChangeInventoryItem(this);
            }
        }

        private void OnEnable()
        {
            if (Item != null)
            {
                image.sprite = Item.Art;
                image.enabled = true;
            }
        }


        public void AddItem(ScriptableBase item)
        {
            if (item == null) return;

            Item = item;
            if (image != null)
            {
                image.sprite = Item.Art;
                image.enabled = true;
            }
        }

        public void RemoveItem()
        {
            Item = null;
            if (image == null) return;
            image.sprite = null;
            image.enabled = false;
        }

        public bool IsEmpty()
        {
            return Item == null;
        }


    }
}