using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject wndwResetProgressWarning,wndwResults;
    [SerializeField] Volume volume;
    [SerializeField] TextAsset hintTextFile;
    [SerializeField] string[] hints;

    [SerializeField] TextMeshProUGUI textHint;

    private void Start()
    {
        hints = hintTextFile.text.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);
        LoadHint();
    }
    public void ResetProgress()
    {
        LevelLoader.SetProgress(0);
        SceneManager.LoadScene("Splash");
    }

    public void FadeInBlur()
    {
        StartCoroutine(FadeCoroutine(volume.weight, 1f, .125f)); 
    }
    public void FadeOutBlur()
    {
        StartCoroutine(FadeCoroutine(volume.weight, 0f, .125f));
    }
    IEnumerator FadeCoroutine(float start, float end, float duration)
    {
        while(volume.weight != end)
        {
            if (start < end)
            {
                volume.weight += .01f;
                if(volume.weight > end)
                {
                    volume.weight = end;
                }
            }
            else if (start > end)
            {
                volume.weight -= .01f;
                if (volume.weight < end)
                {
                    volume.weight = end;
                }
            }
            yield return new WaitForSeconds(duration *.01f);
        }
    }

    public void OpenResultsScreen()
    { 
        wndwResults.SetActive(true);
    }
    public void LoadHint()
    {
        var hint = FindObjectOfType<LevelLoader>().levels[LevelLoader.LoadProgress()].hint;
        if (hint == "")
        {
            hint = hints[UnityEngine.Random.Range(0, hints.Length)];
        }
        textHint.text = hint;

    }
}
