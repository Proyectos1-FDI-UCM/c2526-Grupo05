//---------------------------------------------------------
// Este script maneja el comportamiento de un gameObject que funciona como zona en la que una entidad recibe daño
// CamiloSandovalSánchez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este script maneja el comportamiento de un gameObject que funciona como zona en la que una entidad recibe daño
/// Resta un PV configurable a una entidad
/// </summary>
public class onCollisionDealDamage : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Variable que indica el daño que hace el objeto
    /// </summary>
    [SerializeField]
    private int DamageDone = 1;

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
    /// Se llama cada vez que el collider del GameObject colisiona con otro collider
    /// Cambia la vida del objecto con el que colisiona si este tiene el componente HealthManager.
    /// Si toca a un enemigo llama a su método Stun.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthChanger health = collision.gameObject.GetComponent<HealthChanger>();
        if (health != null)
        {
            health.CambiarVida(-DamageDone);
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

} // class MeleeObject 
// namespace
