using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    [SerializeField] private Image[] keyImages;

    public void Start()
    {
        for (int i = 0; i < keyImages.Length; i++)
        {
            keyImages[i].enabled = false;
        }
    }

    public void UpdateKeys(List<KeyType> keys)
    {
        if (keys.Count == 0)
        {
            for (int i = 0; i < keyImages.Length; i++)
            {
                keyImages[i].enabled = false;
            }
        }
        for (int i = 0; i < keyImages.Length; i++)
        {
            for (int j = 0; j < keys.Count; j++)
            {
                if (keyImages[i].GetComponent<Key>().GetKeyType() == keys[j].GetKeyType())
                {
                    keyImages[i].enabled = true;
                }
                else
                {
                    keyImages[i].enabled = false;
                }
            }
        }
        
    }
}
