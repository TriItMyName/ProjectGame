using UnityEngine;

public class CheckOnPlatform : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    public bool CheckGround()
    {
        if (Physics2D.OverlapCircle(this.transform.position, 0.2f,obstacleLayer)!=null) 
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
