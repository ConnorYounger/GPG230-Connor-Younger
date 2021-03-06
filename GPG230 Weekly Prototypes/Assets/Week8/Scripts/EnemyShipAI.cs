using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    public Transform travelPath;
    public float targetDistance = 5;
    public float movementSpeed = 10;
    public float turnSpeed = 10;

    private List<Transform> travelPoints;
    private int travelIndex;
    private int previousTravelIndex;

    public bool isTriggered;
    public float triggerDistance = 300;

    private Transform player;

    public List<ShipWeapon> weapons;


    void Start()
    {
        travelPoints = new List<Transform>();
        weapons = new List<ShipWeapon>();

        if(travelPath != null)
            SetTravelPaths(travelPath);

        player = GameObject.Find("Player").transform;

        previousTravelIndex = travelPoints.Count - 1;
    }

    public void SetTravelPaths(Transform trans)
    {
        travelPath = trans;
        travelPoints = new List<Transform>();

        foreach (Transform t in travelPath)
        {
            travelPoints.Add(t);
        }
    }

    void Update()
    {
        if(travelPoints.Count > 0)
            ShipMovement();

        if (!isTriggered)
            EnemyTriggerCheck();
    }

    void EnemyTriggerCheck()
    {
        if (Vector3.Distance(transform.position, player.position) < triggerDistance)
        {
            isTriggered = true;
        }
    }

    void ShipMovement()
    {
        if(Vector3.Distance(transform.position, travelPoints[travelIndex].position) > targetDistance)
        {
            //transform.position = Vector3.Slerp(transform.position, travelPoints[travelIndex].position, movementSpeed * Time.deltaTime);

            ShipTurning();
        }
        else
        {
            travelIndex++;
            previousTravelIndex++;

            if(travelIndex >= travelPoints.Count)
            {
                travelIndex = 0;
            }

            if (previousTravelIndex >= travelPoints.Count)
            {
                previousTravelIndex = 0;
            }
        }

        if(Vector3.Distance(transform.position, travelPoints[travelIndex].position) > 1000)
        {
            transform.position = travelPoints[travelIndex].position;
        }
    }

    void ShipTurning()
    {
        Vector3 targetDir = travelPoints[travelIndex].position - transform.position;
        targetDir = targetDir.normalized;

        Vector3 currentDir = transform.forward;

        currentDir = Vector3.RotateTowards(currentDir, targetDir, turnSpeed * Time.deltaTime, 1.0f);

        Quaternion qDir = new Quaternion();
        qDir.SetLookRotation(currentDir, Vector3.up);
        transform.rotation = qDir;

        transform.Translate(transform.right * movementSpeed * Time.deltaTime);
    }
}
