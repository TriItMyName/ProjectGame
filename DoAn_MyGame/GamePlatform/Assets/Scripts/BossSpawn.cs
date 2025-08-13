using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public GameObject defaultPositionObject;

    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasSpawned && collision.CompareTag("Player"))
        {
            GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

            BossController bossController = boss.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.defaultPositionObject = defaultPositionObject;
            }

            hasSpawned = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
