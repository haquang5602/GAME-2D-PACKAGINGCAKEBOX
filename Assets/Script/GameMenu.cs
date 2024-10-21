using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject guideObject;
    void Start()
    {
        DeactivateGuide();
    }
    public void OnGuideButtonClick()
    {
        
        ActivateGuide();
    }

    public void OnExitButtonClick()
    {
        
        DeactivateGuide();
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("Play");
    }

    private void ActivateGuide()
    {
        if (guideObject != null)
        {
            guideObject.SetActive(true);
        }
    }

    private void DeactivateGuide()
    {
        if (guideObject != null)
        {
            guideObject.SetActive(false);
        }
    }
}

