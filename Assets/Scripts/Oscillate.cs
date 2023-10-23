using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float myNewX = Mathf.Cos(Time.time) * 7f;
        transform.position = new Vector3( myNewX , transform.position.y, transform.position.z);   
    }
}
