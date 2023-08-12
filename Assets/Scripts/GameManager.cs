using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string selectedVehicle;

    private GameObject[] vehicles;

    private void Awake()
    {
        vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        foreach (GameObject vehicle in vehicles) 
        {
            if (vehicle.name == selectedVehicle) 
            {
                vehicle.SetActive(true);
            }
            else
            {
                vehicle.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            SceneManager.LoadScene(0);
        }
        Debug.Log(selectedVehicle + " " + vehicles.Length);
    }
}
