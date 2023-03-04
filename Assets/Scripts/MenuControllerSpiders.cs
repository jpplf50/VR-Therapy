using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllerSpiders : MonoBehaviour
{
    public Dropdown dropDown;
    public Slider slider;
    public Toggle toggle;
    public GameObject teleportButton;
    public GameObject xrRig;
    public GameController gameController;

    public Text spiderSizeText;


    int dropDownIndex;
    string dropDownText;


    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        ChangeSizeDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        dropDownIndex = dropDown.value;
        dropDownText = dropDown.options[dropDownIndex].text;
        gameController.spiderSize = slider.value;
        if (toggle.isOn)
            gameController.spiderToggle = true;
        else if(!toggle.isOn)
            gameController.spiderToggle = false;
        if (dropDownText == "Living Room"){
            
            xrRig.transform.position = new Vector3(0,0,-0.5f);
            gameController.currentLevel = 1;
        }
        else if(dropDownText == "Office"){
            xrRig.transform.position = new Vector3(20f,0,-0.5f);
            gameController.currentLevel = 2;
        }
    }

    public void ChangeSizeDisplay()
    {
        spiderSizeText.text = slider.value.ToString();
    }
}
