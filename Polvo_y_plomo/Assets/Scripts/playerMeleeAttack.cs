//---------------------------------------------------------
// Script para detectar el input del ataque melee y guardar posición y rotación de su hitbox
// Hecho por Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Clase que detecta el input del ataque melee y guarda la posición y rotación de su hitbox. Después, llama a otro script que genera dicha hitbox en función de
/// la posición y rotación guardadas por este.
/// Tiene coste de enfriamiento.
/// </summary>
public class playerMeleeAttack : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    
    // GameObjects asignables desde el editor que guarda el cursor del jugador
    public GameObject Cursor = null;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    // Variable float que guarda el cooldown (CD) del ataque melee. Actualizada en el Update cada vez que este se realiza. Está de base a -2,5 ya que el CD
    // es de 2,5 s; así se puede usar desde el segundo 0.
    private float _cooldownMelee = -2.5f;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Método Start de programación defensiva, para evitar buscar input de melee sin InputManager, MeleePrefab o Cursor.
    /// </summary>
    void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"playerMeleeAttack\" en una escena sin InputManager. No podrá atacar con melee.");
            Destroy(this);
        }

        if (Cursor == null)
        {
            Debug.Log("Se ha puesto el componente \"playerMeleeAttack\" sin un cursor asignado. No podrá atacar con melee.");
            Destroy(this);
        }
    }

    /// <summary>
    /// En el Update se comprueba, a través del InputManager, que el control del ataque melee se ha presionado.
    /// Esto se hace frame a frame para evitar que el jugador deje pulsada la tecla, ya que esto haría demasiado artificial la mecánica.
    /// Cuando se cumple la condición, se llama al método del ataque melee proporcionándole la posición actual del jugador, que será utilizada en dicho método.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.MeleeWasPressedThisFrame() && Time.time - _cooldownMelee > 2.5f) CanMelee(transform.position);
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
    /// <summary>
    /// Método privado que calcula la dirección del cursor con respecto al jugador tomando ambas posiciones (la del jugador desde el Update), para saber dónde
    /// generar la hitbox. Después llama al script "CanMelee" para que genere dicha hitbox en función de la información que le proporcione este script.
    /// </summary>
    private void CanMelee(Vector3 posJugador)
    {
        Vector2 posCursor = Cursor.transform.position;
        Vector2 dirCursorJugador = (posCursor - (Vector2)posJugador).normalized;
        Vector2 posHitbox = (Vector2)posJugador + dirCursorJugador;

        CanMelee canmelee = GetComponent<CanMelee>();
        if (canmelee != null) canmelee.HitboxMelee(dirCursorJugador, posHitbox);

        _cooldownMelee = Time.time;
    }
    #endregion   

} // class playerMeleeAttack 
// namespace
