using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyType;
    public GameObject keyPrefab;

    public Key(string keyType)
    {
        this.keyType = keyType;
    }

    public string GetKeyType()
    {
        return this.keyType;
    }
    public GameObject GetKeyObject()
    {
        return this.keyPrefab;
    }
}
