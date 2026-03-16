//---------------------------------------------------------
// Se encarga de manejar la lógica de aparición inicial de objetos a lo largo del tiempo.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using

/// <summary>
/// A este script se le asigna una lista de enemigos que se quiere que aparezcan y cuanto tiempo pasa entre un spawn y otro.
/// Se encarga de esperar estos tiempos y hacer que aparezcan esos objetos.
/// Los objetos en concreto han de tener el componente EnemySpawnLogic (si no, no se pueden asignar). Este será el componente que
/// se encargue de su aparición completa (animación + spawn real del enemigo).
/// También se le puede asignar una lista de otros EnemySpawner que activar cuando termine la lista este spawner.
/// 
/// El componente se desactiva solo una vez acabada la lista, y nada impide que se vuelva a activar para repetir la lista.
/// (!) Es recomendable que este componente se ponga en la escena como desactivado al comienzo, a no ser que se quiera que empiece inmediatamente.
/// (!) Si se activa el componente más de 2 veces, la lista de ActivateSpawnersWhenDone se ejecutará 2 veces.
/// Hay que tener esto en cuenta ya que es posible crear cadenas infintas si no se tiene cuidado.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints


    /// <summary>
    /// Struct que almacena un enemigo y el momento en el que debe aparecer.
    /// </summary>
    [System.Serializable]
    public struct EnemySpawn
    {
        /// <summary>
        /// Almacena el prefab del enemigo a hacer aparecer.
        /// Ha de tener el componente EnemySpawnLogic.
        /// </summary>
        public EnemySpawnLogic EnemySpawnPrefab;

        /// <summary>
        /// Almacena cuanto tiempo tarda en aparecer este enemigo desde el último que apareció.
        /// </summary>
        public float SpawnDelay;
    }

    /// <summary>
    /// Almacena la lista de enemigos a hacer a aparecer.
    /// </summary>
    [SerializeField]
    private EnemySpawn[] SpawnList;

    /// <summary>
    /// Almacena una lista de otros spawners que se activaran tras acabar este spawner.
    /// </summary>
    [SerializeField]
    private EnemySpawner[] ActivateSpawnersWhenDone;

    [SerializeField]
    private bool LastSpawner = false;
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
    /// Almacena el indice del enemigo que se esta esperando por hacer aparecer ahora.
    /// Inicializado en OnEnable().
    /// </summary>
    private int _indEnemigo;

    /// <summary>
    /// Almacena el tiempo desde el último respawn del enemigo.
    /// Inicializado en OnEnable().
    /// </summary>
    private float _t;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al ser activado el componente, incluyendo al cargarse en la escena (si esta activo).
    /// Se encarga de inicializar el indice de la lista de enemigos y el tiempo que se tendrá en cuenta a la hora de hacer los spawns.
    /// </summary>
    private void OnEnable()
    {
        _indEnemigo = 0;
        _t = Time.time;
    }

    /// <summary>
    /// Se llama al ser desactivado el componente. No se llama si al ser cargado el componente esta desactivado (necesariamente ha de ocurrir Enabled -> Disabled).
    /// Realiza la activación de otros EnemySpawner y envia la señal de que el último spawner ha acabado al LevelManager si es así.
    /// </summary>
    private void OnDisable()
    {
        foreach (EnemySpawner spawner in ActivateSpawnersWhenDone) if (spawner != null) spawner.enabled = true;
        if (LevelManager.HasInstance() && LastSpawner) LevelManager.Instance.LastSpawnerDone(); // condición de victoria último spawner
    }

    /// <summary>
    /// Se llama cada frame cuando el componente esta activo.
    /// Se encarga de verificar si ha pasado suficiente tiempo desde el último spawn de enemigo 
    /// para hacer aparecer a otro, y si es así, lo hace aparecer, avisando al LevelManager de ello.
    /// Al acabar la lista, desactiva este mismo componente.
    /// </summary>
    private void Update()
    {
        if (Time.time - _t >= SpawnList[_indEnemigo].SpawnDelay) // spawn de nuevo enemigo
        {
            _t = Time.time;
            Instantiate(SpawnList[_indEnemigo].EnemySpawnPrefab, transform.position, transform.rotation);
            _indEnemigo++;
            if (_indEnemigo >= SpawnList.Length) // lista terminada
            {
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

} // class EnemySpawner 
// namespace
