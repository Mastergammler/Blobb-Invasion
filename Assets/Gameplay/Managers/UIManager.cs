using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager mInstance;
    public static UIManager Instance 
    {
        get
        {
            if(mInstance == null) throw new UnityException("Trying to access UI manager b4 game object is initialized");
            return mInstance;
        }
        private set
        {
            mInstance = value;
        }
    }

    public GameObject uiCanvas;


    private IInventory mInventory;

    private Transform inventoryPanel;
    private Transform craftingPanel;

    private const int BULLET_BUTTON_CHILD_NO = 1;
    private const int WEAPON_BUTTON_CHILD_NO = 2;
    private const int CORE_BUTTON_CHILD_NO = 3;


    private void Awake() {
        Instance = this;    
    }


    // Start is called before the first frame update
    void Start()
    {
        inventoryPanel = uiCanvas.transform.GetChild(0);
        craftingPanel = uiCanvas.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetInventory(IInventory inventory)
    {
        mInventory = inventory;
    }

    // INPUT

    public void Inventory(InputAction.CallbackContext context)
    {
        GameManager.Instance.PauseResume();
    
        if(uiCanvas.activeSelf)
        {
            uiCanvas.SetActive(false);
        }
        else
        {
            ActivateCanvas();
        }
    }

    private void ActivateCanvas()
    {
        uiCanvas.SetActive(true);
        GameObject firstInventoryButton = uiCanvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        if(firstInventoryButton == null) Debug.Log("Something went wrong");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstInventoryButton);
        UpdateUiFromInventory();
    }

    private void UpdateUiFromInventory()
    {
        ScriptableBase currentWeapon = mInventory.GetActiveWeaponItem();
        craftingPanel.GetChild(WEAPON_BUTTON_CHILD_NO).GetComponentInChildren<ItemSlot>().AddItem(currentWeapon);
    }
}
