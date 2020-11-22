using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindManager : MonoBehaviour
{
    //static this class to grab this dictionary across scenes
    [SerializeField]
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    //Serialize this struct to gain visibility once it becomes an array
    [System.Serializable]
    public struct KeyUISetup
    {
        //string to hold keyname, Text reference to display to UI and string to hold default key bind.
        public string keyName;
        public Text keyDisplayText;
        public string defaultKey;
    }
    //Struct becomes array allowing mutliple changes
    public KeyUISetup[] baseSetup;
    //reference to gameObject to hold keybind value input by player
    public GameObject currentKey;
    //apply color for ease of use
    public Color32 changedKey = new Color32(39, 171, 249, 255); //blue
    public Color32 selectedKey = new Color32(239, 116, 36, 255); //orange

    // Start is called before the first frame update
    void Start()
    {
        //onload run for loop to add keys from dictionary with either saved or default key binds
        for (int i = 0; i < baseSetup.Length; i++)
        {
            //determine if saved or default key, covert to Enum, add key from dictionary
            //playerprefs does not store strings - must convert here
            keys.Add(baseSetup[i].keyName, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(baseSetup[i].keyName, baseSetup[i].defaultKey)));
            //Display assigned key to UI element
            baseSetup[i].keyDisplayText.text = keys[baseSetup[i].keyName].ToString();
        }
    }
    /// <summary>
    /// Method to save keybinds
    /// Assign to UI element to save assigned keys
    /// </summary>
    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());    
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Method that passes through clickedKey into currentKey for key assignment
    /// changes color of clickedKey according to selectedKey color
    /// </summary>
    /// <param name="clickedKey"></param>
    public void ChangeKey(GameObject clickedKey)
    {
        currentKey = clickedKey;
        if(clickedKey != null)
        {
            currentKey.GetComponent<Image>().color = selectedKey;
        }
    }
    /// <summary>
    /// Method to trigger an event to capture key assignment
    /// Capture key assignement by using empty string newKey
    /// Assign captured key back into dictionary
    /// </summary>
    private void OnGUI()
    {
        //if current key is not empty, start event
        string newKey = "";
        Event e = Event.current;
        if (currentKey != null) 
        { 
            if (e.isKey)
            {
                newKey = e.keyCode.ToString();
            }
            //Resolving issue with Left/Right Shift keys
            if (Input.GetKey(KeyCode.RightShift))
            {
                newKey = "RightShift";
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                newKey = "LeftShift";
            }
            // if newKey string is not empty
            // Store captured key back into dictionary, update UI and reset
            if (newKey != "")
            {
                keys[currentKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey);
                currentKey.GetComponentInChildren<Text>().text = newKey;
                currentKey.GetComponent<Image>().color = changedKey;
                currentKey = null;
            }
        }
    }
}
