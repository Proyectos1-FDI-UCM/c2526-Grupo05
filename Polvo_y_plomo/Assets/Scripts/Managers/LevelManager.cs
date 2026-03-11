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

    private const float DIV_STREAK_DUR = 2;

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;

    /// <summary>
    /// Este es el contador actual de muertes.
    /// </summary>
    private int _deathsCount = 0;

    /// <summary>
    /// Este es el tiempo de la racha actual.
    /// </summary>
    private float _lastStreak = -99f;

    /// <summary>
    /// Esta es la duración de la racha actual.
    /// </summary>
    private float _streakDuration;

    /// <summary>
    /// Este es el multiplicador de puntaje, entendido como actual racha.
    /// </summary>
    private int _streak = 1;

    /// <summary>
    /// Este es el puntaje.
    /// </summary>
    private int _points = 0;
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
        InitialStreakPoints();
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
    /// Se ejecurta cada frame. Actualizará la racha y el puntuaje.
    /// </summary>
    private void Update()
    {
        UpdateScoreSystem();
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
    /// Este metodo actualiza las cantidad de muertes
    /// </summary>
    public void UpdateDeathsCount()
    {
        _deathsCount += 1;
        if (GameManager.HasInstance()) GameManager.Instance.UpdateTotalDeaths();
        Debug.Log("_deathsCount: " + _deathsCount);
    }

    /// <summary>
    /// Este método reduce la racha, reduce el tiempo de duraión de la nueva racha, y actualiza el tiempo de la última racha
    /// (Lo que debe hacer si NO se ha llegado a matar a otro enemigo).
    /// </summary>
    public void ReduceStreak()
    {
        _streak--;
        _streakDuration /= DIV_STREAK_DUR;
        _lastStreak = Time.time;
        GameManager.Instance.UpdateStreakMultiplierHUD(_streak);
        if (_streak > 2) GameManager.Instance.UpdateStreakBar(1);
    }

    /// <summary>
    /// Este método aumenta la racha, mantiene la duración de la racha, y actualiza el tiempo de la última racha.
    /// (Lo que debe hacer si se ha matado a otro enemigo).
    /// </summary>
    public void KeepStreak()
    {
        _streakDuration = MaxStreakDuration;
        _streak++;
        _lastStreak = Time.time;
        GameManager.Instance.UpdateStreakBar(1);
        GameManager.Instance.UpdateStreakMultiplierHUD(_streak);
    }

    /// <summary>
    /// Este método actualiza racha y puntuaje, en función de si se mantiene o no la racha.
    /// </summary>
    /// <param name="EnemyPoints"></param>
    public void UpdateScoreSystem(int EnemyPoints = 0)
    {
        if (_streak > 1)
        {
            GameManager.Instance.UpdateStreakBar(1 - (Time.time - _lastStreak) / _streakDuration);
        }

        if ((_streak > 1) && (Time.time - _lastStreak > _streakDuration))
        {
            ReduceStreak();
        }
        else if (EnemyPoints > 0)
        {
            KeepStreak();
            UpdateScore(EnemyPoints);
        }
    }

    /// <summary>
    /// Este método actualiza lo puntos del jugador, y se los envía al GameManager para que los actualice en el HUD.
    /// </summary>
    /// <param name="EnemyPoints"></param>
    public void UpdateScore(int EnemyPoints = 0)
    {
        if (EnemyPoints > 0)
        {
            _points += _streak * EnemyPoints;
            GameManager.Instance.UpdateScoreHUD(_points);
        }
    }

    /// <summary>
    /// Este metodo reinicia los puntos a su valor inicial en el nivel
    /// </summary>
    public void InitialStreakPoints()
    {
        if (GameManager.HasInstance())
        {
            _points = GameManager.Instance.TransferInitialPoints();
        }
    }

    /// <summary>
    /// Este metodo avisa al GameManager del final del nivel y y manda los puntos obtenidos en el mismo
    /// </summary>
    public void LevelEnd()
    {
        if (GameManager.HasInstance()) GameManager.Instance.LevelEnds(_points);
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

    #endregion

} // class LevelManager 
// namespace