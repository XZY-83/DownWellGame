using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Collection", menuName = "Character/CharacterCollection")]
public class CharacterCollector : ScriptableObject
{
    [SerializeField] private List<CharacterProfile> characters;
    [SerializeField] private List<CharacterEvent> characterEvents;

    [System.Serializable]
    public class CharacterProfile
    {
        public GameObject pref;
        public bool locked = false;
        public string CharacterName
        {
            get
            {
                return pref.GetComponent<Player>().name;
            }
        }
    }

    [System.Serializable]
    public class CharacterEvent
    {
        public string requirement = "";
        public int amount;
    }

    public List<CharacterProfile> Characters
    { 
        get 
        {
            return characters;
        }
    }

    public CharacterProfile GetCharacterProfile(string name)
    {
        return characters.Find(c => c.pref.GetComponent<Player>().name == name);
    }
    
    public CharacterProfile GetCharacterProfile(int num)
    {
        return characters.Find(n => n.pref.GetComponent<Player>().num == num);
    }
}
