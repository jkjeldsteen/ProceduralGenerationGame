using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public string lockType;
    [SerializeField] private ParticleSystem particles;

    public LockedDoor(string lockType)
    {
        this.lockType = lockType;
    }

    public string GetLockType()
    {
        return this.lockType;
    }

    private void OnDestroy()
    {
        Instantiate(particles, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
    }
}
