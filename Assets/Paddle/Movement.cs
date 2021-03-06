﻿using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField, Range(0, 360f)]
    private float rotation;

    [SerializeField, Range(0, 360f)]
    private float rotateSpeed = 40f;

    [SerializeField, Range(0, 360f)]
    private float maxAcceleration = 40f;

    private float velocity;

    private void OnValidate()
    {
        setRotation();
    }

    private void setRotation()
    {
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }

    private void Awake()
    {
        setRotation();
    }

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, velocity);
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
