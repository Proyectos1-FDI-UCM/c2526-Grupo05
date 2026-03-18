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
    /// Nombre del estado que contiene el clip de la animación de Spawn.
    /// </summary>
    [SerializeField]
    private string StateName = "Spawn";

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
    /// Guarda el tiempo en el que aparece este objeto. Sirve para esperar a la animación en el update.
    /// Se inicializa en el Awake().
    /// </summary>
    private float _t;

    /// <summary>
    /// Guarda la duración del clip de animación indicado.
    /// Inicializado en el Awake() si hay animación.
    /// </summary>
    private float _duracionAnimacion;
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
    private void Awake()
    {
        if (EnemyPrefab == null)
        {
            Debug.Log("No se le ha asignado al componente \"EnemySpawnLogic\" un prefab de enemigo y no funcionará.");
            Destroy(this);
        }

        if (SpawnAnimator == null) DoSpawn();
        else
        {
            _t = Time.time;
            foreach (AnimationClip clip in SpawnAnimator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == StateName)
                {
                    SpawnAnimator.Play(StateName, 0, 0f);
                    _duracionAnimacion = clip.length;
                }
            }
        }
    }

    /// <summary>
    /// Se llama cada frame mientras el componente esté activo.
    /// Espera la duración de la animación y hace el spawn del enemigo, luego autodestruyendose.
    /// </summary>
    private void Update()
    {
        if (Time.time - _t > _duracionAnimacion) DoSpawn();
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

    /// <summary>
    /// Método que realiza el spawn del enemigo y destruye el objeto (puesto que ya no se va a usar).
    /// </summary>
    private void DoSpawn()
    {
        Instantiate(EnemyPrefab, transform.position + SpawnPositionOffset, transform.rotation);
        Destroy(gameObject);
    }
    #endregion   

} // class EnemySpawnLogic 
// namespace
