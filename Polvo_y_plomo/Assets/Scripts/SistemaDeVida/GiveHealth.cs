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
    /// Variable que indica a partir de cuantos segundos de vida RESTANTES empieza el parpadeo
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

    /// <summary>
    /// Maneja si ya se han iniciado los parpadeos del objeto cuando va a desaparecer en el Update().
    /// </summary>
    private bool _yaHaEmpezadoAParpadear = false;

    /// <summary>
    /// Almacena el componente _canFlash si existe en este GameObject.
    /// Inicializado en el Awake().
    /// </summary>
    private CanFlash _canFlash;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al cargarse en la escena.
    /// Registra el componente _canFlash.
    /// </summary>
    private void Awake()
    {
        _canFlash = GetComponent<CanFlash>();
    }

    /// <summary>
    /// Se llama cada frame si el componente esta activo.
    /// Lleva un timer desde que se instancia el objeto (estando activo) para hacer que desaparezca
    /// tras su tiempo de vida y para iniciar el parpadeo antes de desparecer.
    /// </summary>
    void Update()
    {
        if (GameManager.HasInstance()) TiempoVida -= Time.deltaTime * GameManager.SlowMultiplier;
        else TiempoVida -= Time.deltaTime;

        if (TiempoVida <= TiempoParpadeo && !_yaHaEmpezadoAParpadear)
        {
            if (_canFlash != null) _canFlash.StartFlashes();
            _yaHaEmpezadoAParpadear = true;
        }

        if (TiempoVida <= 0) Destroy(gameObject);
    }

    /// <summary>
    /// Se llama cuando el collider trigger de GiveHealth choca con algo.
    /// (!) NO se comprueba si con lo que se choca es jugador (solo ha de tener
    /// HealthChanger y poca vida para curarse).
    /// Esto quiere decir que se deben ajustar las layers de colision para que solo
    /// se choque con el jugador si así se quiere.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();
        if (hitbox != null)
        {
            hitbox.HitboxHeal(gameObject, CantidadCuracion);
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

} // class GiveHealth 
// namespace
