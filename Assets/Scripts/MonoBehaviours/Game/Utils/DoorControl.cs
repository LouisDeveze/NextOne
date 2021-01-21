using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //If player and second door -> open
        //if (other.GetComponent<NextOne.PlayerController>() != null && other.GetComponent<DoorControl>() != null
        if (other.GetComponent<NextOne.PlayerController>() != null)
        {
            Debug.Log("open Sesam !");
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Close the door
        //if (other.GetComponent<NextOne.PlayerController>() != null && other.GetComponent<DoorControl>() != null)
        if (other.GetComponent<NextOne.PlayerController>() != null)
            {
            Debug.Log("Bye Sesam !");
            gameObject.SetActive(true);
        }
    }
}
