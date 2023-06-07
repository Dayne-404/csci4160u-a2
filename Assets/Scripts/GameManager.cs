using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private static PlayerData data;
    public Material baseMaterial;
    public GameObject accessory;

    public PlayerData PlayerData {
        get { return data; }
        set { 
            if(value != null && value != data) {
                SaveSystem.SaveData(value);
                data = value;
            }
        }
    }

    private void Awake()
    {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        LoadData();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.X)) {
            Debug.Log(data.name);
            Debug.Log(data.accessoryMaterial);
            Debug.Log(baseMaterial);
        }
    }

    public void LoadData() {
        data = SaveSystem.LoadData();
        Debug.Log("Loaded\n----------------------------------------------------------------------------------");
        Debug.Log("Name: " + data.name);
        Debug.Log("Base Material: " + data.baseMaterial);
        Debug.Log("Accessory: " + data.accessoryMaterial);
        Debug.Log("\n----------------------------------------------------------------------------------");
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
