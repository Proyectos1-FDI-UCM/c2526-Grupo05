//---------------------------------------------------------
// Lógica y funcionamiento del roll (rodado) del jugador
// Creado por Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que gestiona y ejecuta toda la lógica y funcionamiento de la acción (input) de roll del jugador. Cada vez que se recibe dicho input y si no está en
/// cooldown, desactiva los componentes de desplazamiento (playerControlledMovement), ataque melee (playerAttackMelee) y disparo (PlayerGetShootingInput) del
/// GameObject padre, así como el Box Collider (2D) del hijo que corresponda con su hitbox. Además, modifica la velocidad lineal del jugador a través de su
/// RigidBody (es decir, utilizando físicas y, por tanto, realizando las operaciones en el FixedUpdate) para mantenerla constante y en dirección fija durante
/// todo el rodado.
/// </summary>
public class playerRoll : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Cooldown configurable del roll, que corre desde que se presiona la tecla
    /// </summary>
    [SerializeField] private float CooldownRoll;
    /// <summary>
    /// Cuánto se desplaza el jugador con cada roll
    /// </summary>
    [SerializeField] private float DesplazamientoRodado;
    /// <summary>
    /// Cuánto dura el roll, desde que se presiona la tecla hasta que termina el desplazamiento
    /// </summary>
    [SerializeField] private float DuracionRodado;
    /// <summary>
    /// Collider de la hitbox (hijo) del jugador, que será desactivada posteriormente para evitar que este pueda recibir daño mientras rueda
    /// </summary>
    [SerializeField] private BoxCollider2D HitboxJugador;
    /// <summary>
    /// Sprite del arma del jugador, que debe desaparecer durante el roll.
    /// </summary>
    [SerializeField] private SpriteRenderer SpriteArmaJugador;
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
    /// RigidBody del jugador para gestionar su velocidad lineal (física)
    /// </summary>
    private Rigidbody2D _rb;
    /// <summary>
    /// Variable que se actualiza cada vez que el jugador realiza un roll. Almacena cuanto tiempo falta para volver a poder rodar.
    /// </summary>
    private float _tVolverARodar = 0;

    /// <summary>
    /// Bool que dice si el jugador se encuentra rodando o no, lo que le indica al FixedUpdate si debe hacer cosas o no
    /// </summary>
    private bool _isRolling = false;

    /// <summary>
    /// Variable que se iguala a la duración del rodado (DuracionRodado) creada arriba, cada vez que se inicia una acción de roll. Durante dicha acción, se
    /// actualiza constantemente en el FixedUpdate para avisar de cuándo el jugador debe dejar de rodar.
    /// </summary>
    private float _tDuracionRodado;

    /// <summary>
    /// Componente correspondiente al desplazamiento del jugador, que necesita desactivarse durante el roll
    /// </summary>
    private playerControlledMovement _desplazamientoJugador;

    /// <summary>
    /// Bool que dice si hay o no GameManager en la escena
    /// </summary>
    private bool _gameManager = false;

    /// <summary>
    /// Almacena la dirección en la que se está dando el roll.
    /// </summary>
    private Vector2 _dirRoll;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.Log("Se ha colocado \"PlayerRoll\" en un gameobject sin RigidBody2D, no funcionará");
            Destroy(this);
        }

        _desplazamientoJugador = GetComponent<playerControlledMovement>();
        if (_desplazamientoJugador == null)
        {
            Debug.Log("Se ha colocado \"PlayerRoll\" en un gameobject sin PlayerControlledMovement, no funcionará");
            Destroy(this);
        }
    }

    /// <summary>
    /// Start que inicializa los atributos que guardan el RigidBody2D del jugador, así como su script de desplazamiento básico (playerControlledMovement).
    /// </summary>
    void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("Se ha puesto \"PlayerRoll\" en una escena sin InputManager. No funcionará");
            Destroy(this);
        }
        _gameManager = GameManager.HasInstance();
    }

    /// <summary>
    /// Update que comprueba las condiciones necesarias para realizar un roll: que se pulse su tecla correspondiente, que el jugador se esté desplazando, y que
    /// la acción no esté en cooldown.
    /// </summary>
    void Update()
    {
        if (_gameManager) _tVolverARodar -= Time.deltaTime * GameManager.SlowMultiplier;
        else _tVolverARodar -= Time.deltaTime;

        if (InputManager.Instance.RollWasPressedThisFrame() && InputManager.Instance.MovementVector != Vector2.zero && _tVolverARodar <= 0)
        {
            _tVolverARodar = CooldownRoll;
            EmpiezaRoll();
        }
    }

    /// <summary>
    /// FixedUpdate que, mientras el jugador esté rodando, establece su velocidad lineal según se diseñe desde el editor, y que devuelve al estado de "no
    /// rodando" al GameObject cuando el roll ha sido efectuado por completo (se ha acabado su duración).
    /// </summary>
    void FixedUpdate()
    {
        if (_isRolling)
        {
            _rb.linearVelocity = _dirRoll * DesplazamientoRodado * (1 / DuracionRodado);
            if (_gameManager) _rb.linearVelocity *= GameManager.SlowMultiplier;

            if (_gameManager) _tDuracionRodado -= Time.fixedDeltaTime * GameManager.SlowMultiplier;
            else _tDuracionRodado -= Time.fixedDeltaTime;

            if (_tDuracionRodado <= 0)
            {
                _isRolling = false;
                _rb.linearVelocity = Vector2.zero;
                InputManager.Instance.ActivarInput();
                LogicaRoll(true);
            }
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

    /// <summary>
    /// Método privado (no hay accesos externos) que desactiva todas las funciones del jugador que no deben estar disponibles durante el roll, establece la
    /// variable que se modifica en el FixedUpdate a su máximo valor (la duración determinada actualmente para el rodado), e informa de que ha iniciado la
    /// acción.
    /// </summary>
    private void EmpiezaRoll()
    {
        _dirRoll = InputManager.Instance.MovementVector;
        InputManager.Instance.DesactivarInputRoll();
        LogicaRoll(false);
        _tDuracionRodado = DuracionRodado;
        _isRolling = true;
    }

    /// <summary>
    /// Método privado que activa/desactiva las funciones del jugador que no compaginan con el estado "rodando" del jugador.
    /// </summary>
    /// <param name="logica"></param>
    private void LogicaRoll(bool logica)
    {
        _desplazamientoJugador.enabled = logica;
        HitboxJugador.enabled = logica;
        SpriteArmaJugador.enabled = logica;
    }
    #endregion

} // class playerRoll 
// namespace
