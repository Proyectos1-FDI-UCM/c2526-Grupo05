//---------------------------------------------------------
// Segundo patrón de Suzie, llama a enemigos y hasta que no hayan aparecido todos los enemigos, el jefe estará escondido y no recibira daño
// Miguel Gómez García
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// </summary>
public class SuziesSecondPattern : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Variable que sirve para guardar el audio del silbido
    /// </summary>
    [SerializeField]
    private AudioSource Whistle;

    /// <summary>
    /// Variable que sirve para poder bloquear y desbloquear el daño cuando el jefe este escondido
    /// </summary>
    [SerializeField]
    private HealthChanger HealthManager;

    /// <summary>
    /// Struct que guarda un array de spawners a modo de “nivel” de spawn de enemigos
    /// </summary>
    [System.Serializable]
    private struct SpawnLevel
    {
        public EnemySpawner[] LevelSpawners;
    }

    /// <summary>
    /// Array de structs que guarda distintos "niveles" de spawn de enemigos
    /// </summary>
    [SerializeField] private SpawnLevel[] SpawnLevels;

    [SerializeField]
    [Tooltip("El spawn inicial que activa el resto")]
    private EnemySpawner Pattern2Spawner;

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
    /// Contador de Spawners desactivados
    /// </summary>
    int deactivatedSpawns = 0;

    /// <summary>
    /// Contador que indica el nivel de spawn de enemigos actual
    /// </summary>
    private int _currentSpawnLevel = 0;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    void Awake()
    {
        Whistle = this.GetComponent<AudioSource>();
        HealthManager = this.GetComponent<HealthChanger>();
    }

    /// <summary>
    /// A cada spawn le decimos que somos el jefe para llevar la cuenta
    /// </summary>
    void Start()
    {
        for (int i = 0; i < SpawnLevels.Length; i++)
        {
            for (int j = 0; j < SpawnLevels[i].LevelSpawners.Length; j++)
            {
                if (SpawnLevels[i].LevelSpawners[j] != null) SpawnLevels[i].LevelSpawners[j].SetBoss(this);
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

    /// <summary>
    /// El patron 2 de Suzie, que reproduce el sonido, se esconde y activa los spawners
    /// </summary>
    public void IniciarPatron()
    {

        if (Whistle != null) Whistle.Play();

        Hide();

        if (Pattern2Spawner != null)
        {
            Pattern2Spawner.enabled = true;
        }
    }

    /// <summary>
    /// Suma los Spawners que se han desactivado y si se han acabado Suzie vuelve a ser vulnerable
    /// </summary>
    public void DeactivateSpawnWarning()
    {
        deactivatedSpawns++; // Sumamos 1 al contador de spawners apagados

        // Si ya se han apagado tantos spawners como hay en la lista Suzie volverá a aparecer siendo vulnerable
        if (deactivatedSpawns >= SpawnLevels[_currentSpawnLevel].LevelSpawners.Length)
        {
            UnHide();
            deactivatedSpawns = 0; // Reseteamos el contador para la próxima vez

            if (_currentSpawnLevel < SpawnLevels.Length - 1)
            {
                _currentSpawnLevel++;
                Pattern2Spawner = SpawnLevels[_currentSpawnLevel].LevelSpawners[0];
            }

            SuziePhaseManager phaseManager = GetComponent<SuziePhaseManager>();
            if (phaseManager != null)
            {
                phaseManager.ReportarAtaqueTerminado();
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

    /// <summary>
    /// Metodo que esconde a Suzie y hace que no pueda recibir
    /// </summary>
    private void Hide()
    {
        HealthManager.BlockDamage();
        // Aqui iria una animación de esconderse
    }

    /// <summary>
    /// Método que saca a Suzie de su escondite y hace que pueda recibir
    /// </summary>
    private void UnHide()
    {
        HealthManager.AllowDamage();
        // Aquí iria una animación para salir del escondite
    }
    #endregion

} // class SuziesSecondPattern 
  // namespace