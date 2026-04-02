//---------------------------------------------------------
// Lógica del jefe "Suzie" y sus diferenetes patrones
// Miguel Gómez García
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SuzieScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Variable que guarda el sónido que hace Suzie cuando llama a los enemigos
    /// </summary>
    [SerializeField]
    AudioSource wistle;

    [SerializeField] HealthChanger HealthManager;
    [SerializeField] List<EnemySpawner> Spawns; 
    int deactivatedSpawns = 0;

    [Header("Pattern 2")]
    [SerializeField]
    [Tooltip("El spawn inicial que activa el resto")]
    EnemySpawner Pattern2Spawner;

    [Header("Pattern 3")]
    [SerializeField] List<GameObject> Barrels;
    [SerializeField] Transform player;
    [SerializeField] GameObject grenadePrefab;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private Vector3 _firstTarget;
    private Vector3 _secondTarget;
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
    /// 

    /// <summary>
    /// Se recoge en el awake los componentes para asignarlos
    /// </summary>
    void Awake()
    {
        wistle = this.GetComponent<AudioSource>();
        HealthManager = this.GetComponent<HealthChanger>();
    }

    /// <summary>
    /// Inicializa la referencia del jugador y vincula a Suzie con sus spawners para permitir la comunicación
    /// </summary>
    void Start()
    {
        player = LevelManager.Instance.PlayerTransform();

        foreach (var spawn in Spawns)
        {
            spawn.SetBoss(this);
        }
    }

    /// <summary>
    /// Por ahora solo sirve para probar el patrón 2 y 3
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pattern2();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Pattern3();
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


    public void DeactivateSpawnWarning()
    {
        deactivatedSpawns++; // Sumamos 1 al contador de spawners apagados

        // Si ya se han apagado tantos spawners como hay en la lista...
        if (deactivatedSpawns >= Spawns.Count)
        {
            UnHide(); // ...Suzie vuelve a ser vulnerable
            deactivatedSpawns = 0; // Reseteamos el contador para la próxima vez
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
    /// Patrón 2 del jefe, permite spawnear enemigos tras emitir un silvido y esconderse
    /// </summary>
    private void Pattern2()
    {
        wistle.Play();
        Hide();

        if (Pattern2Spawner != null)
        {
            Pattern2Spawner.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Patrón 3 del jefe, permite lanzar 2 dinamitas al jugador o a coberturas
    /// </summary>
    private void Pattern3()
    {
        Barrels = GameObject.FindGameObjectsWithTag("Barrel").ToList<GameObject>();

        Debug.Log("Suzie ha encontrado " + Barrels.Count + " barriles.");

        if (Barrels.Count >= 2)
        {
            int r1 = UnityEngine.Random.Range(0, Barrels.Count);
            int r2 = r1;
            while (r2 == r1)
            {
                r2 = UnityEngine.Random.Range(0, Barrels.Count);
            }

            _firstTarget = Barrels[r1].transform.position;
            _secondTarget = Barrels[r2].transform.position;

            Invoke(nameof(ThrowFirstGrenade), 0.5f);
            Invoke(nameof(ThrowSecondGrenade), 1.0f);
        }
        else if (Barrels.Count == 1)
        {
            _firstTarget = Barrels[0].transform.position;
            _secondTarget = player.position;

            Invoke(nameof(ThrowFirstGrenade), 0.5f);
            Invoke(nameof(ThrowSecondGrenade), 2.5f);
        }
        else
        {
            _firstTarget = player.position;
            _secondTarget = player.position;

            Invoke(nameof(ThrowFirstGrenade), 2.0f);
            Invoke(nameof(ThrowSecondGrenade), 4.0f);
        }
    }

    private void ThrowFirstGrenade()
    {
        ThrowGrenadeTo(_firstTarget);
    }

    private void ThrowSecondGrenade()
    {
        ThrowGrenadeTo(_secondTarget);
    }

    /// <summary>
    /// Lanza la granada instanciando el prefab de la dinamita
    /// </summary>
    private void ThrowGrenadeTo(Vector3 targetPos)
    {
        Vector3 fireDir = targetPos - transform.position;
        float angulo = 180f / Mathf.PI * Mathf.Atan2(fireDir.y, fireDir.x);
        angulo %= 360;
        if (angulo < 0) angulo += 360f;
        Quaternion rot = Quaternion.Euler(0, 0, angulo);

        GameObject threwGrenade = Instantiate(grenadePrefab, transform.position, rot);

        GrenadeMove grenade = threwGrenade.GetComponent<GrenadeMove>();
        if (grenade != null)
        {
            grenade.SetTarget(targetPos);
        }
    }

    private void Hide()
    {
        HealthManager.BlockDamage();
        // Aqui iria una animación de esconderse
    }

    private void UnHide()
    {
        HealthManager.AllowDamage();
        // Aquí iria una animación para salir del escondite
    }

    #endregion   

} // class SuzieScript 
// namespace
