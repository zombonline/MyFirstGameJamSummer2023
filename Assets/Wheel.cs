using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] int portions = 8;
    [SerializeField] public int actionsRemaining, currentScore, targetScore;
    float rotationAmount, targetRotation;
    [SerializeField] Portion currentPortion;
    [SerializeField] Transform wheelToRotate;
    GameCanvas gameCanvas;
    [SerializeField] public int spinDirection = 1;
    bool isSpinning;

    private void Start()
    {
        rotationAmount = 360f / portions;

        gameCanvas = FindObjectOfType<GameCanvas>();
        gameCanvas.UpdateCanvasText(actionsRemaining, currentScore, targetScore);

        targetRotation = wheelToRotate.eulerAngles.z + rotationAmount;
        if (targetRotation > 360f) { targetRotation -= 360f; }
    }
    public void Spin()
    {
        if (isSpinning || !CheckAllPortionsUsed())
        {
            return;
        }
        //TODO: check all portions to ensure all occupied
        StartCoroutine(SpinCoroutine());
    }

    public bool CheckAllPortionsUsed()
    {
        bool allUsed = true;
        foreach(Portion portion in FindObjectsOfType<Portion>())
        {
            if(!portion.isOccupied)
            {
                allUsed = false;
            }
        }
        return allUsed;
    }

    IEnumerator SpinCoroutine()
    {
        isSpinning = true;
        FindObjectOfType<SegmentController>().disabled = true;
        //TODO: insert a bool to say spin is in progress to stop multiple spins
        targetRotation = Mathf.FloorToInt(wheelToRotate.eulerAngles.z) + Mathf.FloorToInt(rotationAmount/2) *spinDirection;
        while (Mathf.FloorToInt(wheelToRotate.eulerAngles.z) < targetRotation)
        {
            wheelToRotate.eulerAngles = new Vector3(0, 0, wheelToRotate.eulerAngles.z + spinDirection);
            yield return new WaitForSeconds(.025f);
        }
        CheckSlice();
        if (currentPortion.transform.childCount > 0)
        {
            yield return new WaitForSeconds(1f);
        }



        while (actionsRemaining > 0)
        {
            targetRotation = Mathf.FloorToInt(wheelToRotate.eulerAngles.z) + Mathf.FloorToInt(rotationAmount) *spinDirection;
            if (targetRotation > 360f) { targetRotation -= 360f;}
            if(targetRotation < 0f) { targetRotation += 360f;}
            while (Mathf.FloorToInt(wheelToRotate.eulerAngles.z) != targetRotation)
            {
                Debug.Log(Mathf.FloorToInt(wheelToRotate.eulerAngles.z));
                
                wheelToRotate.eulerAngles = new Vector3(0, 0, wheelToRotate.eulerAngles.z + spinDirection);
                yield return new WaitForSeconds(.025f);
            }
            CheckSlice();
            if (currentPortion.transform.childCount > 0)
            {
                yield return new WaitForSeconds(1f);
            }
        }
        SpinOver();
    }

    void SpinOver()
    {
        FindObjectOfType<SegmentController>().disabled = false;

        if (currentScore == targetScore)
        {
            Debug.Log("You win!");
            LevelLoader.SetProgress(LevelLoader.LoadProgress() + 1);
            FindObjectOfType<Menu>().FadeInBlur();
            FindObjectOfType<Menu>().OpenResultsScreen();
        }
        else
        {
            Debug.Log("You Failed!");
            isSpinning = false;
            FindObjectOfType<LevelLoader>().ResetLevel();
            gameCanvas.UpdateCanvasText(actionsRemaining, currentScore, targetScore);
            wheelToRotate.transform.eulerAngles = Vector3.zero;
        }
    }

    void CheckSlice()
    {
        if(currentPortion.transform.childCount>0)
        {
            actionsRemaining--;
            currentScore = currentPortion.transform.GetChild(0).GetComponent<Segment>().ApplySegmentPower(currentScore);
            gameCanvas.UpdateCanvasText(actionsRemaining, currentScore);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Portion"))
        {
            currentPortion = collision.GetComponent<Portion>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portion") && currentPortion == collision.GetComponent<Portion>())
        {
            currentPortion = null;
        }
    }
}
