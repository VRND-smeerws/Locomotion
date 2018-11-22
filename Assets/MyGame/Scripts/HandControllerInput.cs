using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerInput : MonoBehaviour {

    private const int laserRange = 15;

    public static float yNugeAmount = 1f; //specific to teleportAimerObject height
    private static readonly Vector3 yNudgeVector = new Vector3(0f, yNugeAmount, 0f);

    public SteamVR_TrackedObject tracktobj;
    public SteamVR_Controller.Device device;

    //Teleporter
    private LineRenderer laser;
    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    public LayerMask laserMask;

    

	// Use this for initialization
	void Start ()
    {
        tracktobj = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        device = SteamVR_Controller.Input((int)tracktobj.index);

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            laser.gameObject.SetActive(true);
            teleportAimerObject.SetActive(true);

            laser.SetPosition(0, gameObject.transform.position);
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, laserRange, laserMask))
            {
                teleportLocation = hit.point;
                laser.SetPosition(1, teleportLocation);

                //aimer postition
                teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNugeAmount, teleportLocation.z);
            }
            else
            {
                teleportLocation = new Vector3(
                    x: transform.forward.x * laserRange + transform.position.x,
                    y: transform.forward.y * laserRange + transform.position.y,
                    z: transform.forward.z * laserRange + transform.position.z);

                RaycastHit groundRay;
                if(Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17, laserMask))
                {
                    teleportLocation = new Vector3(
                         x: transform.forward.x * laserRange + transform.position.x,
                         y: groundRay.point.y,
                         z: transform.forward.z * laserRange + transform.position.z);
                }

                laser.SetPosition(1, transform.forward * laserRange + transform.position);
                //aimer position
                teleportAimerObject.transform.position = teleportLocation + yNudgeVector;
            }
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            laser.gameObject.SetActive(false);
            teleportAimerObject.SetActive(false);
            player.transform.position = teleportLocation;
            
        }
	}
}
