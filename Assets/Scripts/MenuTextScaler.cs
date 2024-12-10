using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTextScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScaleTextImage(float scale)
    {
        transform.localScale = new Vector3(1*scale, 1*scale, 0f);
    }
}
