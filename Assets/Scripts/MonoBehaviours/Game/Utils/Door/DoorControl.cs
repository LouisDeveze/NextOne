using NextOne;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public float speed = 0.01f;
    public int typeDoor = 0;    // 0: door BT  1: door LR

    private Transform poz;
    private Vector3 direction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Animator>() != null)
        {
            this.transform.position = speed * Time.deltaTime * direction;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Animator>() != null)
        {
            this.transform.position = -speed * Time.deltaTime * direction;
        }
    }

    public void Awake()
    {
        int y = 0;
        switch (typeDoor)
        {
            case 0:
                direction = new Vector3(0, y, 1);
                break;
            case 1:
                direction = new Vector3(1, y, 0);
                break;
        }
        poz = this.transform;
    }
}
