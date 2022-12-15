using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{

    [SerializeField] public Transform cameraTarget;
    [SerializeField] public float sSpeed = 10.0f;
    [SerializeField] public Vector3 dist;
    [SerializeField] public Transform lookTarget;

    void FixedUpdate()
    {
        Vector3 dPos = cameraTarget.position + dist;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, sSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position);
    }

}
