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

    private void Awake() {
        Instance = this;    
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
