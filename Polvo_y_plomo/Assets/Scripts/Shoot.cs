//---------------------------------------------------------
// Este Script creará una bala, y le dará una dirección adecuada.
// Juan José de Reyna Gosoy
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este componente generará un GameObject cuando reciba una señal (se llame a la función adecuada: ), dándole a este GameObject la dirección adecuada (a la que apunta).
/// </summary>
public class Shoot : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// GameObject que será el objetivo hacia el que se disparará la bala.
    /// </summary>
    [SerializeField]
    private GameObject Objetivo = null;

    [SerializeField]
    private BulletMove Bullet = null;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private PlayerGetShootingInput _playerGetShootingInput = null;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al iniciar el GameObject con el componente.
    /// Tendrá que hacer las comprobaciones necesarias de que el GameObject y el componente están en el formato adecuado.
    /// </summary>
    void Awake()
    {
        if (Objetivo == null)
        {
            Debug.Log("Se ha puesto el componente \"PlayerGetShootingInput\" sin asociarse un objetivo. No podrá disparar.");
            Destroy(this);
        }

        if (Bullet == null)
        {
            Debug.Log("Se ha puesto el componente \"PlayerGetShootingInput\" sin asociarse una bala. No podrá disparar.");
            Destroy(this);
        }

        _playerGetShootingInput = GetComponent<PlayerGetShootingInput>();
        if (_playerGetShootingInput == null /* && Logica disparar enemigo, habrá que alterar el mensaje también */)
        {
            Debug.Log("Se ha puesto el componente  \"Shoot\" en un objeto sin el componente \"PlayerGetShootingInput\", no podrá disparar.");
            Destroy(this);
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

    /// <summary>
    /// Este método instanciará una bala, y le pasará una dirección a la que desplazarse.
    /// Esta será la dirección hacia el cursor.
    /// </summary>
    /// <param name="direction"> Dirección a la que apuntará la bala </param>
    public void ShootBullet()
    {
        // Aquí se instanciará la bala, y se le dará la dirección adecuada. 
        BulletMove bala = Instantiate(Bullet, transform.position, transform.Localrotation).GetComponent<BulletMove>();
        //bala. ;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Shoot 
// namespace
