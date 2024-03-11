using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Vector3 currentPosition = this.transform.position;

        currentPosition.x = cameraTransform.position.x;

        this.transform.position = currentPosition;
    }
}
