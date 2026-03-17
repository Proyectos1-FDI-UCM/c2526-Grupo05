//---------------------------------------------------------
// Script de la habilidad "Disparo lento" del jugador
// Creado por Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que controla toda la lógica y funcionamiento de la habilidad del jugador, la cual, tras el input adecuado, comienza un contador en el que el "tiempo" de juego está ralentizado (no totalmente,
/// ya que por ejemplo, los cooldowns del jugador no se ven afectados). Esta ralentización está representada por la variable de get público del GameManager. La duración de la habilidad aumenta según sube
/// de nivel, tras alcanzar umbrales de kills (comunicación de nuevo con el GM que es el que guarda las kills del jugador).
/// Además, su CD y nivel se ven representados en el HUD con una burbuja (que se llena proporcionalmente) y una barra respectivamente. El CD de la habilidad no cambia con la subida de nivel.
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
    [SerializeField] 
    private float PlayerAbilityCooldown = 20f;

    /// <summary>
    /// Struct que guarda el tiempo de duración de la habilidad del jugador en un nivel, y el umbral de kills que necesita alcanzar
    /// para llegar a tal nivel.
    /// </summary>
    [System.Serializable] 
    public struct Level
    {
        /// <summary>
        /// Duración de la habilidad del jugador en un nivel
        /// </summary>
        public float PlayerAbilityDuration;

        /// <summary>
        /// Número de kills necesarias para alcanzar un nivel de la habilidad del jugador
        /// </summary>
        public int AbilityUpgradeKillThreshold;
    }

    /// <summary>
    /// Niveles de habilidad configurados.
    /// </summary>
    [SerializeField] 
    private Level[] AbilityLevels;
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
    /// Bool que dice si la habilidad está activa actualmente o no
    /// </summary>
    private bool _abilityOn = false;

    /// <summary>
    /// Tiempo restante de la habilidad en su activación actual
    /// </summary>
    private float _abilityDurationLasting = 99f;

    /// <summary>
    /// Nivel actual de la habilidad. Inicializado a 0 para cuadrar con el array (en realidad se supone que empieza a nivel 1)
    /// </summary>
    private int _abilityCurrentLevel = 0;

    /// <summary>
    /// Proporción de tiempo restante de la habilidad en su activación actual
    /// </summary>
    private float _abilityProportionLasting = 1.00f;

    /// <summary>
    /// Es la unidad de progreso que hace la LevelBar del HUD cada vez que muere un enemigo.
    /// </summary>
    private float _segmentLevelBar = 0f;

    /// <summary>
    /// Es la cantidad de unidades de progreso de la LevelBar del HUD
    /// </summary>
    private float _sumSegmentLevelBar = 0f;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start de programación defensiva que comprueba si hay Input y Game Managers en la escena, advirtiendo en caso negativo
    /// </summary>
    void Start()
    {
        if (InputManager.Instance == null)
        {
            Debug.Log("Se ha puesto el componente \"playerSlowShot\" en una escena sin InputManager. No funcionará.");
            Destroy(this);
        }
        if (GameManager.Instance == null)
        {
            Debug.Log("Se ha puesto el componente \"playerSlowShot\" en una escena sin GameManager. No funcionará.");
            Destroy(this);
        }
        else
        {
            // Se inicializa activo los cooldowns.
            GameManager.Instance.UpdateTimeHabilityLiquid(1);
            GameManager.Instance.UpdateTimeHabilityShadow(0);
        }

        foreach (Level level in  AbilityLevels)
        {
            if (level.PlayerAbilityDuration == 0 && level.AbilityUpgradeKillThreshold == 0)
            {
                Debug.Log("En \"playerSlowShot\" se ha añadido un nivel de habilidad sin configuración puesta. No funcionará.");
                Destroy(this);
            }
        }

        _segmentLevelBar = 1f / AbilityLevels[_abilityCurrentLevel].AbilityUpgradeKillThreshold;
    }

    /// <summary>
    /// El Awake se ejecuta cuando se activa un objeto con el componente.
    /// En este, en función de las muertes en total que lleve esta partida, dato almacenado por el GameManager,
    /// se establece el nivel de la habilidad del jugador. 
    /// </summary>
    void Awake()
    {
        int kills = GameManager.Instance.TransferTotalDeaths();
        _abilityCurrentLevel = 0;
        bool f = false;
        while (_abilityCurrentLevel < AbilityLevels.Length && !f)
        {
            if (AbilityLevels[_abilityCurrentLevel].AbilityUpgradeKillThreshold < kills)
            {
                _abilityCurrentLevel++;
            }
            else
            {
                f = true;
            }
        }
        PlayerKill(kills);
    }

    /// <summary>
    /// Update que comprueba si se puede activar la habilidad cada vez que recibe el input adecuado, y en caso positivo activarla,
    /// estableciendo a sus valores correspondientes las variables declaradas previamente, que controlan la lógica de desactivación
    /// tras el tiempo esperado según el nivel de la habilidad.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.HabilityWasPressedThisFrame() && !_abilityOn && Time.time - _lastAbilityActivationTime > PlayerAbilityCooldown)
        {
            // activacion de la habilidad
            _abilityDurationLasting = AbilityLevels[_abilityCurrentLevel].PlayerAbilityDuration;
            _abilityOn = true;
            GameManager.Instance.SlowShotOn(); // el resto de componentes que involucran a cosas en movimiento, tiempos, etc ahora van más lentos.
            _lastAbilityActivationTime = Time.time;
        }

        if (_abilityOn)
        {
            _abilityDurationLasting -= Time.deltaTime;
            if (_abilityDurationLasting <= 0)
            {
                _abilityDurationLasting = 0;

                _abilityOn = false;
                GameManager.Instance.SlowShotOff();

                // Reset de las burbujas
                GameManager.Instance.UpdateTimeHabilityLiquid(1);
                GameManager.Instance.UpdateTimeHabilityShadow(1);
            }
            else
            {
                _abilityProportionLasting = _abilityDurationLasting / AbilityLevels[_abilityCurrentLevel].PlayerAbilityDuration;

                // pintar el fillAmmount del liquido de la habilidad
                GameManager.Instance.UpdateTimeHabilityLiquid(_abilityProportionLasting);
            }
        }
        else
        {
            // pintar el fillAmmount de la sombra de la habilidad
            GameManager.Instance.UpdateTimeHabilityShadow(1 - (Time.time - _lastAbilityActivationTime) / PlayerAbilityCooldown);
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
    /// Método público que aumenta el nivel de la habilidad del jugador en caso de que se haya alcanzado el umbral de kills que corresponda.
    /// Actualiza la barra de nivel del HUD.
    /// </summary>
    public void PlayerKill(int kills)
    {
        if (kills >= AbilityLevels[_abilityCurrentLevel + 1].AbilityUpgradeKillThreshold && (_abilityCurrentLevel < (AbilityLevels.Length - 1)) )
        {
            _abilityCurrentLevel++;
            GameManager.Instance.UpdateLevelBar(0);
            _sumSegmentLevelBar = 0f;
            _segmentLevelBar = 1f / (AbilityLevels[_abilityCurrentLevel].AbilityUpgradeKillThreshold - AbilityLevels[_abilityCurrentLevel - 1].AbilityUpgradeKillThreshold);
            Debug.Log(_abilityCurrentLevel);
        }
        else
        {
            _sumSegmentLevelBar += _segmentLevelBar;
            GameManager.Instance.UpdateLevelBar(_sumSegmentLevelBar);
        }
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
 