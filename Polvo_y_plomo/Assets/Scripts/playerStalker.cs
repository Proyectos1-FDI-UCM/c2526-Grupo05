//---------------------------------------------------------
// Componente que hace que un objeto se adelante al movimiento del jugador
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Hace que un objeto (pensado que sea un objeto hijo del jugador, centrado en él) se adelante al movimiento del jugador.
/// El propósito de esto es que este sea el objeto seguido por la cámara, dándole mayor visibilidad al jugador en la dirección de su movimiento.
/// Este movimiento será fluido (tendrá parámetro Spring) y se podrá ajustar que tanto se aleja al adelantarse.
/// En concreto el movimiento se hace actualizando el transform.localPosition
/// </summary>
public class playerStalker : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    

    /// <summary>
    /// Determina a la que se alejará con el movimiento del jugador, para "adelantarse".
    /// </summary>
    [SerializeField]
    private float StalkerOffset = 2.5f;

    /// <summary>
    /// Determina que tan rápido se aleja hasta la distancia indicada.
    /// En concreto, determina para cada Time.deltaTime que porción de distancia entre la actual y la objetivo se recorre en total.
    /// Esto quiere decir que si el StalkerSpring fuese 1f, el Time.deltaTime necesario para llegar a la posición objetivo sería de 1s entero.
    /// Para 2f sería de 0.5s.
    /// Por otra parte, como el Time.deltaTime no funciona así, simplemente acaba siendo un parámetro que deja estimar qué tan rápido se adapta el objeto a posiciones nuevas.
    /// </summary>
    [SerializeField]
    private float StalkerSpring = 5f;

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
    /// <summary>
    /// Se llama la primera vez que el componente esta activo, después del Awake.
    /// Realiza comprobaciones necesarias para el componente.
    /// </summary>
    private void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"playerStalker\" en una escena sin InputManager. No podrá adelantarse al movimiento del jugador.");
            Destroy(this);
        }
    }
    /// <summary>
    /// Se llama cada frame
    /// Calcula la posición local en la que debería estar según el input del jugador y el parámetro StalkerOffset.
    /// Si no esta en esa posición local, se mueve hacia ella fluidamente, con el parametro StalkerSpring.
    /// </summary>
    void Update()
    {
        Vector3 goalPosition = StalkerOffset * (Vector3)InputManager.Instance.MovementVector;
        if (transform.localPosition != goalPosition) transform.localPosition += Time.deltaTime * StalkerSpring * (goalPosition-transform.localPosition);
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

} // class playerStalker 
// namespace
