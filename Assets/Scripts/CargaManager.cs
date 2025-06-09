using UnityEngine;
using System.Collections.Generic;

public class CargaManager : MonoBehaviour
{
    public GameObject paquetePrefab;
    public Transform cargadorTransform;

    private List<GameObject> paquetes = new List<GameObject>();

    public void AgregarPaquete()
    {
        GameObject nuevoPaquete = Instantiate(paquetePrefab, cargadorTransform);
        float offsetY = paquetes.Count * 0.3f; // ajustá según tamaño
        nuevoPaquete.transform.localPosition = new Vector3(0f, offsetY, 0f);
        paquetes.Add(nuevoPaquete);
    }

    public void SoltarTodos()
    {
        foreach (GameObject paquete in paquetes)
        {
            paquete.transform.SetParent(null);
            var rb = paquete.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
        }
        paquetes.Clear();
    }

    public int CantidadPaquetes()
    {
        return paquetes.Count;
    }
}
