using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class OscEncodeur : MonoBehaviour
{

    public OSCReceiver oscReceiver;
    public OSCTransmitter oscTransmitter;
    public GameObject targetGameObject;
    public float jumpForce = 10f;
    public float torqueMultiplier = -5f;
    Rigidbody2D targetRigidbody2D;
    

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    void RotMessageReceived(OSCMessage oscMessage)
    {
        
        // We will store the value in a float even if we get an int
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // If message is neither Int or Float do nothing
            return;
        }

        /*
        float rotation = ScaleValue(value, 0, 4095, 45, 315);
        potGameObject.transform.eulerAngles = new Vector3(0, 0, rotation);
        */
        targetRigidbody2D.AddTorque(value * torqueMultiplier);
    }

    float previousButValue;
    void ButMessageReceived(OSCMessage oscMessage)
    {
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // If message is neither Int or Float do nothing
            return;
        }
        if (value != previousButValue && value == 0)
        {
            targetRigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        previousButValue = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        targetRigidbody2D = targetGameObject.GetComponent<Rigidbody2D>();
        oscReceiver.Bind("/rot", RotMessageReceived);
        oscReceiver.Bind("/but", ButMessageReceived);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
