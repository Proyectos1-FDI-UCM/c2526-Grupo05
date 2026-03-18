//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// Template-P1
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
///
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas)
/// 
/// +++
/// Actualmente sirve para añadirse en toda escena en la que haya enemigos que
/// persigan y/o ataquen al jugador (ChasePlayer y EnemyMeleeAttack lo usan).
/// Se ha de asignar necesariamente el transform del jugador en el PlayerPosition para
/// que todo funcione bien.
/// 
/// +++
/// Ahora lleva toda la lógica de las rachas de muerte y el puntaje, avisando al GameManager
/// sobre todos los sucesos relevantes. Recibe de este al cargarse el puntaje actual, y tiene
/// una función a la que habrá que llamar para que se registre el puntaje en el GameManager
/// 
/// +++
/// Se ha añadido la funcionalidad de que almacene los puntos y cantidad de muertes al inicio
/// de una escena, funcionando junto al GameManager para permitir el respawn del jugador con
/// las estadisticas que tenía al entrar al nivel.
/// 
/// +++
/// Se ha añadido la funcionalidad para que reciba mejoras de nivel
/// una vez superado una escena.
/// 
/// +++
/// Se ha añadido la funcionalidad de tener condiciones de victoria para el nivel; actualmente
/// solo existe 1 y es que no haya enemigos en escena y el último spawner haya acabado de hacer
/// sus spawns (dentro de EnemySpawner se puede configurar si se es el último spawner).
/// 
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Parámetro en el que se ha de asignar el transform del jugador (necesario para muchos componentes de enemigos)
    /// </summary>
    [SerializeField]
    private Transform PlayerPosition;

    /// <summary>
    /// Este es el tiempo base de duración de la máxima racha alcanzada.
    /// </summary>
    [SerializeField]
    private float MaxStreakDuration = 5f;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Número por el que se va dividiendo la duración de descenso de racha en cada pérdida.
    /// </summary>
    private const float DIV_STREAK_DUR = 2;

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;

    /// <summary>
    /// Este es el tiempo de la racha actual.
    /// </summary>
    private float _lastStreak = -99f;

    /// <summary>
    /// Esta es la duración de la racha actual (la que se puede ir reduciendo).
    /// </summary>
    private float _streakDuration;

    /// <summary>
    /// Este es el multiplicador de puntaje, entendido como actual racha.
    /// </summary>
    private int _streak = 1;

    /// <summary>
    /// Este es el puntaje inicial registrado al iniciarse la escena.
    /// Inicializado en el Start(). Puede ser distinto de 0.
    /// </summary>
    private int _pointsOnStart = 0;

    /// <summary>
    /// Guarda la cantidad de muertes iniciales al cargarse la escena.
    /// Se inicializa en Start(). Puede ser distinto de 0.
    /// </summary>
    private int _killsOnStart = 0;

    /// <summary>
    /// Dice si el último spawner del nivel ya ha hecho su función o no.
    /// </summary>
    private bool _lastLevelSpawnerOff = false;

    /// <summary>
    /// Número de enemigos totales en escena.
    /// </summary>
    private int _totalEnemiesInScene = 0;
    #endregion
    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Se ejecuta al activar el objeto. Hará comprobaciones.
    /// </summary>
    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
            Init();
        }
        else
        {
            Debug.Log("Se han puesto 2 LevelManager en una misma escena. Uno será destruido");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Se ejecuta al iniciar la escena. Inicializará valores.
    /// </summary>
    private void Start()
    {
        _streakDuration = MaxStreakDuration;
        if (GameManager.HasInstance())
        {
            _pointsOnStart = GameManager.Instance.TransferInitialPoints();
            _killsOnStart = GameManager.Instance.TransferTotalDeaths();
        }
    }

    /// <summary>
    /// Se ejecutará al destruirse.
    /// </summary>
    private void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }

    /// <summary>
    /// Se ejecurta cada frame. Actualizará la racha y el puntuaje y comprobará las condiciones de victoria del nivel para finalizarlo.
    /// </summary>
    private void Update()
    {
        UpdateScoreSystem();

        if (_lastLevelSpawnerOff && _totalEnemiesInScene == 0)
        {
            LevelEnd();
            this.enabled = false;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el LevelManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método para acceder al transform del jugador.
    /// </summary>
    /// <returns></returns>
    public Transform PlayerTransform()
    {
        return PlayerPosition;
    }

    /// <summary>
    /// Este método actualiza racha y puntuaje, en función de si se mantiene o no la racha.
    /// Puede recibir 0 puntos (o negativos) para simplemente actualizar la racha.
    /// Si se le envian puntos, los procesa y envia al GameManager.
    /// </summary>
    /// <param name="EnemyPoints"></param>
    public void UpdateScoreSystem(int EnemyPoints = 0)
    {
        if (EnemyPoints > 0)
        {
            if (GameManager.HasInstance()) GameManager.Instance.UpdateScoreHUD(EnemyPoints * _streak); // enviado de puntos al GameManager
            KeepStreak();
        }

        if ((_streak > 1) && (Time.time - _lastStreak > _streakDuration))
        {
            ReduceStreak();
        }

        if (_streak > 1)
        {
            if (GameManager.HasInstance()) GameManager.Instance.UpdateStreakBar(1 - (Time.time - _lastStreak) / _streakDuration);
        }
    }


    /// <summary>
    /// Este metodo avisa al GameManager del final del nivel, 
    /// </summary>
    public void LevelEnd()
    {
        if (GameManager.HasInstance()) GameManager.Instance.LevelEnds();
    }

    /// <summary>
    /// Indica que el último spawner del nivel ya ha terminado su función.
    /// </summary>
    public void LastSpawnerDone()
    {
        _lastLevelSpawnerOff = true;
    }

    /// <summary>
    /// Aumenta el contador de enemigos totales en escena.
    /// </summary>
    public void EnemySpawned()
    {
        _totalEnemiesInScene++;
    }

    /// <summary>
    /// Reduce el contador de enemigos totales en escena.
    /// </summary>
    public void EnemyKilled()
    {
        _totalEnemiesInScene--;
    }
    
    /// <summary>
    /// Devuelve los puntos al inicio del nivel.
    /// Necesario para la funcionalidad de Respawn() de GameManager,
    /// </summary>
    public int GetPointsAtStartOfLevel()
    {
        return _pointsOnStart;
    }

    /// <summary>
    /// Devuelve la cantidad de kills al inicio del nivel.
    /// Necesario para la funcionalidad de Respawn() de GameManager.
    /// </summary>
    /// <returns></returns>
    public int GetKillsAtStartOfLevel()
    {
        return _killsOnStart;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        if (PlayerPosition == null)
        {
            Debug.Log("No se ha asignado PlayerPosition al LevelManager y es posible que componentes que lo necesiten den error");
        }
    }

    /// <summary>
    /// Este método reduce la racha, reduce el tiempo de duraión de la nueva racha, y actualiza el tiempo de la última racha
    /// (Lo que debe hacer si NO se ha llegado a matar a otro enemigo).
    /// </summary>
    private void ReduceStreak()
    {
        _streak--;
        _streakDuration /= DIV_STREAK_DUR;
        _lastStreak = Time.time;
        if (GameManager.HasInstance())
        {
            GameManager.Instance.UpdateStreakMultiplierHUD(_streak);
            if (_streak > 2) GameManager.Instance.UpdateStreakBar(1);
        }
    }

    /// <summary>
    /// Este método aumenta la racha, mantiene la duración de la racha, y actualiza el tiempo de la última racha.
    /// (Lo que debe hacer si se ha matado a otro enemigo).
    /// </summary>
    private void KeepStreak()
    {
        _streakDuration = MaxStreakDuration;
        _streak++;
        _lastStreak = Time.time;
        if (GameManager.HasInstance())
        {
            GameManager.Instance.UpdateStreakBar(1);
            GameManager.Instance.UpdateStreakMultiplierHUD(_streak);
        }
    }

    #endregion

} // class LevelManager 
// namespace