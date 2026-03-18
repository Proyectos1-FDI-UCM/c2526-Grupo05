//---------------------------------------------------------
// Componente para hacer que una hitbox en un objeto hijo funcione como si estuviese en el padre.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente para hacer de intermediario entre componentes que requieran interactuar con hitboxes y
/// el objeto padre que contiene los componentes con los que interactuar.
/// 
/// Actualmente cumple 2 funcionalidades:
/// 1) Recibe una señal de stun del OnCollisionStun y se la envia al CanStun del padre.
/// 2) Recibe daño hecho de OnCollisionDealDamage y se lo envia como daño al HealthChanger del padre.
/// </summary>
public class Hitbox : MonoBehaviour
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
    /// Almacena el HealthChanger del padre si lo tiene.
    /// </summary>
    private HealthChanger _healthChanger;

    /// <summary>
    /// Almacena el CanStun del padre si lo tiene.
    /// </summary>
    private CanStun _canStun;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama una vez tras cargarse en la escena si el componente esta activo, o la primera vez que se active.
    /// Inicializa el componente.
    /// </summary>
    private void Start()
    {
        _healthChanger = GetComponentInParent<HealthChanger>();
        _canStun = GetComponentInParent<CanStun>();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Se encarga de enviarle al objeto padre el daño hecho, inviertiendole el signo.
    /// Solo lo hace si el padre tiene HealthChanger.
    /// </summary>
    /// <param name="DamageDone"></param>
    public void HitboxDealDamage(int DamageDone)
    {
        if (_healthChanger != null) _healthChanger.CambiarVida(-DamageDone);
    }

    /// <summary>
    /// Se encarga de enviarle al objeto padre la señal de Stun.
    /// Solo lo hace si el padre tiene CanStun.
    /// </summary>
    public void HitboxStun()
    {
        if (_canStun != null) _canStun.Stun();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Hitbox 
// namespace
