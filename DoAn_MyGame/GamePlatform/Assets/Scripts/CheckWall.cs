using UnityEngine;

public class CheckWall : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;

    public bool CheckHitWall()
    {
        if (Physics2D.OverlapCircle(this.transform.position, 0.2f, obstacleLayer))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.2f);
    }
}
