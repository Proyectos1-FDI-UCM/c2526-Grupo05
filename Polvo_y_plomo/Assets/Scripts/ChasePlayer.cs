//---------------------------------------------------------
// Al añadirse a un objeto hace que este persiga al jugador (en modo "perseguir") hasta llegar a una cierta distancica de este, entrando en modo "atacar".
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente que consigue que un objeto persiga la posición del jugador según la lógica descrita de los enemigos en el GDD. Esta es:
/// Hay un parámetro, radio de persecución, en el que si el enemigo está a una distancia mayor de este, empieza a perseguir al jugador.
/// Hay otro parámetro, radio de ataque, en el que si el enemigo esta a una distancia igual o menor que esta, deja de perseguri al jugador, y empieza a atacarle.
/// También se puede configurar la velocidad de persecución al jugador.
/// 
/// El componente se centra en llevar la lógica de persecución y almacenar si actualmente estamos persiguiendo o no.
/// Usa un RigidBody2D para alterar su velocidad lineal y perseguir al jugador.
/// </summary>
public class ChasePlayer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Parámetro que indica a partir de que distancia el objeto empieza a perseguir al jugador, haciendolo si es igual o mayor.
    /// </summary>
    [SerializeField]
    private float ChaseRadius = 5f;

    /// <summary>
    /// Párámetro que indica a partir de que distancai el objeto deja de perseguir al jugador, haciendolo si es igual o menor.
    /// </summary>
    [SerializeField]
    private float AttackRadius = 3f;

    /// <summary>
    /// Parámetro que almacena la velocidad (tiles/second) a la que el jugador es perseguido.
    /// </summary>
    [SerializeField]
    private float ChaseSpeed = 6f;

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
    /// Parámetro que registra si el enemigo persigue al jugador.
    /// True = está persiguiendo.
    /// False = está atacando.
    /// 
    /// Se inicializa en el Start() y en cada Update().
    /// </summary>
    private bool _isChasing;

    /// <summary>
    /// Almacena el Rigidbody2d del objeto.
    /// Inicializado en el Awake().
    /// </summary>
    private Rigidbody2D rb;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Hace comprobaciones necesarias para que exista este componente.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.Log("Se ha puesto el componente \"ChasePlayer\" en un objeto sin RigidBody2D. No podrá perseguir al jugador");
            Destroy(this);
        }
    }

    private void Start()
    {
        if (!PlayerCore.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"ChasePlayer\" en una escena sin instancia de PlayerCore. No podrá perseguir al jugador");
            Destroy(this);
        }
        else
        {
            _isChasing = (PlayerCore.Instance.ReadPlayerPosition() - transform.position).magnitude >= ChaseRadius;
        }
    }

    /// <summary>
    /// Se llama cada frame.
    /// Comprueba la lógica de persecución y actualiza _isChasing en función de esta.
    /// Además controla el frenarse cuando se empieza a atacar, y si esta persiguiendo, se mueve a velocidad constante hacia el jugador.
    /// </summary>
    private void Update()
    {
        if (!_isChasing && (PlayerCore.Instance.ReadPlayerPosition() - transform.position).magnitude >= ChaseRadius)  // mientras no persigo compruebo si el jugador se aleja lo suficiente como para volver a perseguir
        {
            _isChasing = true;
        }
        else if (_isChasing && (PlayerCore.Instance.ReadPlayerPosition() - transform.position).magnitude <= AttackRadius) // mientras persigo compruebo si he llegado al radio de ataque
        {
            _isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }

        // Si estoy persiguiendo actualizo la velocidad hacia el jugador, con módulo ChaseSpeed.
        if (_isChasing)
        {
            rb.linearVelocity = ChaseSpeed*(PlayerCore.Instance.ReadPlayerPosition() - transform.position).normalized;
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
    /// Método para leer y saber si el componente esta persiguiendo actualmente al jugador.
    /// </summary>
    /// <returns></returns>
    public bool IsChasing()
    {
        return _isChasing;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class ChasePlayer 
// namespace
