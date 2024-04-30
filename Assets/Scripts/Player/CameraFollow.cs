using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.6F;
    private Vector3 _velocity = Vector3.zero*1;

    void FixedUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 10, 0));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }
}