using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    public float mouseScroll = 3.0f;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 horizonal = cam.transform.right * Input.GetAxis("Mouse X") * mouseScroll;
            Vector3 vertical = cam.transform.up * Input.GetAxis("Mouse Y") * mouseScroll;
            cam.transform.position = cam.transform.position + horizonal + vertical;
        }

        if (Input.GetMouseButton(2))
        {
            float rotation = Input.GetAxis("Mouse X") * mouseScroll;
            cam.transform.RotateAround(cam.transform.position, Vector3.up, rotation);
            rotation = -Input.GetAxis("Mouse Y") * mouseScroll;
            cam.transform.RotateAround(cam.transform.position, cam.transform.right, rotation);
        }

        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            cam.transform.position = cam.transform.position + cam.transform.forward * Input.mouseScrollDelta.y;
        }

    }
}
