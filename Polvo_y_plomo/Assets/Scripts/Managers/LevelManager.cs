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
    private float StreakDuration = 5f;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

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
    private float _actualStreakTime;

    /// <summary>
    /// Esta es la duración de la racha actual.
    /// </summary>
    private float _actualStreakDuration;

    /// <summary>
    /// Este es el multiplicador de puntaje, entendido como actual racha.
    /// </summary>
    private int _actualStreak = 1;

    /// <summary>
    /// Este es el puntaje.
    /// </summary>
    private int _streakPoints = 0;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

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
    private void Start()
    {
        _actualStreakDuration = StreakDuration;
        InitialStreakPoints();
    }

    private void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }

    private void Update()
    {
        //Debug.Log("Puntos: " + _streakPoints);
        if ((_actualStreak > 1) && (Time.time - _actualStreakTime > _actualStreakDuration))
        {
            _actualStreak--;
            _actualStreakDuration /= 2;
            UpdateStreak();
            //Debug.Log("Streak: x" + _actualStreak + ", StreakTime: " + _actualStreakDuration);
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
    /// Este metodo actualiza las cantidad de muertes
    /// </summary>
    public void UpdateDeathsCount()
    {
        _deathsCount += 1;
        if (GameManager.HasInstance()) GameManager.Instance.UpdateTotalDeaths();
        Debug.Log("_deathsCount: " + _deathsCount);
    }

    /// <summary>
    /// Este metodo actualiza las racha
    /// </summary>
    public void UpdateStreak(int EnemyPoints = 0)
    {
        _actualStreakTime = Time.time;
        if (EnemyPoints >= 100)
        {
            _actualStreakDuration = StreakDuration;
            _streakPoints += _actualStreak * EnemyPoints;
            _actualStreak++;
            GameManager.Instance.UpdateScoreHUD(_actualStreak);
        }
    }
    /// <summary>
    /// Este metodo reinicia los puntos a su valor inicial en el nivel
    /// </summary>
    public void InitialStreakPoints()
    {
        if (GameManager.HasInstance())
        {
            _streakPoints = GameManager.Instance.TransferInitialPoints();
        }
    }
    /// <summary>
    /// Este metodo reinicia los puntos a su valor inicial en el nivel
    /// </summary>
    public int GetActualStreak()
    {
        return _actualStreak;
    }
    /// <summary>
    /// Este metodo reinicia los puntos a su valor inicial en el nivel
    /// </summary>
    public int GetActualPoints()
    {
        return _streakPoints;
    }
    /// <summary>
    /// Este metodo avisa al GameManager del final del nivel y y manda los puntos obtenidos en el mismo
    /// </summary>
    public void LevelEnd()
    {
        if (GameManager.HasInstance()) GameManager.Instance.LevelEnds(_streakPoints);
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