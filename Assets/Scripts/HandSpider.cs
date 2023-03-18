using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpider : MonoBehaviour
{
    public GameObject spider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnSpider(){
        Instantiate(spider, transform.position + new Vector3(0f, 0.015f, -0.05f), transform.rotation, transform);
    }
}
