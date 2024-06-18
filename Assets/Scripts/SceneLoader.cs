using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject btn;
    public GameObject how;

    public void BtnOn()
    {
        btn.SetActive(true);
        how.SetActive(false);
    }

    public void HowOn()
    {
        btn.SetActive(false);
        how.SetActive(true);
    }

    public void InGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        StopAllCoroutines();
        Application.Quit();
    }
}
