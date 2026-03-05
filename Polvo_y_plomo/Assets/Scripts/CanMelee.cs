//---------------------------------------------------------
// Script que genera la hitbox de un ataque melee
// Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente que se le da a cualquier objeto que tenga la capacidad de realizar un ataque a melee.
/// Se encarga únicamente de realizar el ataque, el cuando y cómo (controlador) se deja a otro script.
/// </summary>
public class CanMelee : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Prefab del objeto que aparecerá para hacer daño.
    /// </summary>
    [SerializeField]
    private GameObject MeleePrefab;

    /// <summary>
    /// Determina que tan lejos aparecerá el objeto del ataque desde su origen (este GameObject).
    /// </summary>
    [SerializeField]
    private float DistanciaSpawnAtaque = 1f;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Start de programación defensiva en caso de que no se haya asignado ningún prefab a generar.
    /// </summary>
    void Start()
    {
        if (MeleePrefab == null)
        {
            Debug.Log("Se ha puesto el componente \"CanMelee\" sin un prefab de hitbox asignado. No podrá generar la hitbox.");
            Destroy(this);
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion
    
    /// <summary>
    /// Método público que genera un prefab de hitbox de un ataque melee, en una posición y rotación dadas, y lo destruye tras cierto tiempo.
    /// </summary>
    /// <param name="dirAtaque"></param>
    public void HitboxMelee(Vector2 dirAtaque)
    {
        float angulo = 180f / Mathf.PI * Mathf.Atan2(dirAtaque.y, dirAtaque.x);
        Instantiate(MeleePrefab, (Vector2)transform.position + DistanciaSpawnAtaque*dirAtaque.normalized, Quaternion.Euler(0, 0, angulo));
    }

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class CanMelee 
// namespace