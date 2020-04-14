using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
