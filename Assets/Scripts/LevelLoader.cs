using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] public LevelInfo[] levels;
    public const string PROGRESS = "progress";

    [SerializeField] HotbarSlot[] hotbarSlots;
    [SerializeField] Segment blankSegmentPrefab;

    List<GameObject> levelPieces = new List<GameObject>();

    private void Awake()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        //all currently active wheels and segments are destroyed and then new ones loaded by accessing the current level using playerprefs.
        ClearAllLevelPieces();
        SpawnWheel();
        SpawnSegments();
        FindObjectOfType<Menu>().LoadHint(levels[LevelLoader.LoadProgress()].hint);
    }

    private void SpawnSegments()
    {
        //check the current level file for segment informartion and generate a segments based on each one.
        for (int i = 0; i < levels[LoadProgress()].segments.Length; i++)
        {
            var newSegmentPrefab = Instantiate(blankSegmentPrefab, hotbarSlots[i].transform.position, Quaternion.identity);
            //add segment to a hotbar slot.
            newSegmentPrefab.transform.parent = hotbarSlots[i].transform;

            //give new segment the information from level file
            newSegmentPrefab.powerType = levels[LoadProgress()].segments[i].powerType;
            newSegmentPrefab.powerAmount = levels[LoadProgress()].segments[i].powerAmount;

            //tell the hotbar slot to check its segment and display the information.
            hotbarSlots[i].CheckSegment();

            //add each segment to the level pieces list to allow them all to be destroyed on a new level load.
            levelPieces.Add(newSegmentPrefab.gameObject);
        }
    }

    private void SpawnWheel()
    {
        var newWheel = Instantiate(levels[LoadProgress()].wheel, Vector3.zero, Quaternion.identity);
        LoadLevelInformation();

        //add the wheel to the level pieces list to allow it to be destroyed on a new level load.
        levelPieces.Add(newWheel.gameObject);
    }

    private void ClearAllLevelPieces()
    {
        //check level pieces exist.
        if (levelPieces.Count <= 0) {return;}
        //Destroy them if they do.
        foreach (var piece in levelPieces)
        {
            Destroy(piece);
        }
    }

    public void LoadLevelInformation()
    {
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
