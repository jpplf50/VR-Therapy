using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class SpiderBehaviour : MonoBehaviour
{
    private Animator mAnimator;


    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mAnimator != null){
            //System.Console.WriteLine(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));
            if(Input.GetKeyDown(KeyCode.RightArrow))
            //if(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) == 1.0)
                mAnimator.SetTrigger("Start Walking");
            //if(OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) == 1.0)
            if(Input.GetKeyDown(KeyCode.LeftArrow))
                mAnimator.SetTrigger("Stop Walking");
        }
    }
}
