using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    public bool flagRaised = false;

    private void Awake()
    {
        int gameSessionCount = FindObjectsOfType<ScenePersist>().Length;
        if (gameSessionCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ToggleFlag(bool val)
    {
        flagRaised = val;
    }
}
