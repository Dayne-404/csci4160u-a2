using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApplyCustomization : MonoBehaviour
{
    [SerializeField] private Renderer modelRenderer;
    [SerializeField] private Renderer jointsRenderer;
    [SerializeField] private Renderer surfaceRenderer;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Transform accessoryPos;
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Start() {
        if (gameManager != null) { 
            modelRenderer.material = gameManager.baseMaterial;
            jointsRenderer.material = gameManager.baseMaterial;
            surfaceRenderer.material = gameManager.baseMaterial;
            playerName.text = gameManager.PlayerData.name;
            attachAccessory();
        }
    }
    
    private void attachAccessory() {
        GameObject accessory = Instantiate(gameManager.accessory, accessoryPos.position, Quaternion.identity);
        accessory.transform.rotation = accessoryPos.rotation;
        accessory.transform.parent = accessoryPos.parent;
    }
}
