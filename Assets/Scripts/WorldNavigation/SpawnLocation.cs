using System;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    public string SpawnName;
    public event Action OnSpawn;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.limeGreen;
        Gizmos.DrawWireSphere(transform.position, 1f);
        Gizmos.DrawRay(transform.position, transform.forward);
    }

}
