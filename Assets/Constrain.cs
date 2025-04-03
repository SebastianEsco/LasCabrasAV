using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constrain : MonoBehaviour
{
    public GameObject parent;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.Move(new Vector3(parent.transform.position.x, transform.position.y, parent.transform.position.z), Quaternion.identity);
    }
}
