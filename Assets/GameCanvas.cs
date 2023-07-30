using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] Button spinButton;

    [SerializeField] TextMeshProUGUI textTargetScore, textCurrentScore, textActionsRemaining;
    [SerializeField] TextAsset hintTextFile;
    public void SpinButton()
    {
        FindObjectOfType<Wheel>().Spin();
    }

    public void UpdateCanvasText(int actionsRemaining, int currentScore, int targetScore = 99999999)
    {
        if(targetScore != 99999999)
        {
            textTargetScore.text = "Target: " + targetScore.ToString();
        }
        textActionsRemaining.text = actionsRemaining.ToString();
        textCurrentScore.text = "Current: " + currentScore.ToString();
    }   
    
}
