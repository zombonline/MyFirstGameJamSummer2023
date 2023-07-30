using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] public LevelInfo[] levels;
    public const string PROGRESS = "progress";

    [SerializeField] Transform[] segmentPositions;
    [SerializeField] Segment blankSegmentPrefab;

    List<GameObject> levelPieces = new List<GameObject>();

    private void Awake()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        if(levelPieces.Count > 0 )
        {
            foreach(var piece in levelPieces)
            {
                Destroy(piece);
            }
        }
        var newWheel = Instantiate(levels[LoadProgress()].wheel, Vector3.zero, Quaternion.identity);
        newWheel.actionsRemaining = levels[LoadProgress()].actions;
        newWheel.currentScore = 0;
        newWheel.targetScore = levels[LoadProgress()].targetScore;
        levelPieces.Add(newWheel.gameObject);
        for (int i = 0; i < levels[LoadProgress()].segments.Length; i++)
        {
            var newSegmentPrefab = Instantiate(blankSegmentPrefab, segmentPositions[i].position, Quaternion.identity);
            newSegmentPrefab.transform.parent = segmentPositions[i].transform;
            newSegmentPrefab.powerType = levels[LoadProgress()].segments[i].powerType;
            newSegmentPrefab.powerAmount = levels[LoadProgress()].segments[i].powerAmount;
            levelPieces.Add(newSegmentPrefab.gameObject);
        }
    }

    public void ResetLevel()
    {
        Debug.Log("Reset");
        FindObjectOfType<Wheel>().actionsRemaining = levels[LoadProgress()].actions;
        FindObjectOfType<Wheel>().currentScore = 0;
        FindObjectOfType<Wheel>().targetScore = levels[LoadProgress()].targetScore;
        
    }

    public static void SetProgress(int level)
    {
        PlayerPrefs.SetInt(PROGRESS, level);
    }

    public static int LoadProgress()
    {
        return PlayerPrefs.GetInt(PROGRESS, 0);
    }

}
