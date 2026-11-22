using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPokemon", menuName = "Pokedex/PokemonData")]
public class PokemonData : ScriptableObject
{
    public string nombre;
    public string tipo;
    public string hp;
    public string habilidad;
    [TextArea] public string descripcion;
    public Sprite imagenFicha; // Para el Image de la ficha
    public GameObject prefab3D; // El Prefab ARContentRoot con 2+ elementos 3D
} 

