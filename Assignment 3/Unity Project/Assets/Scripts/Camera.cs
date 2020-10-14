using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform playerTransform;
    private Transform transform;
    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(playerTransform.position.x < -3 || playerTransform.position.x > 3))
        {
            pos.x = playerTransform.position.x;
            transform.position = pos;
        }
    }
}
