//---------------------------------------------------------
// Se encarga de manejar la lógica necesaria tras la aparición de un enemigo, hasta su spawn como enemigo real.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script al que se le debe asignar un Prefab de enemigo y que se encarga de hacer que aparezca tras una animación inicial (que es opcional).
/// Si no se le asigna un Animator simplemente se encarga de hacer aparecer al enemigo.
/// 
/// Se le DEBE asignar HealthChanger para poder funcionar adecuadamente.
/// 
/// Se le puede asignar un parámetro de offset para la posición de Spawn, desde la posición del spawner.
/// (!) Al acabar, el objeto con este script será destruido.
/// </summary>
public class EnemySpawnLogic : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    
    /// <summary>
    /// Prefab del enemigo funcional que va a aparecer.
    /// </summary>
    [SerializeField]
    private GameObject EnemyPrefab;

    /// <summary>
    /// Animator que tiene que tener por defecto una animación de spawn del enemigo.
    /// </summary>
    [SerializeField]
    private Animator SpawnAnimator;


    /// <summary>
    /// Offset de la posición en la que aparecerá el enemigo, desde la posición del spawner.
    /// </summary>
    [SerializeField]
    private Vector3 SpawnPositionOffset;

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
    /// Guarda la duración del clip de animación indicado.Sirve para esperar a la animación en el update.
    /// Inicializado en el Awake() si hay animación.
    /// </summary>
    private float _duracionAnimacion;
    /// <summary>
    /// Indice del estado que contiene el clip de la animación de Spawn.
    /// </summary>
    private int _spawnID = 0;

    /// <summary>
    /// Variable que contiene una referencia al script de HealthChanger que ha de tener el EnemySpawnLogic.
    /// </summary>
    private HealthChanger _healthChanger;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen

    /// <summary>
    /// Se llama al cargarse en la escena.
    /// Revisa cosas necesarias para el componente (que EnemyPrefab este asignado) y comprueba si existe un Animator asignado, registrandolo en _hasAnimation.
    /// 
    /// Luego, si no hay animación, instancia el objeto y destruye el GameObject. 
    /// Si la hay, busca el clip indicado, fuerza que empiece, y registra su duración.
    /// </summary>
    private void Start()
    {
        if (EnemyPrefab == null)
        {
            Debug.Log("No se le ha asignado al componente \"EnemySpawnLogic\" un prefab de enemigo y no funcionará.");
            Destroy(this);
        }

        _healthChanger = GetComponent<HealthChanger>();
        if (_healthChanger == null)
        {
            Debug.Log("Se ha colocado el componente EnemySpawnLogic en un objeto que no tiene HealthChanger y no podrá funcionar");
            Destroy(this);
        }

        if (SpawnAnimator == null) DoSpawn();
        else
        {
            foreach (AnimationClip clip in SpawnAnimator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == ("Spawn" + _spawnID))
                {
                    SpawnAnimator.Play(clip.name, 0, 0f);
                    SpawnAnimator.speed = 1f;
                    _duracionAnimacion = clip.length;
                }
            }
        }
    }

    /// <summary>
    /// Se llama cada frame mientras el componente esté activo.
    /// Espera la duración de la animación y hace el spawn del enemigo, luego autodestruyendose.
    /// </summary>
    private void FixedUpdate()
    {
        if (GameManager.HasInstance())
        {
            _duracionAnimacion -= Time.deltaTime * GameManager.SlowMultiplier;
            if (SpawnAnimator != null) SpawnAnimator.speed = GameManager.SlowMultiplier;
        }
        else _duracionAnimacion -= Time.deltaTime;

        if (_duracionAnimacion < 0) DoSpawn();
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
    /// Método que modifica el ID de la animación.
    /// </summary>
    public void SetSpawnID(int id)
    {
        _spawnID = id;
    }
    /// <summary>
    /// Método que modifica el offset del spawn del enemigo.
    /// </summary>
    public void SetSpawnOffset(Vector3 offset)
    {
        SpawnPositionOffset = offset;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Método que realiza el spawn del enemigo y destruye el objeto (puesto que ya no se va a usar).
    /// Añadido funcionalidad para pasarle la vida del EnemySpawnLogic.
    /// </summary>
    private void DoSpawn()
    {
        GameObject enemy = Instantiate(EnemyPrefab, transform.position + SpawnPositionOffset, transform.rotation);
        HealthChanger enemyHealth = enemy.GetComponent<HealthChanger>();
        if (enemyHealth != null)
        {
            // Llamada ANTES de que se de el Start() del HealthChanger del enemigo -> no se ha inicializado
            // que sea antes es bueno ya que así al cargarse la nueva vida todavia no esta inicializado el _canFlash y por ende no hace un flash innecesario
            int danyoRecibido = enemyHealth.GetCurrentHealth() - _healthChanger.GetCurrentHealth();
            if (danyoRecibido > 0) enemyHealth.CambiarVida(-danyoRecibido);
        }

        Destroy(gameObject);
    }
    #endregion   

} // class EnemySpawnLogic 
// namespace
