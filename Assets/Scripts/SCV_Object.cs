using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCollisionType { Mineral, CC, Wall }

public class SCV_Object : MonoBehaviour
{
    DLT_VOID_COLLIDER dltCollision;

    public void Init(DLT_VOID_COLLIDER dlt)
    {
        dltCollision = dlt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        dltCollision(other);
    }
}
