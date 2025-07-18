using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusorLockHelper : Singleton<CusorLockHelper>
{
    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    
}
