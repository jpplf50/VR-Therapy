using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class GameController : MonoBehaviour
{

    private InputData _inputData;
    public GameObject spider;

    private bool spawned;

    // Start is called before the first frame update
    void Start()
    {
        _inputData = GetComponent<InputData>();
        spawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool valueGrip))
        {
            if(valueGrip && spawned == false){
                Instantiate(spider, new Vector3(Random.Range(-1.0f,1.0f), 0.75f, Random.Range(0.5f,1.2f)), Quaternion.Euler(0,180,0));
                spawned = true;
            }
            else if(!valueGrip)
                spawned = false;
        }
        else{
            spawned = false;
            
        }
    }
}
