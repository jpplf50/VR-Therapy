using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpiderBehaviour : MonoBehaviour
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

    public GameController gameController;

    private float selfSize;


    // Start is called before the first frame update
    void Start()
    {
        _inputData = GetComponent<InputData>();
        mAnimator = GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        selfSize = gameController.spiderSize * 0.05f;
        transform.localScale = new Vector3(selfSize,selfSize,selfSize);

        if(transform.position.y < 0.2f){
            switch(gameController.currentLevel){
                case 1:
                    target = new Vector3(Random.Range(-5.0f,5.3f), 0.0f, Random.Range(-1.7f,4f));
                break;
                case 2:
                    target = new Vector3(Random.Range(15.0f,20.3f), 0.0f, Random.Range(-1.7f,4f));
                break;
                default:
                break;
            }
            groundSpider = true;
        }
            
        else{
            target = new Vector3(Random.Range(-1.0f,1.0f), 0.75f, Random.Range(0.5f,1.2f));
            groundSpider = false;
        }
        
        move = true;
        speed = Random.Range(0.25f,0.6f);
        counter = 0;
        reachedDestination = false;
        waitTime = Random.Range(150,300);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Left Hand" || other.gameObject.tag == "Right Hand")
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        mAnimator.Play("die");

        yield return new WaitForSeconds(0.46f);
        if(transform.position.y < 0.2f){
            gameController.floorSpiders -= 1;
            gameController.updateText(gameController.tableSpiders, gameController.floorSpiders, gameController.spiderInformation);
        }
        else{
            gameController.tableSpiders -= 1;
            gameController.updateText(gameController.tableSpiders, gameController.floorSpiders, gameController.spiderInformation);
        }

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
                transform.LookAt(target);
                var step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target, step);
            }
            
            if( Vector3.Distance(transform.position, target) < 0.001f){
                
                reachedDestination = true;
                if(groundSpider)
                    switch(gameController.currentLevel){
                        case 1:
                            target = new Vector3(Random.Range(-5.0f,5.3f), 0.0f, Random.Range(-1.7f,4f));
                        break;
                        case 2:
                            target = new Vector3(Random.Range(15.0f,20.3f), 0.0f, Random.Range(-1.7f,4f));
                        break;
                        default:
                        break;
                    }
                else
                    target = new Vector3(Random.Range(-1.0f,1.0f), 0.75f, Random.Range(0.5f,1.2f));
                speed = Random.Range(0.25f,0.6f);
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
