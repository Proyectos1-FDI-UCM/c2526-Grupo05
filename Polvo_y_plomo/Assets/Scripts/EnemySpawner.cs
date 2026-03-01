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
        public float SpawnSecond;
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
    /// Almacena la rutina de spawn para asegurarse que se detiene al desactivarse el componente.
    /// Se regista en onEnable().
    /// </summary>
    private Coroutine _spawnCoroutine;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al ser activado el componente, incluyendo al cargarse en la escena (si esta activo).
    /// Se encarga de iniciar la rutina de spawn de enemgios, y registrarla.
    /// </summary>
    private void OnEnable()
    {
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    /// <summary>
    /// Se llama al ser desactivado el componente. No se llama si al ser cargado el componente esta desactivado (necesariamente ha de ocurrir Enabled -> Disabled).
    /// Realiza la activación de otros EnemySpawner y se asegura de que la corutina iniciada haya acabado.
    /// </summary>
    private void OnDisable()
    {
        foreach (EnemySpawner spawner in ActivateSpawnersWhenDone) spawner.enabled = true;
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
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

    /// <summary>
    /// Corutina encargada de hacer aparecer la lista completa de enemigos, esperando los tiempos indicados.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnRoutine()
    {
        int i = 0;
        while (i < SpawnList.Length)
        {
            yield return new WaitForSeconds(SpawnList[i].SpawnSecond);

            Instantiate(SpawnList[i].EnemySpawnPrefab, transform.position, transform.rotation);
            i++;
        }

        this.enabled = false;
    }

    #endregion

} // class EnemySpawner 
// namespace
