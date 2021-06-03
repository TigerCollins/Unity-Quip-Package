using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QuipController : MonoBehaviour
{
    /// <summary>
    /// Visible in inspector
    /// </summary>

    [ReadOnly]
    public readonly int maxIntelligenceLevel = 5;
    [Tooltip("The amount of quips allowed to be activated at once")]
    [Min(1)]
    public int maxQuipCount = 2;
    [ReadOnly]
    public int currentQuipCount;
    [ReadOnly]
    [SerializeField]
    private int mostRecentIntelligenceInt;

    [Space(5)]

    [Tooltip("Time quip is visible on screen.")]
    public float quipTime;

    [Space(10)]
    public ProximityDisplay proximityDisplay;

    [Header("Quip Library")]
    public Person[] character;




    /// <summary>
    /// Hidden in inspector
    /// </summary>

    [HideInInspector]
    public CharacterType characterType;
    [HideInInspector]
    public int wantedIntelligenceInt;
    [HideInInspector]
    public CharacterType wantedCharacterType;
    private int wantedQuipInt;
    private QuipScript latestQuipScript;
    public enum CharacterType
    {
        [InspectorName("Default Person - Ezio")]
        DefaultPerson,
        [InspectorName("Fire Person - Johnny Storm")]
        FirePerson,
        [InspectorName("Grapple Person - Indiana Jones")]
        GrapplePerson,
        [InspectorName("Ice Person - Frozone")]
        IcePerson,
        [InspectorName("Teleport Person - Minato")]
        TeleportPerson

    }

    public void Awake()
    {
        UpdateQuipNameInspector();
        UpdateIntelligenceLevelInspector();
        if(proximityDisplay.playerTransform == null)
        {
            Debug.LogError("Couldn't find play transform. Assign variable to resolve errors.");
        }
    }


    private void Update()
    {

        ProximityCountdown();

#if UNITY_EDITOR
        UpdateQuipNameInspector();
        UpdateIntelligenceLevelInspector();
#endif
    }



    public void WantedQuip(QuipScript quipScript)
    {
        wantedIntelligenceInt = quipScript.intelligenceInt - 1;
        wantedCharacterType = quipScript.characterType;
        latestQuipScript = quipScript;
     
        quipScript.selectedQuip = CallQuip;
        latestQuipScript = null;
    }

    public float SetCooldownTime
    {
        get
        {
            if(proximityDisplay.randomiseCooldownTime)
            {
                return Random.Range(3, 15);
            }

            else
            {
                return proximityDisplay.cooldownTime;
            }
        }
    }

    public string CallQuip
    {
        get
        {
            string tempString = string.Empty;
            for (int i = 0; i < character.Length; i++)
            {
                if (character.Length != 0)
                {
                    if(character[i].intelligenceLevel.Count != 0)
                    {
                        if (character[i].characterType == wantedCharacterType)
                        {
                            if (wantedIntelligenceInt > character[i].intelligenceLevel.Count)
                            {
                                Debug.LogWarning("Couldn't find the desired intelligence level, clamping int to return string and " +
                                    "lowering and adjusting the orignal QuipScript. Try adding intelligence levels in the Quip library " +
                                    "under " + character[i].characterType.ToString() +" to avoid this.");
                                int newIntelligenceInt = Mathf.Clamp(wantedIntelligenceInt, 0, character[i].intelligenceLevel.Count - 1);
                                wantedIntelligenceInt = newIntelligenceInt;
                                latestQuipScript.intelligenceInt = newIntelligenceInt + 1; //
                                if (character[i].intelligenceLevel[newIntelligenceInt].possibleQuips.Length > 0)
                                {
                                    wantedQuipInt = Random.Range(0, character[i].intelligenceLevel[newIntelligenceInt].possibleQuips.Length);
                                    tempString = character[i].intelligenceLevel[newIntelligenceInt].possibleQuips[wantedQuipInt].quipString;
                                }
                            }

                            else
                            {
                                if (character[i].intelligenceLevel[wantedIntelligenceInt].possibleQuips.Length > 0)
                                {

                                    wantedQuipInt = Random.Range(0, character[i].intelligenceLevel[wantedIntelligenceInt].possibleQuips.Length);
                                    tempString = character[i].intelligenceLevel[wantedIntelligenceInt].possibleQuips[wantedQuipInt].quipString;
                                }
                            }
                            mostRecentIntelligenceInt = wantedIntelligenceInt;
                            break;
                        }
                    }

                    else
                    {
                        Debug.LogError("Couldn't find any presets for intelligence, add some to the Quip Library under " + character[i].characterType.ToString());
                    }
                   
                }

                else
                {
                    Debug.LogError("Couldn't find any characters, add some to the Quip Library");
                }
            }
            return tempString;

            //Debug.LogError("Cannot get IntelligenceCheck as  is null...");

        }
    }


    void ProximityCountdown()
    {
        if(proximityDisplay.cooldownTimeRemaining > 0)
        {
            proximityDisplay.cooldownTimeRemaining -= Time.deltaTime;
        }
    }

#if UNITY_EDITOR


    private void UpdateQuipNameInspector()
    {
        for (int i = 0; i < character.Length; i++)
        {
            //Ran in an if statement for performance while in engine
            if (character[i].characterType.ToString() != character[i].name)
            {
                character[i].name = character[i].characterType.ToString();
            }

        }
    }

    public void UpdateIntelligenceLevelInspector()
    {
        for (int i = 0; i < character.Length; i++)
        {
            for (int ii = 0; ii < character[i].intelligenceLevel.Capacity; ii++)
            {
                character[i].intelligenceLevel[ii].UpdateElementName(ii);
            }
        }
    }

#endif
}



[System.Serializable]
public class Person
{
    [HideInInspector]
    public string name;
    public QuipController.CharacterType characterType;
    public List<IntelligenceLevel> intelligenceLevel = new List<IntelligenceLevel>();

    public void UpdatePlayerName()
    {
        name = characterType.ToString();
    }
}

[System.Serializable]
public class IntelligenceLevel
{
    [HideInInspector]
    public string name;
    public Quip[] possibleQuips;
    public void UpdateElementName(int charID)
    {
        int newID = charID + 1;
        name = newID.ToString();
    }

}

[System.Serializable]
public class Quip
{
    public string quipString;
}

[System.Serializable]
public class ProximityDisplay
{
    public Transform playerTransform;

    [Space(10)]

    public bool randomiseCooldownTime = true;
    [Tooltip("Cooldown before next proximity quip can be shown.")]
    public float cooldownTime;
    [ReadOnly]
    public float cooldownTimeRemaining;
}

