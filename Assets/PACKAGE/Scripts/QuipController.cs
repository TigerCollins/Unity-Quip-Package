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
    [SerializeField]
    [ReadOnly]
    [Tooltip("The lower the number, the dumber the quip")]
    public int maxIntelligenceLevel;


    public QuipLibrary quipLibrary;

    /// <summary>
    /// Hidden in inspector
    /// </summary>

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
    }


    private void Update()
    {
       // if(!Application.isPlaying)
        //{
            UpdateQuipNameInspector();
            UpdateIntelligenceLevelInspector();
       // }

   
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

    public void UpdateIntelligenceLevelInspector()
    {
        for (int i = 0; i < quipLibrary.character.Length; i++)
        {
            for (int ii = 0; ii < quipLibrary.character[i].intelligenceLevel.Capacity; ii++)
            {
                quipLibrary.character[i].intelligenceLevel[ii].UpdateElementName(ii);
            }
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
        name = charID.ToString();
    }

}

[System.Serializable]
public class Quip
{
    public string quip;
}
