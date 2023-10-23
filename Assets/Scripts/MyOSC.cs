using extOSC;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class MyOSC : MonoBehaviour
{

    public extOSC.OSCReceiver oscReceiver;
    public extOSC.OSCTransmitter oscTransmitter;
    public GameObject potGameObject;
    public GameObject photoGameObject;
    public GameObject positionGameObject;

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp( ((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin,outputMax);
    }

    void PotMessageReceived(OSCMessage oscMessage)
    {
        // We will store the value in a float even if we get an int
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int )
        {
            value = oscMessage.Values[0].IntValue;
        } else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        } else
        {
            // If message is neither Int or Float do nothing
            return;
        }

        float rotation = ScaleValue(value, 0, 4095, 45, 315);
        potGameObject.transform.eulerAngles = new Vector3(0,0,rotation);
    }

    void PhotoMessageReceived(OSCMessage oscMessage)
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

        float scale = ScaleValue(value, 1000, 3900, 0.5f, 2.5f);
        photoGameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    // Start is called before the first frame update
    void Start()
    {
        oscReceiver.Bind("/pot", PotMessageReceived);
        oscReceiver.Bind("/photo", PhotoMessageReceived);
    }

    // Update is called once per frame
    void Update()
    {
    }

    float myChronoStart;

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {

        if (Time.realtimeSinceStartup - myChronoStart >= 0.05f )
        {
            myChronoStart = Time.realtimeSinceStartup;

            // Create message
            var myOscMessage = new OSCMessage("/pixel");

            // Populate values.
            float myPositionX = positionGameObject.transform.position.x;
            float myScaledPositionX = ScaleValue(myPositionX, -7, 7, 0, 255);
            myOscMessage.AddValue(OSCValue.Int((int)myScaledPositionX));
            myOscMessage.AddValue(OSCValue.Int((int)myScaledPositionX));
            myOscMessage.AddValue(OSCValue.Int((int)myScaledPositionX));

            // Send message
            oscTransmitter.Send(myOscMessage);
        }
      
    }
}
