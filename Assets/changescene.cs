using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changescene : MonoBehaviour
{
    void Start()
    {
        // Wait for 3 seconds
        StartCoroutine(WaitAndLoadMenu(3f));
    }

    IEnumerator WaitAndLoadMenu(float waitTime)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        // Load the "Menu" scene
        SceneManager.LoadScene("Menu");
    }
}