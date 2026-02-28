//---------------------------------------------------------
// Sirve para el cursor del jugador.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Para que tenga sentido ha de estar como hijo dentro de otro objeto (pensado para el jugador).
/// es un componente que permite mover un objeto con el movimiento típico de "mirar" (mouse y right stick),
/// añadiendole parámetros de distancia máxima de alejado y acercado válidas frente a el objeto padre en el que situa este. 
/// Si estas distancias se exceden, el cursor se teletransporta en la misma dirección hasta estar
/// a una distancia correcta.
/// La actualización del movimiento es cambiando el transform.localScale directamente (usa Time.deltaTime).
/// </summary>
public class playerControlledCursor : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Multiplica la velocidad de movimiento del cursor al moverse el mouse.
    /// No especifica su velocidad concreta puesto que esta puede variar según el input del usuario (sensibilidad, por ejemplo).
    /// </summary>
    [SerializeField]
    private float CursorSpeedMultiplier = 1f;

    /// <summary>
    /// Distancia máxima a la que se puede alejar el cursor respecto al objeto padre.
    /// Si se excede, retrocede (se teletransporta) en la misma dirección (jugador-cursor) hasta llegar a este máximo permitido
    /// </summary>
    [SerializeField]
    private float CursorMaxRadius = 4f;


    /// <summary>
    /// Distancia máxima a la que se puede acercar el cursor respecto al objeto padre.
    /// Si se acerca de más, retrocede (se teletransporta) en la misma dirección (jugador-cursor) hasta estar suficientemente lejos
    /// </summary>
    [SerializeField]
    private float CursorMinRadius = 1f;


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
            Debug.Log("Se ha puesto el componente \"playerControlledCursor\" en una escena sin InputManager. No podrá moverse.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Se llama cada frame.
    /// Actualiza la posición del mouse según el input del jugador,
    /// y se asegura de que no se salga de sus límites.
    /// </summary>
    void Update()
    {
        // Movimiento del mouse
        Vector3 goalPosition = transform.localPosition + (Vector3)InputManager.Instance.LookVector * Time.deltaTime * CursorSpeedMultiplier; // z = 0 automáticamente

        // Asegurarme de que no exceda los límites de radio
        if (goalPosition.magnitude > CursorMaxRadius) goalPosition = goalPosition / goalPosition.magnitude * CursorMaxRadius;
        else if (goalPosition.magnitude < CursorMinRadius) goalPosition =  goalPosition / goalPosition.magnitude * CursorMinRadius;

        // Actualización de la posición
        transform.localPosition = goalPosition;
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

} // class playerControlledCursor 
// namespace
