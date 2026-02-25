//---------------------------------------------------------
// Singleton que representa al jugador.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este script proviene de la necesidad de tener un ente estático que devuelva la posición del
/// jugador, ya que los enemigos disparan y se mueven hacia este.
/// No se ha añadido al GameManager para evitar la dependencia Enemigo -> GameManager -> Jugador, y hacerlo
/// directamente como Enemigo -> Jugador.
/// </summary>
public class PlayerCore : MonoBehaviour
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

    /// <summary>
    /// Almacena la instancia del singleton PlayerCore
    /// </summary>
    private static PlayerCore _instance;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método que llama al cargarse en la escena.
    /// Intenta registrarse como _instance. Si ya existia, se destruye.
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.Log("Se han puesto dos PlayerCore en una escena. Uno será destruido.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Método llamado al destruir el objeto 
    /// </summary>
    private void OnDestroy()
    {
        if (_instance == this) _instance = null;
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
    /// Variable de acceso a la instancia.
    /// </summary>
    public static PlayerCore Instance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// Método que indica si PlayerCore tiene una instancia registrada o no.
    /// </summary>
    /// <returns></returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método que devuelve el transform.position del objeto en el que esté la instancia del PlayerCore.
    /// </summary>
    /// <returns></returns>
    public Vector3 ReadPlayerPosition()
    {
        return _instance.transform.position;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class PlayerCore 
// namespace
