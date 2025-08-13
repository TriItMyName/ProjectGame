using System.Collections;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    private string NamePlayer = "Player";
    private string oneWayPlatform = "OneWayPlatForm";
    private bool isDropping = false;

    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && !isDropping)
        {
            StartCoroutine(DropThroughPlatform());
        }
    }

    private IEnumerator DropThroughPlatform()
    {
        isDropping = true;

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer(NamePlayer),
            LayerMask.NameToLayer(oneWayPlatform),
            true
        );

        yield return new WaitForSeconds(0.5f);

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer(NamePlayer),
            LayerMask.NameToLayer(oneWayPlatform),
            false
        );

        isDropping = false;
    }
}
