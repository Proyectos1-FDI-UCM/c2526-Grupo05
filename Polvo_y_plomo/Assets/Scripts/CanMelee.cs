//---------------------------------------------------------
// Script que genera la hitbox de un ataque melee
// Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
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
    public GameObject MeleePrefab = null;
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
    /// <param name="dirCursorJugador"></param>
    /// <param name="posHitbox"></param>
    public void HitboxMelee(Vector2 dirCursorJugador, Vector2 posHitbox)
    {
        GameObject MeleeObj = Instantiate(MeleePrefab);
        float angulo = 180f / Mathf.PI * Mathf.Atan2(dirCursorJugador.y, dirCursorJugador.x);
        MeleeObj.transform.rotation = Quaternion.Euler(0, 0, angulo);
        MeleeObj.transform.position = posHitbox;
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