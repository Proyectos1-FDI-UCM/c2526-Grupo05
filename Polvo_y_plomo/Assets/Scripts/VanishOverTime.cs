//---------------------------------------------------------
// Componente para iniciar un Fade Out despues de un tiempo configurable
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente que inicia un FadeOut después de un tiempo configurable desde su aparición.
/// Se le ha de asignar FadeOut.
/// 
/// Una vez activado el FadeOut este componente se desactiva a si mismo.
/// </summary>
public class VanishOverTime : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Variable que almacena el tiempo de vida del objeto
    /// </summary>
    [SerializeField]
    private float VanishTime = 5f;

    [SerializeField]
    private FadeColor FadeOut;

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
    /// Almacena el tiempo desde la aparición
    /// Inicializada en el Start()
    /// </summary>
    private float _t = 0;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al cargarse en la escena.
    /// Hace comprobaciones necesarias para el componente.
    /// </summary>
    void Awake()
    {
        if (FadeOut == null)
        {
            Debug.Log("Componente VanishOverTime colocado sin configurarle un FadeOut. No funcionará");
            Destroy(this);
        }
    }

    /// <summary>
    /// Al ser activado se reinicia el contador del tiempo.
    /// </summary>
    private void OnEnable()
    {
        _t = 0;
    }

    /// <summary>
    /// Se llama cada frame si el componente esta activo.
    /// Lleva el contador y si ha pasado suficiente tiempo, activa el FadeOut y desactiva este componente.
    /// </summary>
    void Update()
    {

        if (GameManager.HasInstance()) _t += Time.deltaTime * GameManager.SlowMultiplier;
        else _t += Time.deltaTime;

        if (_t >= VanishTime)
        {
            FadeOut.enabled = true;
            this.enabled = false;
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

} // class VanishOverTime 
// namespace
