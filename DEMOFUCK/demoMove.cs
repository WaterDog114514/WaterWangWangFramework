using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoMove : MonoBehaviour
{
    public GameObj gameObj;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void set(GameObj obj)
    {
        gameObj = obj;
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward*10*Time.deltaTime);
    }
}
