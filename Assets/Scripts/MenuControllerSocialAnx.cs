using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllerSocialAnx : MonoBehaviour
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
        
        
        if (dropDownText == "Classroom"){
            
            xrRig.transform.position = new Vector3(41.17f,0.29f,-1.41f);
            gameController.currentLevel = 6;
        }
        else if(dropDownText == "Theater"){
            xrRig.transform.position = new Vector3(75.98f,3.03f,3.67f);
            xrRig.transform.rotation = Quaternion.Euler(0, -90, 0);
            gameController.currentLevel = 7;
        }
        
        
    }
}
