using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleButton : MonoBehaviour
{
    private Button button;
    private MenuManager menuManager;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        MenuManager.Instance.StartGame(gameObject.name);
    }
}
