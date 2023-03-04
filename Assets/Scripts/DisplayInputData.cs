using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

[RequireComponent(typeof(InputData))]
public class DisplayInputData : MonoBehaviour
{

    private InputData _inputData;
    private Animator mAnimator;


    private void Start()
    {
        _inputData = GetComponent<InputData>();
        mAnimator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool valueLeft))
        {
            if(valueLeft)
                mAnimator.SetTrigger("Start Walking");
        }
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool valueRight))
        {
            if(valueRight)
                mAnimator.SetTrigger("Stop Walking");
        }
    }
}
