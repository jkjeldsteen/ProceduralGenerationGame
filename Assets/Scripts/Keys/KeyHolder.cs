using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyHolder : MonoBehaviour
{
    public List<KeyType> keys;
    [SerializeField] private GameObject keyUI;

    private void Start()
    {
        this.keys = new();
        keyUI = GameObject.FindGameObjectWithTag("KeyUI");
    }

    public void UpdateKeys()
    {
        keyUI.GetComponent<KeyUI>().UpdateKeys(this.keys);
    }

    public bool CompareKeyType(string compareString)
    {
        bool keyFound = false;
        for (int i = 0; i < keys.Count; i++)
        {
            if(keys[i].GetKeyType().Equals(compareString))
            {
                keyFound = true;
            }
        }
        
        return keyFound;
    }

    public void RemoveKey(string compareString)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].GetKeyType().Equals(compareString))
            {
                keys.Remove(keys[i]);
            }
        }
        UpdateKeys();
    }
}
