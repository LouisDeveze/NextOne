using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed;

    private float mH;
    private float mV;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector3(mH * speed, rb.velocity.y, mV * speed);
    }
    private void Update()
    {
        mH = Input.GetAxis("Horizontal");
        mV = Input.GetAxis("Vertical");
    }
}
