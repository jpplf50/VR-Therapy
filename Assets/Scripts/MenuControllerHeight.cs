using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllerHeight : MonoBehaviour
{
    public Dropdown dropDown;
    public GameObject teleportButton;
    public GameObject xrRig;
    public GameController gameController;


    int dropDownIndex;
    string dropDownText;


    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        dropDownIndex = dropDown.value;
        dropDownText = dropDown.options[dropDownIndex].text;
        
        
        if (dropDownText == "Low Height"){
            
            xrRig.transform.position = new Vector3(-69.7f,31.4f,134.9f);
            gameController.currentLevel = 3;
        }
        else if(dropDownText == "Medium Height"){
            xrRig.transform.position = new Vector3(-36.3f,36.6f,134.9f);
            gameController.currentLevel = 4;
        }
        else if(dropDownText == "High Height"){
            xrRig.transform.position = new Vector3(-12.2f,55.4f,134.9f);
            gameController.currentLevel = 5;
        }
        
    }
}
