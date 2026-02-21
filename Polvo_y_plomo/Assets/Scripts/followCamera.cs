//---------------------------------------------------------
// Permite que la cámara siga a un objeto.
// Ängel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using static UnityEngine.GraphicsBuffer;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente que sirve para que la cámara siga a un objeto asignable, intentarlo centrarlo.
/// La forma en la que actua es actualizando su velocidad lineal del RigidBody2D.
/// Usamos esto en vez de un simple cambio de posición ya que queremos que la cámara pueda chocar con colliders
/// concretos puestos para evitar que se salga del mapa.
/// </summary>
public class followCamera : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Determina que tan rápido se acerca a la distancia indicada.
    /// En concreto, determina para cada Time.deltaTime la velocidad a la que se mueve para recorrer la diferencia entre distancia actual y objetivo.
    /// Esto quiere decir que si el Spring fuese 1f, el Time.deltaTime necesario para llegar a la posición objetivo a velocidad constante sería de 1s entero.
    /// Para 2f sería de 0.5s.
    /// Por otra parte, como el Time.deltaTime no funciona así, simplemente acaba siendo un parámetro que deja estimar qué tan rápido se adapta el objeto a posiciones nuevas.
    /// </summary>
    [SerializeField]
    private float Spring = 3f;

    [SerializeField]
    private Transform Target;

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
    /// Guarda el RigidBody2D de la camara.
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
            Debug.Log("Se ha puesto el componente \"followCamera\" en un objeto sin Rigidbody2D. No podrá moverse.");
            Destroy(this);
        }
        if (Target == null)
        {
            Debug.Log("Se ha puesto el componente \"followCamera\" y no se le ha asignado un objetivo. No podrá moverse.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Se llama cada actualización de la física.
    /// Calcula y actualiza la velocidad de movimiento de la cámara según la distancia a recorrer y el parámetro Spring.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = Spring * ((Vector2)Target.position - (Vector2)transform.position); // z se ignora
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

} // class followCamera 
// namespace
