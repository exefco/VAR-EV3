using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System.Collections.Generic;

public class PokedexARController : MonoBehaviour
{
    [Header("AR Foundation")]
    public ARTrackedImageManager trackedImageManager;

    [Header("Contenido 3D")]
    public Transform arContentRoot; // Hijos: Pokeball(0), Lucario(1), Mew(2)

    [Header("Interfaz (Ficha 1)")]
    public GameObject panelFicha;
    public Image imgPokemon;                 // 1.2.1 Image
    public TextMeshProUGUI txtNombre;        // 1.2.2 Name
    public TextMeshProUGUI txtTipo;          // 1.2.3 Type
    public TextMeshProUGUI txtHP;            // 1.2.4 HP
    public TextMeshProUGUI txtHabilidad;     // 1.2.5 Skill
    public TextMeshProUGUI txtDescripcion;   // 1.2.6 Description
    public Button btnFicha1;                 // 1.3
    public Button btnFicha2;                 // 1.4
    public Button btnCerrar;                 // 1.5

    [Header("Datos de las Cartas")]
    public CartaData[] cartas;

    private CartaData cartaActual;
private Dictionary<ARTrackedImage, string> imagenesProcesadas = new Dictionary<ARTrackedImage, string>();

    void OnEnable()  => trackedImageManager.trackedImagesChanged += OnImagesChanged;
    void OnDisable() => trackedImageManager.trackedImagesChanged -= OnImagesChanged;

    void Start()
    {
        panelFicha.SetActive(false);
        OcultarModelos();

        btnFicha1.onClick.AddListener(() => MostrarFicha(0));
        btnFicha2.onClick.AddListener(() => MostrarFicha(1));
        btnCerrar.onClick.AddListener(CerrarTodo);
    }

    void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
    foreach (var img in args.added)   Procesar(img);
    foreach (var img in args.updated) Procesar(img);
    foreach (var img in args.removed) imagenesProcesadas.Remove(img);
    }

    void Procesar(ARTrackedImage img)
    {
    // Si la imagen ya no se rastrea, limpiar y salir
    if (img.trackingState != TrackingState.Tracking)
    {
        imagenesProcesadas.Remove(img);
        return;
    }

    string nombreTarget = img.referenceImage.name;

    if (imagenesProcesadas.ContainsKey(img) && imagenesProcesadas[img] == nombreTarget)
        return;

    imagenesProcesadas[img] = nombreTarget;

    cartaActual = BuscarCarta(nombreTarget);
    if (cartaActual == null) { return; } // No cerrar todo, solo ignorar

    arContentRoot.SetParent(img.transform, false);
    arContentRoot.localPosition = Vector3.zero;
    ActivarModelo(nombreTarget);

    panelFicha.SetActive(true);
    MostrarFicha(0);
    }

    void ActivarModelo(string nombreTarget)
    {
        OcultarModelos();
        int indice = -1;
        switch (nombreTarget.ToLower())
        {
        case "reverso":  indice = 0; break;
        case "lucario":  indice = 1; break;
        case "mew":      indice = 2; break;
        }
        if (indice >= 0 && indice < arContentRoot.childCount)
            arContentRoot.GetChild(indice).gameObject.SetActive(true);
    }

    void OcultarModelos()
    {
        foreach (Transform hijo in arContentRoot)
            hijo.gameObject.SetActive(false);
    }

    CartaData BuscarCarta(string nombre)
    {
        foreach (var c in cartas)
            if (c.nombreTarget.ToLower() == nombre.ToLower()) return c;
        return null;
    }

    public void MostrarFicha(int i)
    {
        if (cartaActual == null) return;

        FichaData datos = (i == 0) ? cartaActual.ficha1 : cartaActual.ficha2;

        txtNombre.text      = datos.nombre;
        txtTipo.text        = "Tipo: " + datos.tipo;
        txtHP.text          = "HP: " + datos.hp;
        txtHabilidad.text   = "Habilidad: " + datos.habilidad;
        txtDescripcion.text = datos.descripcion;

        // === AQUÍ SE ASIGNA LA IMAGEN ===
        if (imgPokemon != null && datos.imagen != null)
            imgPokemon.sprite = datos.imagen;

        btnFicha1.interactable = (i != 0);
        btnFicha2.interactable = (i != 1);
    }

    public void CerrarTodo()
    {
    panelFicha.SetActive(false);
    }
}

// === CLASES DE DATOS ===
[System.Serializable]
public class FichaData
{
    public string nombre;
    public string tipo;
    public string hp;
    public string habilidad;
    [TextArea] public string descripcion;
    public Sprite imagen;   // ← Aquí subes la imagen desde el Inspector
}

[System.Serializable]
public class CartaData
{
    public string nombreTarget;  // "Lucario", "Mew", "Reverso"
    public FichaData ficha1;
    public FichaData ficha2;
}