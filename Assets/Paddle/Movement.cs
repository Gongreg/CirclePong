using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField, Range(0, 10f)]
    private float distanceFromCenter;

    [SerializeField, Range(0, 360f)]
    private float rotation;

    [SerializeField, Range(0, 360f)]
    private float rotateSpeed = 40f;

    [SerializeField, Range(0, 360f)]
    private float maxAcceleration = 40f;
    private float velocity;


    private Transform paddleCollider;

    private void OnValidate()
    {
        setDistanceFromCenter();

        transform.eulerAngles = new Vector3(0, 0, rotation);
    }

    private void GetReferenceToPaddleCollider()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();

        foreach (Transform t in transforms)
        {
            if (t.gameObject.name == "PaddleCollider")
            {
                paddleCollider = t;
            }
        }

    }

    private void Awake()
    {
        OnValidate();
    }

    private void setDistanceFromCenter()
    {
        if (!paddleCollider)
        {
            GetReferenceToPaddleCollider();
        }

        paddleCollider.localPosition = new Vector3(paddleCollider.localPosition.x, 0.5f * distanceFromCenter, paddleCollider.localPosition.z);
    }

    private void Update()
    {

        float desiredVelocity = -Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;

        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        velocity = Mathf.MoveTowards(velocity, desiredVelocity, maxSpeedChange);

        transform.Rotate(0f, 0f, velocity);

        rotation = transform.eulerAngles.z;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 48;
        style.normal.textColor = Color.white;


        GUI.Label(new Rect(10, 25, 0, 0), "InputAxis: " + -Input.GetAxis("Horizontal"), style);
        GUI.Label(new Rect(10, 75, 0, 0), "Velocity: " + (velocity / Time.deltaTime), style);
    }
}
