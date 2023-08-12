using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject targetVehicle;
    private Vector3 offset;

    [SerializeField] private float cameraSpeed;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject vehicle in GameObject.FindGameObjectsWithTag("Vehicle"))
        {
            if (vehicle.name == GameManager.selectedVehicle) 
            { 
                targetVehicle = vehicle;
            }
        }

        offset = targetVehicle.transform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetDirection = (targetVehicle.transform.position - transform.position).normalized;
        Quaternion wantedRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, cameraSpeed * Time.deltaTime);

        Vector3 wantedPos = targetVehicle.transform.position - targetVehicle.transform.forward * offset.z - targetVehicle.transform.up * offset.y;
        transform.position = Vector3.Slerp(transform.position, wantedPos, cameraSpeed * Time.deltaTime);
    }
}
