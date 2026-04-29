//---------------------------------------------------------
// Este script generará otro GameObject cuando sea llamado.
// Juan José de Reyna Godoy
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Cuando un GameObject muera (jugador, enemigo o cobertura), su clase HealthManager, que gestiona su vida, llame a esta, y esta genere un cadaver, es decir otro GameObject que remplaze al
/// anterior, con menos lógica y otras caracteríticas (principalmente desactivar colisiones y otro renderizado). El HealthManager llamará a esta clase una vez la vida del objeto llegue a cero,
/// y llamará a la función correspondiente antes de destruir el objeto anterior.
/// 
/// +++
/// Implementada funcionalidad para que puedan aparecer varios cadáveres (necesario para dejar caer drops de vida)
/// 
/// +++
/// Implementada funcionalidad para poder poner el cadaver en una posicion distinta si tenemos el compnoente EnemySpawnLogic
/// </summary>
public class GeneraCadaver : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Estos serán los cadáveres del GameObject con este componente.
    /// </summary>
    [SerializeField]
    GameObject[] Cadaveres;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    ///<summary>
    /// Este método genera un array de GameObjects dados;
    ///</summary>
    public void PonCadaver()
    {
        foreach (GameObject c in Cadaveres)
        {
            if (c != null)
            {
                EnemySpawnLogic enemySpawn = GetComponent<EnemySpawnLogic>();
                if (enemySpawn == null) Instantiate(c, transform.position + c.transform.position, transform.rotation);
                else
                {
                    Hitbox enemyHitbox = enemySpawn.GetComponentInChildren<Hitbox>();
                    if (enemyHitbox != null) Instantiate(c, enemyHitbox.transform.position + c.transform.position, enemyHitbox.transform.rotation);
                    else Debug.Log("No se ha encontrado hitbox en el EnemySpawnLogic y no se puede generar cadaver correctamente");
                }
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

} // class CoberturaScripy 
// namespace
