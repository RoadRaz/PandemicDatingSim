using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    // All characters attached to Character Panel
    public RectTransform characterPanel;

    // A list of characters in the scene
    public List<Character> characters = new List<Character>();

    // Easy lookup fro characters
    public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();
    void Awake()
    {
        instance = this;
    }
    
    public Character GetCharacter(string characterName, bool createCharIfNotExist = true)
    {
        int index = -1;
        if (characterDictionary.TryGetValue(characterName, out index))
        {
            return characters[index];
        }
        else if (createCharIfNotExist)
        {
            CreateCharacter(characterName);
        }
        return null;
    }

    // Create a character
    public Character CreateCharacter(string characterName)
    {
        Character newCharacter = new Character(characterName);
        characterDictionary.Add(characterName, characters.Count);
        characters.Add(newCharacter);
        return newCharacter;
    }
}
