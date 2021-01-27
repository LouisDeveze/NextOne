using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingText : MonoBehaviour
{
    public TextBehaviour TextPrefab;
    float ms = 0;

    TextBehaviour currentText;

    // Start is called before the first frame update
    void Start()
    {
        TextPrefab.gameObject.SetActive(false);
        currentText = Instantiate(TextPrefab,transform);
        currentText.gameObject.SetActive(true);
      
    }

    // Update is called once per frame
    void Update()
    {
        ms += Time.deltaTime;
        if(ms > 5)
        {
            currentText = Instantiate(TextPrefab, transform);
            ms = 0;
            currentText.gameObject.SetActive(true);
        }
       

    }
}
