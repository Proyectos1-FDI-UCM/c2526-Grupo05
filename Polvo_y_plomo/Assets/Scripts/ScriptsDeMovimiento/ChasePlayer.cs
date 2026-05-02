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
/// 
/// +++
/// Se ha añadido la funcionalidad para que el enemigo sea stunneado. Se controla mediante CanBeStunned.
/// En concreto la funcionalidad esta añadida aquí ya que era necesario reconfigurar este componente para evitar
/// el movimiento durante el stun, y erá más sencillo que también se encargase de hacer el empuje.
/// Mientras el enemigo esta stunneado, _isChasing es verdadero y por ende los enemigos no atacan.
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
    /// Parámetro que indica la distancia a la que el enemigo empezara a esquivar obstaculos
    /// </summary>
    [SerializeField]
    private float raycastDistance = 2f;

    /// <summary>
    /// Parámetro que indica la que layers esquivara el enemigo
    /// </summary>
    [SerializeField]
    private LayerMask mascarachoque;

    /// <summary>
    /// Parámetro que el giro que usa el enemigo para esquivar
    /// </summary>
    [SerializeField]
    private float finalAngle = 15;


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

    /// <summary>
    /// Parámetro que almacena la velocidad (tiles/second) a la que el enemigo es empujado tras ser stunneado.
    /// </summary>
    [SerializeField]
    private float StunSpeed = 7f;
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
    /// Se inicializa en el Start() y actualiza en cada FixedUpdate().
    /// </summary>
    private bool _isChasing;

    /// <summary>
    /// Parámetro que indica si el enemigo está stunneado.
    /// </summary>
    private bool _isStunned = false;

    /// <summary>
    /// Almacena el Rigidbody2d del objeto.
    /// Inicializado en el Awake().
    /// </summary>
    
    private Rigidbody2D _rb;
    /// <summary>
    /// Almacena el Animator del objeto.
    /// Inicializado en el Awake().
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Almacena el Transform del jugador leido en el LevelManager.
    /// Inicializado en el Start();
    /// </summary>
    private Transform _playerTransform;

    /// <summary>
    /// Bool que dice si hay o no GameManager en la escena
    /// </summary>
    private bool _gameManager = false;

    /// <summary>
    /// Almacena la dirección y velocidad del stun en el momento en el que se inicia.
    /// </summary>
    private Vector3 _stunVelocity;
     
    /// <summary>
    /// Almacena el multiplicador de dificultad de velocidad del DificultyManager.
    /// Inicializado en el Start();
    /// </summary>
    private float _difficultySpeedMultiplier;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al cargarse en escena.
    /// Hace comprobaciones necesarias para que exista este componente.
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.Log("Se ha puesto el componente \"ChasePlayer\" en un objeto sin RigidBody2D. No podrá perseguir al jugador");
            Destroy(this);
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.Log("Se ha puesto el componente \"ChasePlayer\" en un objeto sin Animator. No podrá hacer sus animaciones");
        }
    }

    /// <summary>
    /// Se llama una vez si el componente esta activo o al activarse por primera vez.
    /// Hace comprobaciones necesarias para el componente, después del Awake().
    /// </summary>
    private void Start()
    {
        if (!LevelManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"ChasePlayer\" en una escena sin instancia de LevelManager. No podrá perseguir al jugador");
            Destroy(this);
        }
        else
        {
            _playerTransform = LevelManager.Instance.PlayerTransform();
            if (_playerTransform == null)
            {
                Debug.Log("Se ha puesto el componente \"ChasePlayer\" en una escena en la que no se le ha asignado PlayerTransform al LevelManager. No podrá seguir al jugador");
                Destroy(this);
            }
            else
            {
                _isChasing = (_playerTransform.position - transform.position).magnitude >= ChaseRadius;
            }
        }

        UpdateDifficultyStats();
        _gameManager = GameManager.HasInstance();
    }

    /// <summary>
    /// Se llama cada actualización de la física.
    /// Comprueba la lógica de persecución y actualiza _isChasing en función de esta.
    /// Además controla el frenarse cuando se empieza a atacar, y si esta persiguiendo, se mueve a velocidad constante hacia el jugador.
    /// </summary>
    private void FixedUpdate()
    {
        Vector3 directionToPlayer = _playerTransform.position - transform.position;

        //Revisar si chocamos con algo
        //Si chocamos nos movemos en otra direccion

        //ahora mismo, el raycast puede chocar con el propio jugador

        Debug.DrawRay(transform.position, directionToPlayer.normalized * raycastDistance, Color.red, Time.fixedDeltaTime);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, raycastDistance, mascarachoque);
        if (hit)
        {

            //Cuando deja de chocar a la derecha
            bool hittingright = true;
            Vector3 directionRight = directionToPlayer; //si chocamos con algo, nos movemos en otra dirección (perpendicular a la dirección al jugador)
            int rightangles = 0;
            while (hittingright)
            {
                rightangles += 5;
                directionRight = Quaternion.Euler(0, 0, rightangles) * directionRight; //si chocamos con algo, nos movemos en otra dirección (perpendicular a la dirección al jugador)
                RaycastHit2D _hit = Physics2D.Raycast(transform.position, directionRight.normalized, raycastDistance, mascarachoque);



                if (!_hit)
                {
                    hittingright = false;
                }
            }

            //Cuando deja de chocar a la izquierda
            bool hittingLeft = true;
            Vector3 directionLeft = directionToPlayer; //si chocamos con algo, nos movemos en otra dirección (perpendicular a la dirección al jugador)
            int lefttangles = 0;
            while (hittingLeft)
            {
                lefttangles += 5;
                directionLeft = Quaternion.Euler(0, 0, -lefttangles) * directionLeft; //si chocamos con algo, nos movemos en otra dirección (perpendicular a la dirección al jugador)
                RaycastHit2D _hit = Physics2D.Raycast(transform.position, directionLeft.normalized, raycastDistance, mascarachoque);


                if (!_hit)
                {
                    hittingLeft = false;
                }
            }

            directionRight = Quaternion.Euler(0, 0, (rightangles + finalAngle)) * directionRight; //si chocamos con algo, nos movemos en otra dirección (perpendicular a la dirección al jugador)
            directionLeft = Quaternion.Euler(0, 0, -(lefttangles + finalAngle)) * directionLeft; //si chocamos con algo, nos movemos en otra dirección (perpendicular a la dirección al jugador)


            Debug.DrawRay(transform.position, directionRight.normalized * raycastDistance, Color.yellow, Time.fixedDeltaTime);
            Debug.DrawRay(transform.position, directionLeft.normalized * raycastDistance, Color.green, Time.fixedDeltaTime);



            directionToPlayer = Vector3.Angle(directionRight, directionToPlayer) < Vector3.Angle(directionLeft, directionToPlayer) ? directionRight : directionLeft;



            //Debug.Log("Chocamos con " + hit.collider.name);
        }


        if (_isStunned) // lógica de stunneo
        {
            _isChasing = true; // durante el stun se indica que se persigue para evitar ataques de enemigo
            _rb.linearVelocity = _stunVelocity;
            if (_gameManager)
            {
                _rb.linearVelocity *= GameManager.SlowMultiplier;
                if (_animator != null) _animator.speed = GameManager.SlowMultiplier;
            }
        }
        else
        {
            if (!_isChasing && (directionToPlayer).magnitude >= ChaseRadius)  // mientras no persigo compruebo si el jugador se aleja lo suficiente como para volver a perseguir
            {
                _isChasing = true;
            }
            else if (_isChasing && (directionToPlayer).magnitude <= AttackRadius) // mientras persigo compruebo si he llegado al radio de ataque
            {
                _isChasing = false;
                if (!_isStunned) _rb.linearVelocity = Vector2.zero;
            }

            // Si estoy persiguiendo actualizo la velocidad hacia el jugador, con módulo ChaseSpeed.
            if (_isChasing)
            {
                _rb.linearVelocity = ChaseSpeed * (directionToPlayer).normalized * _difficultySpeedMultiplier;
                if (_gameManager) _rb.linearVelocity *= GameManager.SlowMultiplier;
            }
            else
            {
                if (!_isStunned) _rb.linearVelocity = Vector2.zero;
            }
        }
    }

    /// <summary>
    /// Se llama al destruirse el componente.
    /// Intentará destruir otros componentes que dependen completamente del ChasePlayer.
    /// </summary>
    private void OnDestroy()
    {
        Destroy(GetComponent<CanBeStunned>());
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

    /// <summary>
    /// Método para alterar el valor de la variable _isStunned.
    /// Si se inicia un Stun se registra la velocidad y la dirección a la que debe moverse durante el stun.
    /// </summary>
    public void Stunned(bool stunned)
    {
        _isStunned = stunned;
        _animator.SetBool("Stun", stunned);
        _animator.speed = 1f; // reinicio de la velocidad de animación
        if (stunned) _stunVelocity = StunSpeed * (transform.position - _playerTransform.position).normalized;
    }


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Método para actualizar las stats de este componente que dependan de la dificultad.
    /// </summary>
    private void UpdateDifficultyStats()
    {
        if (DifficultyManager.HasInstance())
        {
            if (GetComponent<CanMelee>() != null)
            {
                _difficultySpeedMultiplier = DifficultyManager.Instance.GetMeleeChaseSpeedMultiplier();
            }
            else
            {
                _difficultySpeedMultiplier = DifficultyManager.Instance.GetRangedChaseSpeedMultiplier();
            }
        }
        else
        {
            _difficultySpeedMultiplier = 1f;
        }
    }

    #endregion

} // class ChasePlayer 
// namespace
