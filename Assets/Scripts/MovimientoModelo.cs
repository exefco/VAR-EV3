using UnityEngine;

public class AnimacionModelo : MonoBehaviour
{
    [Header("Rotación")]
    public bool rotar = true;
    public float velocidadRotacion = 30f;
    public Vector3 ejeRotacion = Vector3.up;

    [Header("Flotación")]
    public bool flotar = true;
    public float amplitud = 0.05f;
    public float velocidadFlotacion = 2f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        if (rotar)
            transform.Rotate(ejeRotacion, velocidadRotacion * Time.deltaTime, Space.Self);

        if (flotar)
        {
            float y = Mathf.Sin(Time.time * velocidadFlotacion) * amplitud;
            transform.localPosition = posicionInicial + new Vector3(0, y, 0);
        }
    }
}