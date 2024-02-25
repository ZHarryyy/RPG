using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float xOffset;
        if (mousePosition.x > Screen.width * .5f) xOffset = Screen.width * .2f * -1f;
        else xOffset = Screen.width * .2f;

        float yOffset;
        if (mousePosition.y > Screen.height * .5f) yOffset = Screen.height * .1f * -1;
        else yOffset = Screen.height * .1f;

        transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }
}
