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
    /// 
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
    /// Variable que se actualiza cada vez que el jugador realiza un roll. Está inicializada a -99 para asegurar que se pueda realizar está acción nada más
    /// comienza el juego
    /// </summary>
    private float _tiempoDeUltimoRoll = -99f;
    /// <summary>
    /// Bool que dice si el jugador se encuentra rodando o no, lo que le indica al FixedUpdate si debe hacer cosas o no
    /// </summary>
    private bool _isRolling = false;
    /// <summary>
    /// Variable que se iguala a la duración del rodado (DuracionRodado) creada arriba, cada vez que se inicia una acción de roll. Durante dicha acción, se
    /// actualiza constantemente en el FixedUpdate para avisar de cuándo el jugador debe dejar de rodar. Inicializada a 99 por motivos que se explicarán más abajo
    /// </summary>
    /// <summary>
    private Vector2 dirRoll;
    /// </summary>
    private float _tiempoRestanteRodado = 99f;
    /// <summary>
    /// Componente correspondiente al desplazamiento del jugador, que necesita desactivarse durante el roll
    /// </summary>
    private playerControlledMovement _desplazamientoJugador;
    /// <summary>
    /// Componente correspondiente al ataque melee del jugador, que también se debe desactivar
    /// </summary>
    private playerMeleeAttack _meleeJugador;
    /// <summary>
    /// Componente correspondiente al disparo del jugador, que hara lo propio
    /// </summary>
    private playerControlledMovement _disparoJugador;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start que inicializa los atributos que guardan el RigidBody2D del jugador, así como sus scripts de desplazamiento básico (playerControlledMovement)
    /// y ataque melee (playerMeleeAttack).
    /// </summary>
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _desplazamientoJugador = GetComponent<playerControlledMovement>();
        _meleeJugador = GetComponent<playerMeleeAttack>();
    }

    /// <summary>
    /// Update que comprueba las condiciones necesarias para realizar un roll: que se pulse su tecla correspondiente, que el jugador se esté desplazando, y que
    /// la acción no esté en cooldown.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.RollWasPressedThisFrame() && InputManager.Instance.MovementVector != Vector2.zero &&
            Time.time - _tiempoDeUltimoRoll > CooldownRoll)
        {
            _tiempoDeUltimoRoll = Time.time;
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
            _rb.linearVelocity = dirRoll * DesplazamientoRodado * (1 / DuracionRodado);
            _tiempoRestanteRodado -= Time.fixedDeltaTime;
        }

        if (_tiempoRestanteRodado <= 0)
        {
            _isRolling = false;
            _rb.linearVelocity = Vector2.zero;
            LogicaRoll(true);
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
        dirRoll = InputManager.Instance.MovementVector;
        LogicaRoll(false);
        _tiempoRestanteRodado = DuracionRodado;
        _isRolling = true;
    }

    /// <summary>
    /// Método privado que activa/desactiva las funciones del jugador que no compaginan con el estado "rodando" del jugador.
    /// </summary>
    /// <param name="logica"></param>
    private void LogicaRoll(bool logica)
    {
        _desplazamientoJugador.enabled = logica;
        _meleeJugador.enabled = logica;
        //_disparoJugador.enabled = logica;
        HitboxJugador.enabled = logica;
        SpriteArmaJugador.enabled = logica;
    }
    #endregion

} // class playerRoll 
// namespace
