using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarUI : MonoBehaviour
{
    [SerializeField] private Text PlayerName = null;
    [SerializeField] private Text HealthText = null;
    [SerializeField] private RawImage HealthBar = null;
    [SerializeField] private RawImage HealthBarGhost = null;

    private float percent;
    [SerializeField] private float ghostSpeed = .2f;

    private float test = 3000;
    private void Start()
    {
        SetLife(3000, 3000, true);
    }

    // Update is called once per frame
    void Update()
    {
        // Animate Ghost Percent;
        if(HealthBarGhost.rectTransform.anchorMax.x > HealthBarGhost.rectTransform.anchorMin.x) {
            float x = HealthBarGhost.rectTransform.anchorMax.x - (Time.deltaTime * ghostSpeed);
            x = Mathf.Max(x, HealthBarGhost.rectTransform.anchorMin.x);
            HealthBarGhost.rectTransform.anchorMax = new Vector2(x, HealthBarGhost.rectTransform.anchorMax.y);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            test -= 750;
            SetLife(test, 3000, false);
        }
    }

    /// <summary>
    /// Set the life of the player ensuring that it is in the bounds of 0-MaxLife
    /// If Instant is Set, there won't be any ghost animation of the life bar;
    /// </summary>
    /// <param name="newLife"></param>
    /// <param name="maxLife"></param>
    /// <param name="instant"></param>
    public void SetLife(float newLife, float maxLife, bool instant)
    {
        newLife = newLife >= 0 ? (newLife <= maxLife ? newLife: maxLife): 0;

        this.HealthText.text = newLife.ToString() + " / " + maxLife;

        // Calculate Percent
        float percent = (float)newLife / (float)maxLife;

        // Set Health Bar
        Vector2 anchor = new Vector2(percent, this.HealthBar.rectTransform.anchorMax.y);
        this.HealthBar.rectTransform.anchorMax = anchor;


        if (instant) this.HealthBarGhost.rectTransform.anchorMax = anchor;

        anchor.y = this.HealthBarGhost.rectTransform.anchorMin.y;
        this.HealthBarGhost.rectTransform.anchorMin = anchor;
        
    }
}
