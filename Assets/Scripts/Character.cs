using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public string characterName;

    // The root is the container for all images related to the character in the scene. The root object
    [HideInInspector] public RectTransform root;

    public Character (string _name)
    {
        CharacterManager cm = CharacterManager.instance;

        //locate character prefab
        GameObject prefab = Resources.Load("Characters/Character["+_name+"]") as GameObject;

        // spawn an instance
        GameObject ob = GameObject.Instantiate(prefab, cm.characterPanel);

        root = ob.GetComponent<RectTransform>();
        characterName = _name;

        // Get the renderer
        renderers.renderer = ob.GetComponentInChildren<Image>();
    }

    [System.Serializable]
    public class Renderers
    {
        public Image renderer;
    }

    public Renderers renderers = new Renderers();
}
