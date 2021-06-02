using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuipScript : MonoBehaviour
{
    public QuipController.CharacterType characterType;
    public QuipController quipController;
    public TextMeshPro tmp;
    public bool randomiseIntelligenceOnAwake;
    public int intelligenceLevel;
    [ReadOnly]
    public string selectedQuip;

    [Space(20)]

    public Color invisible;
    public Color visible;
    public float quipTime;
    public bool canTrigger = true;
    // Start is called before the first frame update
    void Awake()
    {
        NullReferenceCheck();
        if (randomiseIntelligenceOnAwake)
        {
            intelligenceLevel =  Random.Range(0, quipController.maxIntelligenceLevel);
        }
        selectedQuip = IntelligenceCheck;
    }

    public void NullReferenceCheck()
    {
        if(quipController == null)
        {
            quipController = FindObjectOfType<QuipController>();
            Debug.LogWarning("Assigning QuipController on " + gameObject.name + ". To maintain performance, try assigning variable out of runtime...");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string IntelligenceCheck
    {
        get
        {
            int selectedQuipInt = 0;
            string tempString = string.Empty;
                intelligenceLevel = Mathf.Clamp(intelligenceLevel, 1, quipController.maxIntelligenceLevel);
            for (int i = 0; i < quipController.quipLibrary.character.Length; i++)
            {
                if(quipController.quipLibrary.character[i].characterType == characterType)
                {
                    selectedQuipInt = Random.Range(0, quipController.quipLibrary.character[i].intelligenceLevel[intelligenceLevel].possibleQuips.Length);
                    tempString = quipController.quipLibrary.character[i].intelligenceLevel[intelligenceLevel].possibleQuips[selectedQuipInt].quip;
                    break;
                }
            }
            selectedQuip = tempString;
            return tempString;

            //Debug.LogError("Cannot get IntelligenceCheck as QuipController is null...");

        }
    }

    public void UpdateText()
    {
        if(canTrigger)
        {
            canTrigger = false;
            tmp.text = IntelligenceCheck;
            tmp.color = visible;
            StartCoroutine(TextDisplay());
        }
        
    }

    public IEnumerator TextDisplay()
    {
        yield return new WaitForSeconds(quipTime);
        canTrigger = true;
        tmp.color = invisible;
        

    }
}


