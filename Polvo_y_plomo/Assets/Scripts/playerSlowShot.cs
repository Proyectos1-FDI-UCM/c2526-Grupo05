//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class playerSlowShot : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Cooldown de la habilidad del jugador
    /// </summary>
    [SerializeField] private float PlayerAbilityCooldown = 20f;
    /// <summary>
    /// Duración de la habilidad del jugador, que cambie en función de su nivel
    /// </summary>
    [SerializeField] private float[] PlayerAbilityDuration = new float[4];
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
    /// Variable que guarda la última vez que el jugador activó su habilidad. Inicializada a -99 para que se pueda usar desde segundo 0
    /// </summary>
    private float _lastAbilityActivationTime = -99f;
    /// <summary>
    /// Nivel actual de la habilidad. Inicializado a 0 para cuadrar con el array
    /// </summary>
    private int _abilityCurrentLevel = 0;
    /// <summary>
    /// Bool que dice si la habilidad está activa actualmente o no
    /// </summary>
    private bool _abilityOn = false;
    /// <summary>
    /// Tiempo restante de la habilidad en su activación actual
    /// </summary>
    private float _abilityDurationLasting = 99f;
    /// <summary>
    /// Proporción de tiempo restante de la habilidad en su activación actual
    /// </summary>
    private float _abilityProportionLasting = 1.00f;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update que comprueba si se puede activar la habilidad cada vez que recibe el input adecuado, y en caso positivo activarla, estableciendo a sus valores
    /// correspondientes las variables declaradas previamente, que controlan la lógica de desactivación tras el tiempo esperado según el nivel de la habilidad.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.HabilityWasPressedThisFrame() && Time.time - _lastAbilityActivationTime > PlayerAbilityCooldown)
        {
            _abilityDurationLasting = PlayerAbilityDuration[_abilityCurrentLevel];
            _abilityOn = true;
            //GameManager.Instance.SlowShotOn();
            _lastAbilityActivationTime = Time.time;
        }

        if (_abilityOn)
        {
            _abilityDurationLasting -= Time.deltaTime;
            _abilityProportionLasting = _abilityDurationLasting / PlayerAbilityDuration[_abilityCurrentLevel];
        }

        if (_abilityDurationLasting <= 0 && _abilityOn)
        {
            _abilityOn = false;
            //GameManager.Instance.SlowShotOff();
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
    /// Método público que aumenta el nivel de la habilidad del jugador.
    /// </summary>
    public void AbilityBuff()
    {
        _abilityCurrentLevel++;
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class playerSlowShot 
// namespace
