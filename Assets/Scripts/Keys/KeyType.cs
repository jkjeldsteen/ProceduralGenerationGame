using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyType
{
    public string keyType;
    public GameObject keyPrefab;

    public KeyType(string keyType, GameObject keyObject)
    {
        this.keyType = keyType;
        this.keyPrefab = keyObject;
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
