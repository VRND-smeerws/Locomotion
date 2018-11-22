using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusControllerInput : MonoBehaviour {

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

    //Dash
    public float dashSpeed = 0.1f;
    private bool isDashing;
    private float lerpTime;
    private Vector3 dashStartPosition;

    //Walking
    public Transform playerCam;
    public float moveSpeed = 4f;
    private Vector3 movementDirection;

    

	// Use this for initialization
	void Start ()
    {
        tracktobj = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //device = SteamVR_Controller.Input((int)tracktobj.index);

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            movementDirection = playerCam.transform.forward;
            movementDirection = new Vector3(movementDirection.x, 0, movementDirection.z);
            movementDirection = movementDirection * moveSpeed * Time.deltaTime;
            player.transform.position += movementDirection;
        }

        if (isDashing)
        {
            lerpTime += Time.deltaTime * dashSpeed;
            player.transform.position = Vector3.Lerp(dashStartPosition, teleportLocation, lerpTime);
            if(lerpTime >= 1)
            {
                isDashing = false;
                lerpTime = 0;
            }
        }
        else
        {
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                laser.gameObject.SetActive(true);
                teleportAimerObject.SetActive(true);

                laser.SetPosition(0, gameObject.transform.position);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, laserRange, laserMask))
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
                    if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17, laserMask))
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
            if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                laser.gameObject.SetActive(false);
                teleportAimerObject.SetActive(false);
                //player.transform.position = teleportLocation;
                dashStartPosition = player.transform.position;
                isDashing = true;

            }
        }

        
	}
}
