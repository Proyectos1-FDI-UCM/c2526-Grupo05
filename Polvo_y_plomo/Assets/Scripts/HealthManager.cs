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
    /// <summary>
    /// Esta será la variable de la vida con la que iniciarán los gameObject. Debe ser configurable para ajustarse a cada caso especificó y no variará una vez establecida
    /// </summary>
    [SerializeField]
    private int VidaMax = 10; //Máxima vida del GameObject
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
    /// Esta será la variable de la vida que tendrán los game objects (irá variando)
    /// </summary>
    private int _vida;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// Al iniciar el juego, la vida del gameObject tomará el valor de la vida con la que empieza.
    /// </summary>
    private void Start()
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
    /// Este metodo permitirá curarse (teniendo como tope la vida con la que empiezas) y hacer daño hasta quedarte sin vida
    /// Si te quedas sin vida llamara al metodo para matar
    /// </summary>
    public void CambiarVida(int cambio = -1)      
    {
        if (_vida + cambio <= VidaMax)
        {
            _vida += cambio;
            if (_vida <= 0)
            {
                MetodoMuerte();
            }
        }
    }
    /// <summary>
    /// Este metodo permitirá leer la vida que tenga el gameObject
    /// </summary>
    public int LlamaVida()
    {
        return _vida;
    }
    /// <summary>
    /// Este metodo comprueba si el gameObject que le invoca es el jugador.
    /// Dependiendo de si lo llama el gameObject del jugador destruirá reiniciará la escena
    /// De otra manera comprueba si el gameObject tiene cadaver, si lo tiene lo genera el cadaver, de lo contrario destruye el objeto 
    /// (Incompleto/Futuro)
    /// </summary>
    public void MetodoMuerte()
    {
        if (this.GetComponent<playerControlledMovement>() != null)
        {
            //Hay que hacer más adelante para que se reinicie la escena
            Debug.Log("El jugador ha muerto");
        }
        else
        {
            if (this.GetComponent<GeneraCadaver>() != null)
            {
                GeneraCadaver genCad = this.GetComponent<GeneraCadaver>();
                genCad.PonCadaver();
            }
            else Debug.Log("Este Objeto no tiene un componente GeneraCadaver");
            Destroy(this.gameObject);
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
