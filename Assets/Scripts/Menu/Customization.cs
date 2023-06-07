using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Customization : MonoBehaviour
{
    [SerializeField] private string matDirectory = "Materials/Colors";
    [SerializeField] private string accDirectory = "Prefabs";
    [SerializeField] private Transform accessoryPos;
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject[] accessories;
    [SerializeField] private TMP_InputField nameInputField;

    [SerializeField] private GameObject model;
    private GameManager gameManager;

    private Renderer baseRenderer;

    public int currentMat;
    public int currentAcc;


    public bool hasBase = false;

    private GameObject nextAccessory;


    private void Awake() {
        if (model.TryGetComponent<Renderer>(out baseRenderer)) hasBase = true;
        gameManager = FindObjectOfType<GameManager>();

        materials = Resources.LoadAll<Material>(matDirectory);
        accessories = Resources.LoadAll<GameObject>(accDirectory);

        currentMat = 0;
        currentAcc = 0;

        if (hasBase && materials.Length != 0)
            setModelMaterial(gameManager.PlayerData.baseMaterial);

        if (accessories.Length != 0)
            setAccessory(gameManager.PlayerData.accessoryMaterial);

        nameInputField.text = gameManager.PlayerData.name;
    }

    private void Start() {
        
    }

    public void setAccessory(string accessoryName) {
        foreach (GameObject obj in accessories) {
            if (accessoryName == obj.name) {
                gameManager.accessory = obj;
                spawnAccessory(obj);
                return;
            } else if(accessoryName == "None") {
                currentAcc = -1;
                gameManager.accessory = null;
                return;
            } else {
                currentAcc++;
            }
            
        }
    }

    public void spawnAccessory(GameObject obj) {
        if (nextAccessory != null) { Destroy(nextAccessory); }
        nextAccessory = Instantiate(accessories[currentAcc], accessoryPos.position, Quaternion.identity);
        nextAccessory.transform.rotation = accessoryPos.rotation;
        nextAccessory.transform.parent = accessoryPos.parent;
    }

    //Works as Intended
    public void setModelMaterial(string materialName) {
        foreach( Material material in materials) {
            if (materialName == material.name) {
                gameManager.baseMaterial = material;
                baseRenderer.material = material;
                return;
            } else {
                currentMat++;
            }
            
        }

        currentMat = 0;
    }

    public Material getBaseMaterial() {
        return baseRenderer.material;
    }

    public string getCurrentBaseMaterial() {
        return baseRenderer.material.ToString().Split(' ')[0];
    }

    public GameObject getAccessory() {
        return accessories[currentAcc];
    }

    public string getAccessoryName() {
        if(currentAcc >= 0)
            return accessories[currentAcc].name;
    
        return "None";
    }

    public void gotoNextAccessory() {
        if(accessories.Length != 0) {
            if(currentAcc < accessories.Length - 1) currentAcc++;
            else currentAcc = -1;
        }

        if(nextAccessory != null) { Destroy(nextAccessory); }
        if (currentAcc != -1) {
            nextAccessory = Instantiate(accessories[currentAcc], accessoryPos.position, Quaternion.identity);
            nextAccessory.transform.rotation = accessoryPos.rotation;
            nextAccessory.transform.parent = accessoryPos.parent;
        }
    }

    public void gotoPreviousAccessory() {
        if (accessories.Length != 0) {
            if (currentAcc > -1) currentAcc--;
            else currentAcc = accessories.Length - 1;
        }

        if (nextAccessory != null) { Destroy(nextAccessory); }
        if (currentAcc != -1) {
            nextAccessory = Instantiate(accessories[currentAcc], accessoryPos.position, Quaternion.identity);
            nextAccessory.transform.rotation = accessoryPos.rotation;
            nextAccessory.transform.parent = accessoryPos.parent;
        }
    }

    public void gotoNextMaterial() {
        if (hasBase && materials.Length != 0) {
            if (currentMat < materials.Length - 1) currentMat++;
            else currentMat = 0;

            baseRenderer.material = materials[currentMat];
        }
    }

    public void gotoPreviousMaterial() {
        if (hasBase && materials.Length != 0) {
            if (currentMat > 0) currentMat--;
            else currentMat = materials.Length - 1;

            baseRenderer.material = materials[currentMat];
        } 
    }
}
