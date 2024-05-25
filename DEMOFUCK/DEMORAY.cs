using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMORAY : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PhysicsTrigger.RayCast(Camera.main.ScreenPointToRay(Input.mousePosition), 15, (hit) => { 
                print("µ„µΩ¡À"+hit.collider.name);
            },"Enemy","Friend");
        }
    }
}
