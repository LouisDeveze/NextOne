using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDoor : MonoBehaviour
{
    private LevelGenerator templates;
    private void Start()
    {
        if (this.gameObject.transform.position.y == 0)
        {
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<LevelGenerator>();
            templates.AddDoor(this.gameObject);
        }        
    }
}
