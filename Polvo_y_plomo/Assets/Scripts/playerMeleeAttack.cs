//---------------------------------------------------------
// Archivo para la lógica y el funcionamiento del ataque melé del jugador
// Hecho por Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// </summary>
/// Clase con toda la lógica del ataque melé del jugador, que funciona mediante la detección de una tecla que, al presionarse; genera delante del
/// jugador en la dirección en la que apunta su mira, un objeto vacío con una caja de colisión que daña y aturde (empuja hacia atrás) a los enemigos que se
/// encuentren en cualquier parte de su área durante su tiempo de efecto. Después de este periodo, desaparece.
/// Tiene coste de enfriamiento.
public class playerMeleeAttack : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] float Anchohitbox;
    [SerializeField] float Largohitbox;
    [SerializeField] float Duracionhitbox;
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
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    /// En el update se comprueba, a través del InputManager, que el control del ataque melee se ha presionado.
    /// Esto se hace frame a frame para evitar que el jugador deje pulsada la tecla, ya que esto haría demasiado artificial la mecánica.
    /// Cuando se cumple la condición, se llama al método del ataque melee proporcionándole la posición actual del jugador, que será utilizada en dicho método.
    void Update()
    {
        if (InputManager.Instance.MeleeWasPressedThisFrame()) Melee(transform.position);
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
    void Melee(Vector3 posJugador) // Método de lógica y funcionamiento esencial del ataque melee
    {
        // Se calcula la dirección del cursor con respecto al jugador tomando ambas posiciones (la del jugador desde el Update), para saber dónde generar la hitbox.
        Vector2 posCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirCursorJugador = (posCursor - (Vector2)posJugador).normalized;
        Vector2 posHitbox = (Vector2)posJugador + dirCursorJugador;

        // Se crea la hitbox y se coloca cerca del jugador, en la dirección comentada justo antes.
        GameObject hitbox = new GameObject("hitboxMelee");
        hitbox.transform.position = posHitbox;

        // Se le añade el collider y se pone a trigger ya que no es un objeto que se mantenga en el tiempo (no es una colisión "real").
        BoxCollider2D box = hitbox.AddComponent<BoxCollider2D>();
        box.isTrigger = true;
        box.size = new Vector2(Anchohitbox, Largohitbox);

        Destroy(hitbox, Duracionhitbox);
    }
    #endregion   

} // class playerMeleeAttack 
// namespace
