//---------------------------------------------------------
// Este Script rotará el revólver alrededor del jugador siguiendo la dirección del cursor.
// Responsable de la creación de este archivo
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este componente moverá el GameObject al que se le ajunta, al rededor de su eje de coordenadas (el del GameObject objetivo, que es el revolver, será igual a la posición del jugador).
/// Siempre mantendrá la misma distancia al eje, y cambiará la rotación de tal manera que se encuentre en la linea entre el origen de su sistema de referencia, y un GameObject deseado.
/// </summary>
public class RotateRevolver : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Este es el GameObject del Cursor cuya rotación seguirá nuestro Componente. Ambos tendrán el origen de sus sistemas de referencia en el mismo lugar (serán hijos del mismo GameObject).
    /// </summary>
    [SerializeField]
    private playerControlledCursor Cursor;

    ///<summary>
    ///Esta será la distancia a la que siempre se encontrará el Componente del sistema de referencia.
    ///</summary>>
    [SerializeField]
    private float CircleRadious;

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
        transform.localPosition = new Vector3(CircleRadious, 0, 0);
    }

    /// <summary>
    /// El Update se llama en cada frame. En el Update, cada vez que se ejecute, 
    /// </summary>
    void Update()
    {
        Vector3 _newPos = transform.localPosition;

        _newPos = Cursor.transform.localPosition;
        _newPos = (_newPos / _newPos.magnitude) * CircleRadious;

        /*float ang = Mathf.Atan(Cursor.transform.localPosition.y / Cursor.transform.localPosition.x);
        ang *= Mathf.Rad2Deg;
        Debug.Log(ang);*/

        transform.forward = new Vector3(1, 0, 0);
        Quaternion rotation;
        if (_newPos.x < 0) rotation = new Quaternion(transform.localRotation.x, transform.localRotation.y + 180, transform.localRotation.z, transform.localRotation.w);
        else rotation = new Quaternion(transform.localRotation.x, 0, transform.localRotation.z, transform.localRotation.w);

        transform.localRotation = rotation;
        transform.localPosition = _newPos;
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

} // class RotateRevolver 
// namespace
