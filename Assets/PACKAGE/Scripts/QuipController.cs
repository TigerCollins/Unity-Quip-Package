using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QuipController : MonoBehaviour
{
    /// <summary>
    /// Visible in inspector
    /// </summary>
    public CharacterType characterType;
    public QuipLibrary quipLibrary;

    [SerializeField]
    [Tooltip("The lower the number, the dumber the quip")]
    private int _intelligenceLevel;
    [SerializeField]
    [Range(1,10)]
    private int _maxIntelligenceLevel;

    /// <summary>
    /// Hidden in inspector
    /// </summary>

    [HideInInspector]
    public int maxIntelligence = 4;

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

    }


    private void Update()
    {
        if(!Application.isPlaying)
        {
            UpdateQuipNameInspector();
            UpdateIntelligenceLevelInspector();
            UpdateCharacterName();
        }

    }

    public int IntelligenceCheck
    {
        get
        {
            _intelligenceLevel = Mathf.Clamp(_intelligenceLevel, 1, maxIntelligence);
            return _intelligenceLevel;
        }

        set
        {
            _intelligenceLevel = Mathf.Clamp(value,1,maxIntelligence);
            UpdateIntelligenceLevelInspector();
        }
    }


#if UNITY_EDITOR
    private void UpdateQuipNameInspector()
    {
       for (int i = 0; i<quipLibrary.character.Length; i++)
            {
                //Ran in an if statement for performance while in engine
                if(quipLibrary.character[i].characterType.ToString() != quipLibrary.character[i].name)
                {
                    quipLibrary.character[i].name = quipLibrary.character[i].characterType.ToString();
                }
                
            }
    }

    private void UpdateIntelligenceLevelInspector()
    {
        for (int i = 0; i < quipLibrary.character.Length; i++)
        {
            for (int ii = 0; ii < quipLibrary.character[i].intelligenceLevel.Length; ii++)
            {
                quipLibrary.character[i].intelligenceLevel[ii].UpdateElementName(ii);
            }
        }
     
    }

    private void UpdateCharacterName()

    {
        for (int i = 0; i < quipLibrary.character.Length; i++)
        {
            quipLibrary.character[i].name = characterType.ToString();
        }
    }
#endif



}


[System.Serializable]
public class QuipLibrary
{
    public Person[] character;
}

[System.Serializable]
public class Person
{
    [HideInInspector]
    public string name;
    public QuipController.CharacterType characterType;
    public IntelligenceLevel[] intelligenceLevel;
}

[System.Serializable]
public class IntelligenceLevel
{
    [HideInInspector]
    public string name;
    public Quip[] possibleQuips;
    public void UpdateElementName(int charID)
    {
        name = charID.ToString();
    }

}

[System.Serializable]
public class Quip
{
    public string quip;
}
