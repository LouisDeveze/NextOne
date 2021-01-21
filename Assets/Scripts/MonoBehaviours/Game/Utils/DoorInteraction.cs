using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    public string LevelToLoad;
    public int Next = 0; // 1 : Go to the next level - 0 : Go to the previous level
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<NextOne.PlayerController>() != null)
        {
            //Player near door !
            //Debug.Log("Player near door");
            string NextMap;
            if (Next == 0)
            {
                NextMap = GlobalControl.Instance.PreviousMap();
            }
            else if (Next == 1) 
            {
                NextMap = GlobalControl.Instance.NextMap();
            }
            else {
                NextMap = LevelToLoad;

            }
            SceneManager.LoadScene(NextMap);
        }
    }
}
