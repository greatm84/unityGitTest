using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manmove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            transform.position += new Vector3(0.01f, 0);
        }else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.position += new Vector3(-0.01f, 0);
        }
    }
}
