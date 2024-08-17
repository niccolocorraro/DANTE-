using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private bool isInputBlocked = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Makes sure the InputManager persists across scenes if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsInputBlocked()
    {
        return isInputBlocked;
    }

    public void BlockInput()
    {
        isInputBlocked = true;
    }

    public void UnblockInput()
    {
        isInputBlocked = false;
    }
}