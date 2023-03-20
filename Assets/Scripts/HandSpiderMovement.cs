using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandSpiderMovement : MonoBehaviour
{
    private InputData _inputData;
    private Animator mAnimator;

    [SerializeField]
    public float speed = 1.0f;

    private Vector3 target;
    private bool move;
    private int counter;
    private bool reachedDestination;
    private int waitTime;
    private bool groundSpider;
    public Transform spiderPrefab;
    public GameObject controller;

    public GameController gameController;

    private Transform parent;

    private float selfSize;
    // Start is called before the first frame update
    void Start()
    {
        _inputData = GetComponent<InputData>();
        mAnimator = GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //parent = GameObject.FindWithTag("Right Hand").transform;
        //transform.SetParent(parent);
        selfSize = gameController.spiderSize * 0.05f;
        transform.localScale = new Vector3(selfSize,selfSize,selfSize);
        transform.Rotate(-2f, 0f, 0f, Space.Self);
        target = new Vector3(Random.Range(-0.03f,0f), 0.015f, Random.Range(-0.05f,-0.16f));

        move = true;
        speed = Random.Range(0.05f,0.1f);
        counter = 0;
        reachedDestination = false;
        waitTime = Random.Range(150,300);

        selfSize = 0.2f * 0.05f;
        transform.localScale = new Vector3(selfSize,selfSize,selfSize);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Left Hand" || other.gameObject.tag == "Grabbables")
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        mAnimator.Play("die");

        yield return new WaitForSeconds(0.46f);

        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        selfSize = gameController.spiderSize * 0.05f;
        transform.localScale = new Vector3(selfSize,selfSize,selfSize);

        if(move){
            
            if(!reachedDestination){
                mAnimator.SetTrigger("Start Walking");
                 // fast rotation
                float rotSpeed = 360f; 
                
                // distance between target and the actual rotating object
                Vector3 D = target - transform.localPosition;  
                
                
                // calculate the Quaternion for the rotation
                Quaternion rot = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(D), rotSpeed * Time.deltaTime);
                
                //Apply the rotation 
                transform.localRotation = rot; 
                
                // put 0 on the axys you do not want for the rotation object to rotate
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z); 
                //transform.LookAt(new Vector3(transform.position.x + target.x, transform.position.y, transform.position.z + target.z));
                var step = speed * Time.deltaTime; // calculate distance to move
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, step);

                //Vibrate the controller
                _inputData._rightController.SendHapticImpulse(0u, 0.3f, 0.1f);
            }
            
            if( Vector3.Distance(transform.localPosition, target) < 0.001f){
                
                reachedDestination = true;
                target = new Vector3(Random.Range(-0.03f,0f), 0.015f, Random.Range(-0.05f,-0.16f));
                Debug.DrawRay(transform.localPosition, new Vector3(target.x, transform.position.y, target.z), Color.red);
                speed = Random.Range(0.05f,0.1f);
            }

        }
        
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool valueLeft))
        {
            if(valueLeft){
                
                move = true;
            }
        }
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool valueRight))
        {
            if(valueRight){
                mAnimator.SetTrigger("Stop Walking");
                move = false;
            }
        }
        if(reachedDestination){
            mAnimator.SetTrigger("Stop Walking");
            counter ++;
            if(counter > waitTime){
                reachedDestination = false;
                counter = 0;
            }
                
        }
    }
}
