//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class entityHurtMelee : MonoBehaviour
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
    bool entityIsPlayer = false;
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
        if (GetComponent<playerControlledMovement>() != null)//Verificar qué tipo de entidad tiene el collider
        {
            Debug.Log("Se ha puesto el componente \"entityHurtMelee\" en el jugador.");
            entityIsPlayer = true;
        }
        else entityIsPlayer = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MeleeObject objetoMelee = collision.gameObject.GetComponent<MeleeObject>();
        if (objetoMelee != null)
        {
            Debug.Log("Colision con prefab de melee.");
            if (entityIsPlayer && !objetoMelee.PlayerOrigin)//Melee de enemigo a jugador
            {
                //Restar vida jugador
            }
            else if (!entityIsPlayer && objetoMelee.PlayerOrigin)//Melee de jugador a entidad(enemigo/cobertura)
            {
                //Restar vida entidad
                //Stun enemigo
            }
            else if (!entityIsPlayer && !objetoMelee.PlayerOrigin /* && !GetComponent<EnemyMeleeAttack>() [Si no tengo ese componente soy una cobertura]*/)     //Melee de enemigo a cobertura
            {
                //Restar vida cobertura
            }
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

} // class entityHurtMelee 
// namespace
