using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform ArrowPos;
    [SerializeField] private GameObject[] Arrows;

    private float cooldownTimer;
    private void Attack()
    {
        cooldownTimer = 0;

        Arrows[FindArrow()].transform.position = ArrowPos.position;

        Arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown) 
        {
            Attack();
        }
            
    }

    private int FindArrow()
    {
        for (int i = 0; i < Arrows.Length; i++)
        {
            if (!Arrows[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
