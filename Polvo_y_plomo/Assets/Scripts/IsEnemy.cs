//---------------------------------------------------------
// Componente que se añade a enemigos para que sus muertes cuenten
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script que se le añade únicamente a los enemigos y que puede servir para hacer DuckTyping (han de tenerlo todos los enemigos).
/// Lleva lógica sobre enemigos; actualiza la cuenta de ellos en la escena (para el LevelManager) y indica cuando ha muerto
/// uno a LevelManager y GameManager.
/// </summary>
public class IsEnemy : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

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

    /// <summary>
    /// Se llama una vez al cargarse en escena si el componente esta activo, o tras activarse por primera vez.
    /// Le indica al LevelManager que ha aparecido un enemigo.
    /// </summary>
    private void Start()
    {
        if (LevelManager.HasInstance()) LevelManager.Instance.EnemySpawned(); // le dice al LevelManager que hay un nuevo enemigo en escena
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
    /// Se llama al morir un enemigoo.
    /// Indica al LevelManager y al GameManager que ha muerto un enemigo.
    /// </summary>
    public void EnemyDied()
    {
        if (LevelManager.HasInstance()) LevelManager.Instance.EnemyKilled();
        if (GameManager.HasInstance())
        {
            GameManager.Instance.AnEnemyDied(); // Cambia nivel de la habilidad.
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

} // class IsEnemy 
// namespace
