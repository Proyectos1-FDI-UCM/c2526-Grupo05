//---------------------------------------------------------
// Archivo que gestiona el cronómetro y texto de la meta de un nivel; y su representación en pantalla
// Creado por Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que calcula en cada frame el tiempo restante del cronómetro determinado, y actualiza la representación de dicho cronómetro, así como del objetivo del
/// nivel, en función de la situación temporal.
/// </summary>
public class TimerAndGoalManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Texto para el cronómetro del nivel
    /// </summary>
    [SerializeField] private TextMeshProUGUI TimerText = null;

    /// <summary>
    /// Texto para el objetivo del nivel
    /// </summary>
    [SerializeField] private TextMeshProUGUI GoalText = null;
    /// <summary>
    /// String para el objetivo del nivel
    /// </summary>
    [SerializeField] private string GoalString = null;
    
    /// <summary>
    /// Tiempo inicial del cronómetro del nivel (en segundos)
    /// </summary>
    [SerializeField] private float InitialTimeInSeconds = 210;
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
    /// Script de vibración del texto del cronómetro
    /// </summary>
    private UIVibration _timerVibration = null;
    /// <summary>
    /// Script de vibración del texto del objetivo
    /// </summary>
    private UIVibration _goalVibration = null;

    /// <summary>
    /// Array que guarda los umbrales de tiempo restante para que la vibración del texto del crono cambie
    /// </summary>
    private int[] _vibrationThresholds = { 60, 30, 10, 0 };
    /// <summary>
    /// Array que guarda las intensidades que toma la vibración del texto del crono, cuando queda cada vez menos tiempo
    /// </summary>
    private int[] _vibrations = { 1, 2, 3 };

    /// <summary>
    /// Tiempo restante en el crono
    /// </summary>
    private float _lastingTime = 210;
    /// <summary>
    /// Minutos restantes
    /// </summary>
    private int _minutes = 3;
    /// <summary>
    /// Segundos restantes
    /// </summary>
    private int _seconds = 30;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Awake que busca los scripts de vibración de ambos textos, y pone el tiempo restante al máximo (es decir, al inicial del crono).
    /// </summary>
    void Awake()
    {
        _timerVibration = TimerText.GetComponent<UIVibration>();
        _goalVibration = GoalText.GetComponent<UIVibration>();

        _lastingTime = InitialTimeInSeconds;
    }

    /// <summary>
    /// Update que calcula en cada frame el tiempo restante para representarlo en pantalla, y que cambia el texto del objetivo del nivel cuando el crono acaba.
    /// Además, controla las vibraciones de ambos textos.
    /// </summary>
    void Update()
    {
        _lastingTime -= Time.deltaTime * GameManager.SlowMultiplier;

        _minutes = (int)_lastingTime / 60;
        _seconds = (int)_lastingTime % 60;

        if (TimerText != null)
        {
            if (_lastingTime >= 0)
            {
                if (_seconds >= 10) TimerText.text = $"{_minutes}:{_seconds}";
                else TimerText.text = $"{_minutes}:0{_seconds}";

                if (_timerVibration != null)
                {
                    if (_lastingTime < _vibrationThresholds[0] && _lastingTime > _vibrationThresholds[1]) _timerVibration.ChangeIntensity(_vibrations[0]);
                    else if (_lastingTime < _vibrationThresholds[1] && _lastingTime > _vibrationThresholds[2]) _timerVibration.ChangeIntensity(_vibrations[1]);
                    else if (_lastingTime < _vibrationThresholds[2] && _lastingTime > _vibrationThresholds[3]) _timerVibration.ChangeIntensity(_vibrations[2]);
                }
            }

            else
            {
                TimerText.text = "0:00";
                _timerVibration.enabled = false;

                if (GoalText != null)
                {
                    _goalVibration.ChangeIntensity(_vibrations[2]);
                    GoalText.text = GoalString;
                }

                this.enabled = false;
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

    #endregion   

} // class TimerAndGoalManager 
// namespace
