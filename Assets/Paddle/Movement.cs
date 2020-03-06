using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField, Range(0, 10f)]
    private float distanceFromCenter;

    [SerializeField, Range(0, 360f)]
    private float rotation;

    private Transform paddleCollider;

    // Start is called before the first frame update

    private void OnValidate()
    {
        setDistanceFromCenter();
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
        setDistanceFromCenter();
    }

    private void setDistanceFromCenter()
    {
        if (!paddleCollider)
        {
            GetReferenceToPaddleCollider();
        }

        paddleCollider.localPosition = (0.5f * distanceFromCenter) * transform.up;
    }

    // Update is called once per frame
    private void Update()
    {
        float rotateSpeed = 1f;
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
