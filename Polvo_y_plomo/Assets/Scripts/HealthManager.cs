//---------------------------------------------------------
// Gestor de vida
// Miguel Gómez García
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField]
    int VidaMax; //Máxima vida del GameObject
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    int _vida; // Vida del GameObject
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
        _vida = VidaMax; 
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
    /// Por lo general todos los ataques harán uno de daño, pero si te curas, no te puedes curar más del maximo de lo que se te permite
    /// </summary>
    public void CambiarVida(int cambio = -1)      
    {
        if (_vida + cambio <= VidaMax)
        {
            _vida = +cambio;
            if (_vida <= 0)
            {
                MetodoMuerte();
            }
        }
    }
    /// <summary>
    /// Por lo general todos los ataques harán uno de daño, pero si te curas, no te puedes curar más del maximo de lo que se te permite
    /// </summary>
    public void MetodoMuerte()
    {
        if (this.GetComponent<playerControlledMovement>() != null)
        {
            Debug.Log("El jugador ha muerto");
        }
        else
        {
            Destroy(this);
            //Hay que hacer más adelante las animaciónes de muerte de los enemigos
            //CargarCadaver();
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
}
