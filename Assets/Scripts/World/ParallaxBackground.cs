using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float xParallaxEffect;
    [SerializeField] private float yParallaxEffect = 1;

    private float xPosition;
    private float yPosition;
    private float xlength;

    [SerializeField] private float minXPosition;
    [SerializeField] private float maxXPosition;
    [SerializeField] private float minYPosition;
    [SerializeField] private float maxYPosition;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");

        xlength = GetComponent<SpriteRenderer>().bounds.size.x;

        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    private void Update()
    {
        float distanceXMoved = cam.transform.position.x * (1 - xParallaxEffect);
        float distanceXToMove = cam.transform.position.x * xParallaxEffect;

        float distanceYToMove = cam.transform.position.y * yParallaxEffect;

        float targetXPosition = xPosition + distanceXToMove;
        float targetYPosition = yPosition + distanceYToMove;

        targetXPosition = Mathf.Clamp(targetXPosition, minXPosition, maxXPosition);
        targetYPosition = Mathf.Clamp(targetYPosition, minYPosition, maxYPosition);

        transform.position = new Vector3(targetXPosition, targetYPosition);

        if (distanceXMoved > xPosition + xlength) xPosition = xPosition + xlength;
        else if (distanceXMoved < xPosition - xlength) xPosition = xPosition - xlength;
    }
}
