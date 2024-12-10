using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour
{
    [SerializeField] public int damage;

    void Start()
    {
        damage = 5;
    }

}
