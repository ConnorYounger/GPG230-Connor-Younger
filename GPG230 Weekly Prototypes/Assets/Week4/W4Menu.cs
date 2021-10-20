using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W4Menu : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    public GameObject levelUI;

    public void ReturnToMain()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
        levelUI.SetActive(false);
    }

    public void MoveToLevelUI()
    {
        cam2.SetActive(true);
        cam1.SetActive(false);

        StopCoroutine("ShowLevelUI");
        StartCoroutine("ShowLevelUI");
    }

    IEnumerator ShowLevelUI()
    {
        yield return new WaitForSeconds(1);

        levelUI.SetActive(true);
    }
}
