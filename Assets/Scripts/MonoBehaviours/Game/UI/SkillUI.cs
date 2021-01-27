using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{

    [SerializeField] private string index = "Skill";
    [SerializeField] private RawImage image;
    [SerializeField] private RawImage progress;
    [SerializeField] private Text control;


    // Set Pogress 
    void setProgress(float percent)
    {
        percent = percent > 1 ? 1 : (percent < 0 ? 0 : percent);
        this.progress.rectTransform.anchorMax = new Vector2(progress.rectTransform.anchorMax.y, percent);
    }

    // Change the picture
    void setControl(string text) { this.control.text = text; }

    // Change the picture
    void setImage(Texture texture) { this.image.texture = texture; }

    // Sets the color of the cooldown progress image
    void setProgressColor(Color color) { this.progress.color = color; }
}
