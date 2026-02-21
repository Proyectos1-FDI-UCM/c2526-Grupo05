//---------------------------------------------------------
// Permite mover un objeto con el input de movimiento del jugador.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente que permite controlar un objeto con el movimiento típico de "caminar" (WASD, D-Pad y left stick).
/// Contiene un parámetro para ajustar la velocidad (units per second) a la que se mueve.
/// Según el InputManager, debería estar usando un input normalizado.
/// Actualiza la velocidad del RigidBody2D para moverse.
/// </summary>
public class playerControlledMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    // Parámetro que almacena la velocidad a la que se mueve el jugador, de forma constante.
    [SerializeField]
    private float PlayerSpeed = 5f;

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
    /// Guarda el Rigidbody2D del objeto. Inicializado en el awake.
    /// </summary>
    private Rigidbody2D rb;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Se llama al ser cargado, una vez.
    /// Hace comprobaciones necesarias para el componente.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.Log("Se ha puesto el componente \"playerControlledMovement\" en un objeto sin Rigidbody2D. No podrá moverse.");
            Destroy(this);
        }
    }
    /// <summary>
    /// Se llama la primera vez que el componente esta activo, después del Awake.
    /// Realiza comprobaciones necesarias para el componente.
    /// </summary>
    private void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"playerControlledMovement\" en una escena sin InputManager. No podrá moverse.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Se llama cada actualización de la física.
    /// Actualiza la velocidad de movimiento del jugador según el input y la velocidad asignada.
    /// </summary>
    private void FixedUpdate()
    {
        // Movimiento del jugador
        rb.linearVelocity = InputManager.Instance.MovementVector * PlayerSpeed;
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

} // class playerControlled 
  // namespace
