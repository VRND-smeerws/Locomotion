using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerInput : MonoBehaviour {

    public SteamVR_TrackedObject tracktobj;
    public SteamVR_Controller.Device device;

	// Use this for initialization
	void Start ()
    {
        tracktobj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        device = SteamVR_Controller.Input((int)tracktobj.index);

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {

        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {

        }
	}
}
