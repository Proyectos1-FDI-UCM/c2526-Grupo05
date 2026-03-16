//---------------------------------------------------------
// Componente que se añade a un objeto y le da la capacidad de curar vida.
// Miguel Gómez García 
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script que verifica si el gameObject con el que choca tiene la vida máxima, y si no lo tiene podrá curarle una cierta cantidad de vida configurable.
/// El objeto con este script sera destruido tras una cierta cantidad de tiempo configurable.
/// </summary>
public class GiveHealth : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Con esta variable indicaremos la cantidad de curación que podra darnos el gameObject
    /// </summary>
    [SerializeField]
    private int CantidadCuracion = 1;

    /// <summary>
    /// Con esta variable indicaremos el tiempo de vida de nuestro gameObject
    /// </summary>
    [SerializeField]
    private float TiempoVida = 15f;

    /// <summary>
    /// Con esta variable indicaremos el tiempo de vida de nuestro gameObject
    /// </summary>
    [SerializeField]
    private float TiempoParpadeo = 5f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private bool _yaHaEmpezadoAParpadear = false;

    private CanFlash _canFlash;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>

    private void Awake()
    {
        _canFlash = GetComponent<CanFlash>();
    }
    void Update()
    {
        TiempoVida -= Time.deltaTime;
        
        if (TiempoVida <= TiempoParpadeo && !_yaHaEmpezadoAParpadear)
        {
            if (_canFlash != null)
            {
                _canFlash.StartFlashes();
                _yaHaEmpezadoAParpadear = true;
            }
        }

        if (TiempoVida <= 0) Destroy(gameObject);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthChanger curacion = collision.gameObject.GetComponent<HealthChanger>();
        if (curacion != null && curacion.CuracionPermitida())
        {
            curacion.CambiarVida(CantidadCuracion);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Se esta intentando curar vida a un GameObject sin el componente HealthChanger o con la vida al máximo, no se podrá curar");
        }
    }
    #endregion   

} // class GiveHealth 
// namespace
