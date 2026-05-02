//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SuziesThirdPattern : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Un array que almacenará el número de barriles que habrá en escena
    /// </summary>
    [SerializeField]
    GameObject[] Barrels;

    /// <summary>
    /// Variable para determinar a donde se lanzarán las dinamitas
    /// </summary>
    [SerializeField] 
    Transform player;

    /// <summary>
    /// Contadir hacia cobertura
    /// </summary>
    [SerializeField]
    float Contador1 = 0.5f;

    /// <summary>
    /// Contador hacia jugador
    /// </summary>
    [SerializeField]
    float Contador2 = 2f;

    /// <summary>
    /// Prefab de la dinamita que lanzará Suzie
    /// </summary>
    [SerializeField]
    private GameObject DynamitePrefab;

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

    private bool _manyBarrels = false;
    private bool _lessThanTwo = false;

    private float _tFirstDyna = 0f;

    /// <summary>
    /// Almacena el HeatlhChanger de Suzie para evitar que reciba daño durante este patrón
    /// </summary>
    private HealthChanger _suzieHealthChanger;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al cargarse en escena.
    /// Inicializa el componente.
    /// </summary>
    private void Awake()
    {
        _suzieHealthChanger = this.GetComponent<HealthChanger>();
    }

    /// <summary>
    /// En el update dependiendo de los booleanos que se hayan marcado como true, lanzaremos la segunda dinamita con el tiempo configurable al objetivo
    /// </summary>
    void Update()
    {
        if (GameManager.HasInstance()) _tFirstDyna += Time.deltaTime * GameManager.SlowMultiplier;
        else _tFirstDyna += Time.deltaTime;

        if (_manyBarrels && _tFirstDyna > Contador1)
        {
            ThrowSecondGrenade();
            FinalizarPatron();
        }
        else if (_lessThanTwo && _tFirstDyna > Contador2)
        {
            _secondTarget = player.position;
            ThrowSecondGrenade();
            FinalizarPatron();
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
    /// Localizaremos los objetos con el tag de barril y los guardaremos en un array
    /// Luego dependiendo de si hay más de una cobertura o no, determianremos a que barriles aleatorios y distintos lanzaremos la dinamita
    /// Si no hay más coberturas esa dinamita ira a la posición del jugador
    /// Se marcaran con booleanos cada situación para posteriormente el tiempo entre lanzamientos y se lanzará la primera dinamita
    /// </summary>
    public void IniciarPatron()
    {
        _suzieHealthChanger.BlockDamage();
        _manyBarrels = false;
        _lessThanTwo = false;

        player = LevelManager.Instance.PlayerTransform();

        Barrels = GameObject.FindGameObjectsWithTag("Barrel");

        Debug.Log("Suzie ha encontrado " + Barrels.Length + " barriles.");

        if (Barrels.Length >= 2)
        {
            _manyBarrels = true;

            int r1 = UnityEngine.Random.Range(0, Barrels.Length);
            int r2 = r1;
            while (r2 == r1)
            {
                r2 = UnityEngine.Random.Range(0, Barrels.Length);
            }

            _firstTarget = Barrels[r1].transform.position;
            _secondTarget = Barrels[r2].transform.position;
        }
        else if (Barrels.Length == 1)
        {
            _firstTarget = Barrels[0].transform.position;
        }
        else
        {
            _firstTarget = player.position;
        }
        if (!_manyBarrels) _lessThanTwo = true;
        ThrowFirstGrenade();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)


    /// <summary>
    /// Metodo para lanzar la primera dinamita e iniciar el contador desde que la ha lanzado
    /// </summary>
    private void ThrowFirstGrenade()
    {
        ThrowGrenadeTo(_firstTarget);
        _tFirstDyna = 0;
    }

    /// <summary>
    /// Metodo para lanzar la seguda dinamita
    /// </summary>
    private void ThrowSecondGrenade()
    {
        ThrowGrenadeTo(_secondTarget);
    }

    /// <summary>
    /// Lanza la granada instanciando el prefab de la dinamita
    /// </summary>
    private void ThrowGrenadeTo(Vector3 targetPos)
    {
        if (DynamitePrefab != null)
        {
            GameObject dynamite = Instantiate(DynamitePrefab, transform.position, Quaternion.identity);

            MoveToCoordsAndExplode moveScript = dynamite.GetComponent<MoveToCoordsAndExplode>();

            if (moveScript != null)
            {
                moveScript.SetFinalPosition(targetPos);
            }
        }
    }

    /// <summary>
    /// Termina el ataque y lo reporta al Manager
    /// </summary>
    private void FinalizarPatron()
    {
        _suzieHealthChanger.AllowDamage();
        GetComponent<SuziePhaseManager>().ReportarAtaqueTerminado();
    }
    #endregion
}
// class SuziesThirdPattern 
// namespace
