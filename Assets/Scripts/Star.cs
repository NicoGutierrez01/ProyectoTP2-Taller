// Star.cs
using UnityEngine;

public class Star : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SumarEstrella();
            Destroy(gameObject);
        }
    }
}
