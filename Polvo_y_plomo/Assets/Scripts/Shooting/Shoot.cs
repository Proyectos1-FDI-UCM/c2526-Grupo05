//---------------------------------------------------------
// Este Script creará una bala en la dirección indicada.
// Juan José de Reyna Gosoy
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este componente generará un GameObject de bala cuando reciba una señal (se llame a la función adecuada: ). Le dará la dirección indicada en el método de spawn.
/// 
/// Es imperativo que este componente esté cómo hijo de un gameObject con RotateTowardsObject para que las balas aparezcan con la direccion y rotación adecuadas.
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
    /// Esta variable referencia al Objeto de tipo BulletMove que se instanciará cada vez que se dispare (la bala);
    /// </summary>
    [SerializeField]
    protected BulletMove Bullet;

    [SerializeField]
    private AudioClip ShootClip;


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
    /// Se llama al iniciar el GameObject con el componente.
    /// Tendrá que hacer las comprobaciones necesarias de que el GameObject y el componente están en el formato adecuado.
    /// </summary>
    void Awake()
    {
        if (Bullet == null)
        {
            Debug.Log("Se ha puesto el componente \"Shoot\" sin asociarse una bala. No podrá disparar.");
            Destroy(this);
        }

        if (GetComponentInParent<rotateTowardsObject>() == null)
        {
            Debug.Log("Se ha puesto el componente \"Shoot\" en un objeto cuyo padre no tiene el componente RotateTowardsObject y el spawn de la bala fallaría. No podrá disparar");
            Destroy(this);
        }
    }

    /// <summary>
    /// Si el componente es destruido por no poder funcionar, se asegura que el resto de componentes dejen de funcionar también.
    /// No puede destruir el controlador del disparo puesto que aquí no sabemos que script será.
    /// </summary>
    private void OnDestroy()
    {
        Destroy(GetComponent<HasAmmo>());
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
    public void ShootBullet(Vector2 fireDir)
    {
        float angulo = 180f / Mathf.PI * Mathf.Atan2(fireDir.y, fireDir.x);
        angulo %= 360;
        if (angulo < 0) angulo += 360f;
        Quaternion rot = Quaternion.Euler(0, 0, angulo);

        Instantiate(Bullet, transform.position, rot);

        if (ShootClip) AudioManager.Instance.Play(ShootClip, transform.position);
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
