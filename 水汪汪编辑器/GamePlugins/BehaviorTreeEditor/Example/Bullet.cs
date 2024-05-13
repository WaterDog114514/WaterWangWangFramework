using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject,3);
    }
    private void Update()
    {
        transform.Translate(Vector3.forward*5*Time.deltaTime*2);
    }
}
