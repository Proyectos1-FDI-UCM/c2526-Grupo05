//---------------------------------------------------------
// Script de la habilidad "Disparo lento" del jugador
// Creado por Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngineInternal;
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

    [SerializeField] private AudioClip LevelUP;
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
    /// Variable que guarda el tiempo restante para activar otra vez la habilidad.
    /// </summary>
    private float _tToReactivateAbility = 0;

    /// <summary>
    /// Bool que dice si la habilidad está activa actualmente o no
    /// </summary>
    private bool _abilityOn = false;

    /// <summary>
    /// Tiempo restante de la habilidad en su activación actual
    /// </summary>
    private float _tRemainingOfAbility;

    /// <summary>
    /// Nivel actual de la habilidad. Inicializado a 0 para cuadrar con el array (en realidad se supone que empieza a nivel 1)
    /// </summary>
    private int _abilityCurrentLevel = 0;

    /// <summary>
    /// Proporción de tiempo restante de la habilidad en su activación actual (para el HUD)
    /// </summary>
    private float _abilityProportionLasting = 1.00f;

    /// <summary>
    /// Es el porcentaje de progreso para subir al siguiente nivel de habilidad (para el HUD)
    /// </summary>
    private float _levelBar = 0f;
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

        // Inicialización de los niveles
        int kills = kills = GameManager.Instance.TransferTotalDeaths();

        _abilityCurrentLevel = 0;
        bool f = false;
        while (_abilityCurrentLevel < AbilityLevels.Length - 1 && !f)
        {
            if (AbilityLevels[_abilityCurrentLevel + 1].AbilityUpgradeKillThreshold <= kills)
            {
                _abilityCurrentLevel++;
                GameManager.Instance.UpdateActLevelText(_abilityCurrentLevel + 1);
            }
            else
            {
                f = true;
            }
        }
        // Render inicial de la barra de nivel
        if (_abilityCurrentLevel < AbilityLevels.Length - 1) _levelBar = (float)(kills - AbilityLevels[_abilityCurrentLevel].AbilityUpgradeKillThreshold) / (float)(AbilityLevels[_abilityCurrentLevel + 1].AbilityUpgradeKillThreshold - AbilityLevels[_abilityCurrentLevel].AbilityUpgradeKillThreshold);
        else _levelBar = 1f;
        GameManager.Instance.UpdateLevelBar(_levelBar);
    }

    /// <summary>
    /// Update que comprueba si se puede activar la habilidad cada vez que recibe el input adecuado, y en caso positivo activarla,
    /// estableciendo a sus valores correspondientes las variables declaradas previamente, que controlan la lógica de desactivación
    /// tras el tiempo esperado según el nivel de la habilidad.
    /// </summary>
    void Update()
    {
        // Lógica de la habilidad
        if (_abilityOn)
        {
            // Paso del tiempo
            if (GameManager.SlowMultiplier != 0) _tRemainingOfAbility -= Time.deltaTime;

            if (_tRemainingOfAbility <= 0)
            {
                _tToReactivateAbility = PlayerAbilityCooldown;

                _abilityOn = false;
                GameManager.Instance.SlowShotOff();

                // Reset de las burbujas
                GameManager.Instance.UpdateTimeHabilityLiquid(1);
                GameManager.Instance.UpdateTimeHabilityShadow(1);
            }
            else if (InputManager.Instance.HabilityWasPressedThisFrame()) // Desactivar la habilidad
            {
                // Tiempo restante para reactivar la habilidad
                _tToReactivateAbility = PlayerAbilityCooldown * (1 - _tRemainingOfAbility / AbilityLevels[_abilityCurrentLevel].PlayerAbilityDuration);

                _abilityOn = false;
                GameManager.Instance.SlowShotOff();

                // reset fillAmmount del líquido
                GameManager.Instance.UpdateTimeHabilityLiquid(1);

                // Pintado de sombra
                GameManager.Instance.UpdateTimeHabilityShadow(_tToReactivateAbility / PlayerAbilityCooldown);
            }
            else
            {

                _abilityProportionLasting = _tRemainingOfAbility / AbilityLevels[_abilityCurrentLevel].PlayerAbilityDuration;

                // pintar el fillAmmount del liquido de la habilidad
                GameManager.Instance.UpdateTimeHabilityLiquid(_abilityProportionLasting);
            }
        }
        else
        {
            // Paso del tiempo
            _tToReactivateAbility -= Time.deltaTime * GameManager.SlowMultiplier;

            // Activacion de la habilidad
            if (InputManager.Instance.HabilityWasPressedThisFrame() && _tToReactivateAbility <= 0)
            {
                _tRemainingOfAbility = AbilityLevels[_abilityCurrentLevel].PlayerAbilityDuration;
                _abilityOn = true;
                GameManager.Instance.SlowShotOn(); // el resto de componentes que involucran a cosas en movimiento, tiempos, etc ahora van más lentos.
                                                   // el _tToReactivateHability se reinicia una vez _abilityOn = false.
            }

            // pintar el fillAmmount de la sombra de la habilidad
            GameManager.Instance.UpdateTimeHabilityShadow(_tToReactivateAbility / PlayerAbilityCooldown);
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
    /// Método público que aumenta el nivel de la habilidad del jugador en caso de que se haya alcanzado el
    /// umbral de kills que corresponda.
    /// Actualiza la barra de nivel del HUD, el texto con el nivel actual y activa el mensaje de subida de nivel.
    /// </summary>
    public void PlayerKill(int kills)
    {
        if (_abilityCurrentLevel < (AbilityLevels.Length - 1) )
        {
            if (kills >= AbilityLevels[_abilityCurrentLevel + 1].AbilityUpgradeKillThreshold)
            {
                _abilityCurrentLevel++;

                // Para que la habilidad se amplie si se sube de nivel durante la duración de esta:
                if (_abilityOn) _tRemainingOfAbility += AbilityLevels[_abilityCurrentLevel].PlayerAbilityDuration - AbilityLevels[_abilityCurrentLevel - 1].PlayerAbilityDuration;

                GameManager.Instance.ActivateLevelUpText();
                if (_abilityCurrentLevel == AbilityLevels.Length - 1) GameManager.Instance.UpdateLevelBar(1);
                else GameManager.Instance.UpdateLevelBar(0);
                GameManager.Instance.UpdateActLevelText(_abilityCurrentLevel + 1);
                if (LevelUP) AudioManager.Instance.Play(LevelUP, transform.position);
            }
            else
            {
                _levelBar = (float)(kills - AbilityLevels[_abilityCurrentLevel].AbilityUpgradeKillThreshold) / (float)(AbilityLevels[_abilityCurrentLevel + 1].AbilityUpgradeKillThreshold - AbilityLevels[_abilityCurrentLevel].AbilityUpgradeKillThreshold);
                GameManager.Instance.UpdateLevelBar(_levelBar);
            }
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
 