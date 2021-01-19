using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBehaviour : MonoBehaviour
{
    public float TextWidth;
    public float pixelsPerSecond;
    RectTransform rt;

    public float GetXPosition { get { return rt.position.x; }  }
    public float GetWidth { get { return rt.rect.width; } }

     void Awake()
    {

        rt = GetComponent<RectTransform>();
        rt.position = new Vector3(Screen.width+rt.rect.width/2, rt.position.y, rt.position.z);
    }


    // Update is called once per frame
    void Update()
    {
        rt.position += Vector3.left * pixelsPerSecond * Time.deltaTime;

        if (GetXPosition <= -GetWidth)
        {
            Destroy(gameObject);
        }

    }
}
