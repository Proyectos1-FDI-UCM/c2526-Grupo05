//---------------------------------------------------------
// Permite comprobar el estado de "estar persiguiendo" o "atacando" del componente ChasePlayer
// Ángel Seijas de ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script de testeo para comprobar si se esta actualizando correctamente la variable _isChasing del componente ChasePlayer.
/// Para ello escribe con Debug.Log() el estado de persecución.
/// </summary>
public class ChasingTest : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// Almacena el componente ChasePlayer.
    /// Inicializado en el Start
    /// </summary>
    private ChasePlayer _chase;

    /// <summary>
    /// Almacena el último segundo en el que se comprobó, para evitar llenar la consola de testeos.
    /// </summary>
    private float _lastCheckTime = -99;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Se llama al activarse el componente, una vez solo.
    /// Se asegura de que exista el componente ChasePlayer en el gameObject.
    /// </summary>
    private void Start()
    {
        _chase = GetComponent<ChasePlayer>();
        if (_chase == null)
        {
            Debug.Log("Se ha puesto el componente \"ChasingTest\" en un objeto sin el componente ChasePlayer. No funcionará.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Se llama cada frame.
    /// Revisa si ChasePlayer esta persiguiendo o no, y lo escribe en consola, cada 1 s.
    /// </summary>
    private void Update()
    {
        if (Time.time - _lastCheckTime > 1f)
        {
            _lastCheckTime = Time.time;
            Debug.Log("Persiguiendo: " + _chase.IsChasing());
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
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class ChasingTest 
// namespace
