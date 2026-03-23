//---------------------------------------------------------
// Controlador del menú de settings del juego
// Creado por Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que controla el tipo de resolución y tamaño de pantalla en píxeles, además de los volúmenes del juego (comunicándose con el AudioManager) y la sensibilidad del cursor del jugador (comunicándose
/// con dicho objeto hijo del mismo). Además, se encarga de representarlos en el propio menú.
/// </summary>
public class SettingsMenuController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Texto del tipo de resolución de pantalla
    /// </summary>
    [SerializeField] private TextMeshProUGUI ResolutionsText;
    /// <summary>
    /// Texto del tamaño de la ventana de juego por píxeles (32x18)
    /// </summary>
    [SerializeField] private TextMeshProUGUI PixelsText;
    /// <summary>
    /// Texto que representa (del 0 al 10) el volumen de los efectos de sonido del juego
    /// </summary>
    [SerializeField] private TextMeshProUGUI SFXVolumeNumber;
    /// <summary>
    /// Texto que representa (del 0 al 10) el volumen de la música del juego
    /// </summary>
    [SerializeField] private TextMeshProUGUI MusicVolumeNumber;
    /// <summary>
    /// Texto que representa (del 0.1 al 10.0) la sensibilidad del cursor
    /// </summary>
    [SerializeField] private TextMeshProUGUI SensNumber;
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
    /// Referencia Transform al jugador
    /// </summary>
    private Transform _player = null;
    /// <summary>
    /// Cursor del jugador
    /// </summary>
    private playerControlledCursor _playerCursor = null;
    /// <summary>
    /// Tipos de resolucion  de pantalla. Se pueden añadir más
    /// </summary>
    private FullScreenMode[] _resolutions =
    {
        FullScreenMode.ExclusiveFullScreen,
        FullScreenMode.FullScreenWindow,
        FullScreenMode.Windowed
    };
    /// <summary>
    /// Índice de las resoluciones para ciclar en el menú de opciones
    /// </summary>
    private int _resolutionsIndex = 0;
    /// <summary>
    /// Proporciones por píxeles (32x18) para el tamaño de la ventana de juego
    /// </summary>
    private int[] _proportions = { 60, 40, 30 };
    /// <summary>
    /// Índice de las proporcones de píxeles para ciclar en el menú de opciones
    /// </summary>
    private int _proportionsIndex = 0;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start de programación defensiva que comprueba que haya LevelManager y jugador en la escena, ambos necesarios para poder modificar la sensibilidad del cursor del juego.
    /// </summary>
    void Start()
    {
        if (LevelManager.HasInstance())
        {
            _player = LevelManager.Instance.PlayerTransform();
            if (_player != null) _playerCursor = _player.GetComponentInChildren<playerControlledCursor>();
            else Debug.Log("Se ha puesto el componente \"SettingsMenuController\" en una escena sin Jugador. No se podrá tocar la sensibilidad del cursor");
        }

        else Debug.Log("Se ha puesto el componente \"SettingsMenuController\" en una escena sin LevelManager. No se podrá tocar la sensibilidad del cursor");
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
    /// Método público para cambiar a la siguiente resolución de pantalla guardada en la lista. Funciona de manera circular.
    /// </summary>
    public void NextFixedResolution()
    {
        _resolutionsIndex = (_resolutionsIndex + 1) % _resolutions.Length;
        Screen.fullScreenMode = _resolutions[_resolutionsIndex];
        UpdateSettingsGUI();
    }

    /// <summary>
    /// Método público para cambiar a la anterior resolución de pantalla guardada en la lista. Funciona de manera circular.
    /// </summary>
    public void PrevFixedResolution()
    {
        _resolutionsIndex--;
        if (_resolutionsIndex < 0) _resolutionsIndex = _resolutions.Length - 1;
        Screen.fullScreenMode = _resolutions[_resolutionsIndex];
        UpdateSettingsGUI();
    }

    /// <summary>
    /// Método público para cambiar a la siguiente proporción de píxeles guardada en la lista. Funciona de manera circular.
    /// </summary>
    public void NextPixelByPixelResolution()
    {
        _proportionsIndex = (_proportionsIndex + 1) % _proportions.Length;
        Screen.SetResolution(_proportions[_proportionsIndex] * 32, _proportions[_proportionsIndex] * 18, _resolutions[_resolutionsIndex]);
        UpdateSettingsGUI();
    }

    /// <summary>
    /// Método público para cambiar a la anterior proporción de píxeles guardada en la lista. Funciona de manera circular.
    /// </summary>
    public void PrevPixelByPixelResolution()
    {
        _proportionsIndex--;
        if (_proportionsIndex < 0) _proportionsIndex = _proportions.Length - 1;
        Screen.SetResolution(_proportions[_proportionsIndex] * 32, _proportions[_proportionsIndex] * 18, _resolutions[_resolutionsIndex]);
        UpdateSettingsGUI();
    }

    /// <summary>
    /// Método público para aumentar el volumen de los efectos de sonido del juego.
    /// </summary>
    public void IncreaseSFXVolume()
    {
        float effectsVolume = AudioManager.Instance.GetSFXVolume();
        AudioManager.Instance.SetSFXVolume(effectsVolume + 0.1f);
        UpdateSettingsGUI();
    }

    /// <summary>
    /// Método público para disminuir el volumen de los efectos de sonido del juego.
    /// </summary>
    public void DecreaseSFXVolume()
    {
        float effectsVolume = AudioManager.Instance.GetSFXVolume();
        AudioManager.Instance.SetSFXVolume(effectsVolume - 0.1f);
        UpdateSettingsGUI();
    }

    /// <summary>
    /// Método público para aumentar el volumen de la música del juego.
    /// </summary>
    public void IncreaseMusicVolume()
    {
        float musicVolume = AudioManager.Instance.GetMusicVolume();
        AudioManager.Instance.SetMusicVolume(musicVolume + 0.1f);
        UpdateSettingsGUI();
    }

    /// <summary>
    /// Método público para disminuir el volumen de la música del juego.
    /// </summary>
    public void DecreaseMusicVolume()
    {
        float musicVolume = AudioManager.Instance.GetMusicVolume();
        AudioManager.Instance.SetMusicVolume(musicVolume - 0.1f);
        UpdateSettingsGUI();
    }

    /// <summary>
    /// Método público para aumentar (en 0.01) la sensibilidad del cursor del jugador.
    /// </summary>
    public void LitSensIncrease()
    {
        if (_player != null)
        {
            float cursorSpeed = _playerCursor.GetCursorSpeed();
            _playerCursor.SetCursorSpeed(cursorSpeed + 0.01f);
            UpdateSettingsGUI();
        }
    }

    /// <summary>
    /// Método público para aumentar (en 0.1) la sensibilidad del cursor del jugador.
    /// </summary>
    public void BigSensIncrease()
    {
        if (_player != null)
        {
            float cursorSpeed = _playerCursor.GetCursorSpeed();
            _playerCursor.SetCursorSpeed(cursorSpeed + 0.1f);
            UpdateSettingsGUI();
        }
    }

    /// <summary>
    /// Método público para disminuir (en 0.01) la sensibilidad del cursor del jugador.
    /// </summary>
    public void LitSensDecrease()
    {
        if (_player != null)
        {
            float cursorSpeed = _playerCursor.GetCursorSpeed();
            _playerCursor.SetCursorSpeed(cursorSpeed - 0.01f);
            UpdateSettingsGUI();
        } 
    }

    /// <summary>
    /// Método público para disminuir (en 0.1) la sensibilidad del cursor del jugador.
    /// </summary>
    public void BigSensDecrease()
    {
        if (_player != null)
        {
            float cursorSpeed = _playerCursor.GetCursorSpeed();
            _playerCursor.SetCursorSpeed(cursorSpeed - 0.1f);
            UpdateSettingsGUI();
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Método privado para representar las variables controladas por este script, en pantalla. Más concreta y lógicamente, en los distintos apartados del menú de opciones.
    /// </summary>
    private void UpdateSettingsGUI()
    {
        ResolutionsText.text = $"{_resolutions[_resolutionsIndex]}";
        PixelsText.text = $"{_proportions[_proportionsIndex] * 32}x{_proportions[_proportionsIndex] * 18}";
        
        SFXVolumeNumber.text = $"{Mathf.Round(AudioManager.Instance.GetSFXVolume() * 10)}";
        MusicVolumeNumber.text = $"{Mathf.Round(AudioManager.Instance.GetMusicVolume() * 10)}";

        SensNumber.text = (_playerCursor.GetCursorSpeed() * 10f).ToString("0.0");
    }
    #endregion

} // class SettingsMenuController 
// namespace
