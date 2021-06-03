using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuipScript : MonoBehaviour
{
    public QuipController.CharacterType characterType;
    [SerializeField]
    private QuipController quipController;
    [SerializeField]
    private TextMeshPro tmp;
    [SerializeField]
    private bool randomiseIntelligenceOnAwake;
    [Range(1,5)]
    public int intelligenceInt;
    [ReadOnly]
    [SerializeField]
    public string selectedQuip;

    [Space(20)]

    [ReadOnly]
    [SerializeField]
    private Color invisible = new Color(0,0,0,0);
    [SerializeField]
    private Color visible = new Color(255, 255, 255, 255);
    [ReadOnly]
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private bool canTrigger = true;

    [Space(20)]

    public ProximitySettings proximity;
    // Start is called before the first frame update
    void Awake()
    {
        NullReferenceCheck();
       if(proximity.distanceProximityRange < proximity.nearProximityRange && proximity.useProximity)
        {
            Debug.LogError("Near Proximity Range needs to be higher than Distance Proximity for correct functionality.");
        }
    }
    private void Start()
    {
        if (randomiseIntelligenceOnAwake == true)
        {
            intelligenceInt = Random.Range(1, quipController.maxIntelligenceLevel);
        }
        //selectedQuip = QuipText;

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
        ProximityCheck();
    }

    public void ProximityCheck()
    {
        if(proximity.useProximity && quipController.proximityDisplay.playerTransform != null)
        {
            proximity.distanceFromTarget = Vector3.Distance(quipController.proximityDisplay.playerTransform.position, gameObject.transform.position);
            if (proximity.distanceFromTarget > proximity.nearProximityRange && proximity.distanceFromTarget  < proximity.distanceProximityRange)
            {
                UpdateText();
            }
        }
      
    }


    public string QuipText
    {
        get
        {
            selectedQuip = string.Empty;
            intelligenceInt = Mathf.Clamp(intelligenceInt, 1, quipController.maxIntelligenceLevel);
            quipController.WantedQuip(this);
            return selectedQuip;
        }
    }

    public void UpdateText()
    {
        if(canTrigger && quipController.currentQuipCount < quipController.maxQuipCount && !isActive && quipController.proximityDisplay.cooldownTimeRemaining <= 0 && proximity.useProximity)
        {
            isActive = true;
            quipController.currentQuipCount++;
            tmp.text = QuipText;
            tmp.color = visible;
            StartCoroutine(TextDisplay());
        }

        else if (canTrigger && quipController.currentQuipCount < quipController.maxQuipCount && !isActive && !proximity.useProximity)
        {
            isActive = true;
            quipController.currentQuipCount++;
            tmp.text = QuipText;
            tmp.color = visible;
            StartCoroutine(TextDisplay());
        }


    }

    public IEnumerator TextDisplay()
    {
        yield return new WaitForSeconds(quipController.quipTime);
        tmp.color = invisible;
        quipController.currentQuipCount--;
        isActive = false;
        if(proximity.useProximity)
        {
            quipController.proximityDisplay.cooldownTimeRemaining = quipController.SetCooldownTime;
        }
    }
}

[System.Serializable]
public class ProximitySettings
{
    [Tooltip("Stops taking raycast in favour for proximity")]
    public bool useProximity;

    [Space(5)]

    [ReadOnly]
    public float distanceFromTarget;
    [Min(0)]
    public float nearProximityRange = 2;
    [Min(1)]
    public float distanceProximityRange = 5;
}



