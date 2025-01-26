using System;
using UnityEngine;
public class PlayerSpawn : MonoBehaviour
{
    public Vector3 SpawnPoint;
    private void Start()
    {
        var tmp_restartData = FindAnyObjectByType<RestartData>();
        if (tmp_restartData != null)
        {
            SpawnPoint = tmp_restartData.SpawnPoint;
            FindFirstObjectByType<InGameTimer>().InGameTime = tmp_restartData.TimerCurrent;
            Destroy(tmp_restartData.gameObject);
            transform.position = SpawnPoint;
            Camera.main.transform.position = transform.position + new Vector3(0, 0.8f, -15f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            SpawnPoint = other.GetComponent<CheckPoint>().NextPosition.position;
        }
    }
}
