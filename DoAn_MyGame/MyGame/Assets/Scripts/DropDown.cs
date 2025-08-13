using UnityEngine;

public class UpDown : MonoBehaviour
{
    private string NamePlayer = "Player";
    private string oneWayPlatform = "OneWayPlatForm";

    private void Update()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(NamePlayer), LayerMask.NameToLayer(oneWayPlatform), true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(NamePlayer), LayerMask.NameToLayer(oneWayPlatform), false);
        }
    }

}
