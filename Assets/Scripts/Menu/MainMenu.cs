using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    private Customization customization;
    private bool hasCustomization;
    private GameManager gameManager;

    [Header("Menu Navigation")]
    [SerializeField] private GameObject[] menus;

    [Header("Main Buttons")]
    [SerializeField] private Button[] buttons;

    [Header("Text")]
    [SerializeField] private TMP_Text accessoryText;
    [SerializeField] private TMP_Text baseColourText;
    [SerializeField] private TMP_Text nameText;

    string newPlayerName;

    public string NewPlayerName { 
        get { return newPlayerName; }
        set {
            newPlayerName = value;
            nameText.text = newPlayerName;
        }
    }

    private void Awake() {
        if (this.TryGetComponent<Customization>(out customization)) hasCustomization = true;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start() {
        if (hasCustomization) baseColourText.text = customization.getCurrentBaseMaterial();
        else baseColourText.text = "Base";

        if (hasCustomization) accessoryText.text = customization.getAccessoryName();
        else accessoryText.text = "None";

        ActivateAllObjects(menus);
        buttons = GameObject.FindObjectsOfType<Button>();
        

        foreach (Button button in buttons) {
            AssignFunction(button);
        }

        ResetMenu(0);
        if(nameText != null) SetNameText();
    }

    public void AssignFunction(Button button) {
        string tag = button.tag;

        if (tag == "Play") button.onClick.AddListener(delegate { ResetMenu(4); });
        else if (tag == "Options") button.onClick.AddListener(delegate { ResetMenu(0, 3); });
        else if (tag == "Customization") button.onClick.AddListener(delegate { ResetMenu(1); });
        else if (tag == "Exit") button.onClick.AddListener(gameManager.QuitGame);
        else if (tag == "About") button.onClick.AddListener(delegate { ResetMenu(0, 2); });
        else if (tag == "Back") button.onClick.AddListener(delegate { ResetMenu(0); });
        else if (tag == "Save") button.onClick.AddListener(SaveCustomization);
        else if (tag == "Random") button.onClick.AddListener(delegate { GotoScene("Level"); });
        else if (tag == "Tutorial") button.onClick.AddListener(delegate { GotoScene("Tutorial"); });

        else if (tag == "Accessory" && hasCustomization) {
            if (button.name == "Next_Button")
                button.onClick.AddListener(pressedNextAccessory);
            else if (button.name == "Previous_Button")
                button.onClick.AddListener(pressedPreviousAccessory);
        } else if (tag == "Base" && hasCustomization) {
            if (button.name == "Next_Button")
                button.onClick.AddListener(pressedRight);
            else if (button.name == "Previous_Button")
                button.onClick.AddListener(pressedLeft);
        } 
    }

    public void SetNameText() {
        nameText.text = gameManager.PlayerData.name;
    }

    private void ActivateAllObjects(GameObject[] objects) {
        foreach(GameObject obj in objects) {
            obj.SetActive(true);
        }
    }

    public void SaveCustomization() {
        PlayerData data = new PlayerData(newPlayerName, customization.getCurrentBaseMaterial(), customization.getAccessoryName());
        gameManager.baseMaterial = customization.getBaseMaterial();
        gameManager.PlayerData = data;
        Debug.Log("Saved");

        Debug.Log(gameManager.PlayerData.ToString());
    }

    public void pressedNextAccessory() {
        customization.gotoNextAccessory();
        accessoryText.text = customization.getAccessoryName();
    }

    public void pressedPreviousAccessory() {
        customization.gotoPreviousAccessory();
        accessoryText.text = customization.getAccessoryName();
    }

    public void pressedLeft() {
        customization.gotoPreviousMaterial();
        baseColourText.text = customization.getCurrentBaseMaterial();
    }

    public void pressedRight() {
        customization.gotoNextMaterial();
        baseColourText.text = customization.getCurrentBaseMaterial();
    }

    public void GotoScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void ResetMenu(int activeIndex, int secondaryIndex = -1) {
        foreach (GameObject menu in menus) {
            menu.SetActive(false);
        }

        if(activeIndex >= menus.Length) activeIndex = 0;   
        if(secondaryIndex != -1) menus[secondaryIndex].SetActive(true);
   
        menus[activeIndex].SetActive(true);
    }
}
