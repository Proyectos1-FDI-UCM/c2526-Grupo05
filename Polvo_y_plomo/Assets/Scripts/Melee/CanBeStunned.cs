//---------------------------------------------------------
// Script que permite a un GameObject ser Stuneado por un ataque a melee.
// Camilo Sandoval Sánchez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script que lleva la lógica necesaria para que un enemigo pueda ser stuneado
/// tras recibir un ataque con OnCollisionStun. Esto implica parar todo su movimiento
/// natural y empujarlo en una dirección durante el stun.
/// 
/// En concreto se encarga de activar en ChasePlayer (componente necesario) la lógica
/// de ser Stunneado, y de desactivarla tras pasar suficiente tiempo en el Update().
/// 
/// Este componente se inicializa desactivado (para evitar un Update innecesario).
/// </summary>
public class CanBeStunned : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField]
    private float StunDuration = 1.0f;
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
    /// Almacena el ChasePlayer que debe tener el objeto con este componente.
    /// </summary>
    private ChasePlayer _chasePlayer;

    /// <summary>
    /// Almacena cuanto falta de duración de Stun.
    /// </summary>
    private float _tOfStunRemaining;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary> 
    /// Se llama al cargarse en escena.
    /// Hace comprobaciones necesarias para el componente.
    /// </summary>
    void Awake()
    {
        _chasePlayer = GetComponent<ChasePlayer>();
        if ( _chasePlayer == null)
        {
            Debug.Log("Se ha puesto el componente \"CanStun\" en un objeto sin el componente \"ChasePlayer\", y no podrá ser stunneado.");
            Destroy(this);
        }

        this.enabled = false;
    }

    /// <summary>
    /// Se llama cada frame mientras el componente este activo.
    /// Realiza un contador
    /// </summary>
    void Update()
    {
        // Reducción del contador según el flujo del tiempo.
        if (GameManager.HasInstance()) _tOfStunRemaining -= Time.deltaTime * GameManager.SlowMultiplier;
        else _tOfStunRemaining -= Time.deltaTime;

        if (_tOfStunRemaining <= 0)
        {
            _chasePlayer.Stunned(false);
            this.enabled = false;
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

    /// <summary>
    /// Método llamado por OnCollisionStun para iniciar el stun.
    /// </summary>
    public void Stun()
    {
        _tOfStunRemaining = StunDuration;
        _chasePlayer.Stunned(true);
        this.enabled = true;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class CanStun 
// namespace
