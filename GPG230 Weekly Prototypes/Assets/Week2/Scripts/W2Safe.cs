using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W2Safe : MonoBehaviour
{
    public string safeCodeFirstHalf;
    public string safeCodeSecondHalf;

    public int[] codeNumbers;
    public int[] codeGeneratedNumbers;
    public TMP_Text[] codeTexts;

    public W2Player player;

    public Animator safeAnimator;

    public GameObject keyItem;
    public Transform keySpawnPoint;

    public GameObject excalibur;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip editCodeSound;
    public AudioClip correctCodeSound;
    public AudioClip incorrectCodeSound;

    // Start is called before the first frame update
    void Start()
    {
        codeGeneratedNumbers = new int[6];
        codeNumbers = new int[codeGeneratedNumbers.Length];
        GenerateCode();   
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateCode()
    {
        for(int i = 0; i < codeGeneratedNumbers.Length; i++)
        {
            codeGeneratedNumbers[i] = Random.Range(0, 10);
        }

        safeCodeFirstHalf = codeGeneratedNumbers[0].ToString() + codeGeneratedNumbers[1].ToString() + codeGeneratedNumbers[2].ToString();
        safeCodeSecondHalf = codeGeneratedNumbers[3].ToString() + codeGeneratedNumbers[4].ToString() + codeGeneratedNumbers[5].ToString();
    }

    public void IncreaseValue(int i)
    {
        codeNumbers[i]++;

        if (codeNumbers[i] >= 10)
        {
            codeNumbers[i] = 0;
        }

        codeTexts[i].text = codeNumbers[i].ToString();

        PlaySound(editCodeSound);
    }

    public void DecreaseValue(int i)
    {
        codeNumbers[i]--;

        if (codeNumbers[i] < 0)
        {
            codeNumbers[i] = 9;
        }

        codeTexts[i].text = codeNumbers[i].ToString();

        PlaySound(editCodeSound);
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    bool EnterCorrectCode()
    {
        for(int i = 0; i < codeGeneratedNumbers.Length; i++)
        {
            if(codeNumbers[i] != codeGeneratedNumbers[i])
            {
                return false;
            }
        }

        return true;
    }

    public void EnterCode()
    {
        if (EnterCorrectCode())
        {
            Debug.Log("Correct code!");
            safeAnimator.SetBool("open", true);
            safeAnimator.gameObject.GetComponent<BoxCollider>().enabled = false;
            player.HideSafeUI();

            GameObject key = Instantiate(keyItem, keySpawnPoint.position, keySpawnPoint.rotation);
            key.transform.parent = transform;

            PlaySound(correctCodeSound);

            excalibur.SetActive(true);
        }
        else
        {
            Debug.Log("Incorrect code");
            StartCoroutine("IncorrectCode");
        }
    }

    IEnumerator IncorrectCode()
    {
        foreach (TMP_Text text in codeTexts)
            text.color = Color.red;

        PlaySound(incorrectCodeSound);

        yield return new WaitForSeconds(0.7f);

        foreach (TMP_Text text in codeTexts)
            text.color = Color.white;
    }
}
