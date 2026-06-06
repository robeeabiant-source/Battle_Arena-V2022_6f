using System;
using UnityEngine;

public class BillboardFaceCamera : MonoBehaviour
{
    [SerializeField] BillboardMethod billboardMethod = BillboardMethod.Limited;
    Camera MainCamera;

    [SerializeField] bool OverrideCamera = false;
    [SerializeField] Camera TargetCamera;

    public float smoothSpeed = 5f;

    public float minRotationY = -5f;
    public float maxRotationY = 5f;

    public bool useXRotaion = true;
    public float minRotationX = -5f;
    public float maxRotationX = 5f;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;

        if(OverrideCamera)
        {
            MainCamera = TargetCamera;
        }
    }

    void LateUpdate()
    {
        if (MainCamera == null)
        {
            return;
        }
        
        if (billboardMethod == BillboardMethod.Full)
        {
            transform.forward = MainCamera.transform.forward;
        }
        else if(billboardMethod == BillboardMethod.Limited)
        {          
            Vector3 targetPosition = MainCamera.transform.position;
            targetPosition.y = transform.position.y;

            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            float targetY = NormalizeAngle(targetRotation.eulerAngles.y);
            targetY = Mathf.Clamp(targetY, minRotationY, maxRotationY);

            float targetX = transform.rotation.eulerAngles.x;

            if (useXRotaion)
            {
                targetX = NormalizeAngle(targetRotation.eulerAngles.x);
                targetX = Mathf.Clamp(targetX, minRotationX, maxRotationX);
            }

            Quaternion clampedRotation = Quaternion.Euler(targetX, -targetY, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, clampedRotation, Time.deltaTime * smoothSpeed);         
        }
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}

[Serializable]
public enum BillboardMethod
{
    Limited,
    Full,
}
