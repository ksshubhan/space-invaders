using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2D : MonoBehaviour
{

    public float speed = 10.4f;

    public GameObject topRightLimitGameObject;
    public GameObject bottomLeftLimitGameObject;

    private Vector3 topRightLimit;
    private Vector3 bottomLeftLimit;

    private Vector2 input;

    void Start()
    {
        topRightLimit = topRightLimitGameObject.transform.position;
        bottomLeftLimit = bottomLeftLimitGameObject.transform.position;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        input = new Vector2(horizontalInput, verticalInput);
        
        
        if ((transform.position.x <= bottomLeftLimit.x && input.x < 0) || (transform.position.x >= topRightLimit.x && input.x > 0))
        {
            input.x = 0;
        }

        Vector3 translate = new Vector3(input.x, 0, input.y);
        transform.Translate(translate * speed * Time.deltaTime);
    }    
}
