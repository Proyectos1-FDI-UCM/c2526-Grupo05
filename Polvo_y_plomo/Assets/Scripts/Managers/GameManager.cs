//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro P. Gómez Martín
// Marco A. Gómez Martín
// Template-P1
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// 
/// Funcionalidades implementadas:
/// +++
/// Manejo del HUD: Se le pueden asignar distintos elementos del HUD para
/// que se vayan modificando con los datos del juego. Funcionará sin problemas
/// si no son asignados.
/// 
/// +++
/// Manejo entre escenas: Se le pueden asignar distintos tiempos de espera antes de
/// reiniciar el nivel o cargar la siguiente escena. Se encarga de llevar la lógica 
/// de reinicio de escena (Respawn()); reiniciar atributos del jugador, desactivar 
/// el input, realiza la animación de pantalla negra e inicia un contador con el Update(). 
/// También el de victoria de nivel (LevelEnds()) poniendo la pantalla negra, música 
/// de victoria y iniciando un contador distinto en el Update().
/// Las esperas se llevan en el Update, usando un parámetro para distinguir entre jugador
/// derrotado y victoria.
/// Por último tiene un método para reiniciar la escena y otro para cargar una distinta (usado
/// por el componente ChangeScene, para indicarle que a que escena cambiar).
/// 
/// +++
/// Tranferencia de información: permite configurarle a cada GameManager un
/// HUD en cada escena, que se transfiere en Awake() cuando la nueva instancia se da cuenta de que 
/// ya existe otro.
/// Incluye llevar la cuenta de la vida del jugador, su munición, cantidad de kills y cantidad de puntos.
/// Además el GameManager, al estar en el DontDestroyOnLoad, acarrea información entre escenas;
/// la vida del jugador, su puntaje y su cantidad de muertes. Este acarreo es intencionado y si el jugador
/// muriese estos datos se reinician a los que había al inicio de la escena (si hay LevelManager), o a 0.
/// Al cargarse en una escena, si había otro GameManager, es llamado el método NewSceneUpdate(), desde el
/// Start(). En esta llamada se hacen cosas necesarias al cargarse una escena; actualizar la vida del jugador,
/// el HUD, su puntaje, sus niveles de habilidad...
/// Si no hay otro GameManager se asume que la transferencia de datos es innecesaria.
/// +
/// Añadida a esta funcionalidad el método MatchEnded() que reinicia las estadísticas a cero. Será útil después
/// para añadir la funcionalidad de guardar el Highscore
/// 
/// +++
/// Lógica para la habilidad "SlowShot" del jugador, "ralentizando" el juego: todos los otros componentes que
/// tengan comportamientos relacionados con el tiempo usaran, si existe GameManager Instance, el parámetro
/// _slowMultiplier para simular la relantización del tiempo.
/// En caso de que no exista Instance, usan 1.
/// 
/// +++
/// Funcionalidad para almacenar la sensibilidad ajustada por el jugador.
/// 
/// +++
/// Funcionalidad añadida para manejar la vibración del multiplicador de la racha y sus colores. En el update
/// del HUD se aprovecha a verificar si cambiar la vibración.
/// 
/// +++
/// Funcionalidad añadida para manejar las animaciones de los corazones
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Texto que guarda el nivel actual de la habilidad.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI ActLevelMessage;

    /// <summary>
    /// Texto que saldrá al subir de nivel la habilidad.
    /// </summary>
    [SerializeField]
    ChangeColorAndHide LevelUpMessage;

    /// <summary>
    /// Componente con el FadeIn configurado
    /// Realizará un FadeIn de pantalla negra al morir el jugador.
    /// Se debería configurar para que acabe en 1 de transparencia.
    /// </summary>
    [SerializeField]
    private FadeColor FadeInBlackScreen;

    /// <summary>
    /// Componente con el FadeOut configurado
    /// Realizará un FadeOut de pantalla negra al reaparecer el jugador.
    /// </summary>
    [SerializeField]
    private FadeColor FadeOutBlackScreen;

    /// <summary>
    /// Componente con el FadeIn configurado.
    /// Realizará un FadeIn de pantalla azul al activarse la habilidad de tiempo lento del jugador.
    /// </summary>
    [SerializeField]
    private FadeColor FadeInBlueScreen;

    /// <summary>
    /// Componente con FadeOut configurado.
    /// Realizará un FadeOut de pantalla azul al desactivarse la habilidad de tiempo lento del jugador.
    /// </summary>
    [SerializeField]
    private FadeColor FadeOutBlueScreen;

    /// <summary>
    /// Componente con el "líquido" de la habilidad (duración restante de esta)
    /// Se podrá llamar al GameManager para modificar su fillAmmount.
    /// </summary>
    [SerializeField]
    private ImageFill HabilityLiquid;

    /// <summary>
    /// Componente con la "sombra" de la habilidad (duración restante del cooldown de la habilidad)
    /// Se podrá llamar al GameManager para modificar su fillAmmount.
    /// </summary>
    [SerializeField]
    private ImageFill HabilityShadow;

    /// <summary>
    /// Barril de revólver del HUD
    /// </summary>
    [SerializeField]
    private GameObject Barrel;

    /// <summary>
    /// Lista de objetos de vida del HUD
    /// </summary>
    [SerializeField]
    private HeartUI[] Lifes;
  
    /// <summary>
    /// Lista de objetos de balas del HUD
    /// </summary>
    [SerializeField]
    private GameObject[] Bullets = new GameObject[MUNICIONBASEJUGADOR];

    /// <summary>
    /// Texto que muestra los puntos en el HUD.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI ScoreText;

    /// <summary>
    /// Texto que muestra el multiplicador en el HUD.
    /// También se usará para acceder al componente UIVibration del mismo GameObject.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI StreakMultiplier;


    /// <summary>
    /// Struct para añadir colores al multiplicador y su racha asociada a la que cambiar
    /// </summary>
    [System.Serializable]
    public struct StreakColor
    {
        /// <summary>
        /// Color al que cambiará el texto en este StreakColor
        /// </summary>
        public Color Color;

        /// <summary>
        /// Cantidad para pasar al siguiente nivel configurado de StreakColors.
        /// El del último elemento del Array será ignorado.
        /// </summary>
        public int StreakToChangeColor;

        /// <summary>
        /// Nueva Vibration Intensity para el texto.
        /// </summary>
        public float NewVibration;
    }

    /// <summary>
    /// Array de colores.
    /// </summary>
    [SerializeField]
    private StreakColor[] StreakColors;

    /// <summary>
    /// Componente con el "liquido" de la barra que muestra cuanto porcentaje de tiempo
    /// queda para que la racha baje en 1.
    /// </summary>
    [SerializeField]
    private ImageFill StreakBar;

    /// <summary>
    /// Componente con el "liquido" de la barra que muestra cuantas muertes
    /// quedan para que la habilidad suba de nivel.
    /// </summary>
    [SerializeField]
    private ImageFill LevelBar;

    /// <summary>
    /// Componente de audio que almacena la canción de victoria que sonará
    /// tras ganar un nivel.
    /// </summary>
    [SerializeField]
    private AudioClip VictoryMusic;

    /// <summary>
    /// Indice de la escena que contiene el nivel al que se pasará tras
    /// ganar la partida.
    /// </summary>
    [SerializeField]
    private int NextLevel = 0;

    /// <summary>
    /// Tiempo que tardara la escena en reiniciarse
    /// </summary>
    [SerializeField]
    private float TiempoEsperaRespawn = 3f;

    /// <summary>
    /// Almacena cuanto tiempo se tardará en cargar la siguiente escena tras
    /// la victoria del jugador.
    /// </summary>
    [SerializeField]
    private float TiempoEsperaSiguienteNivel = 5f;

    /// <summary>
    /// ImageFill de la "sombra" del ataque melee, para representar su cooldown con un sprite que "cambia" de color
    /// </summary>
    [SerializeField]
    private ImageFill MeleeCooldown = null;

    [Header("Highscore")]

    /// <summary>
    /// Texto en el que se escribe el número del highscore.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI highScoreTextUI;


    [SerializeField]
    int highScore;

    [SerializeField]
    GameObject[] streakText;

    [SerializeField]
    int actualtext;


    /// <summary>
    /// Variable a la que se le debe asignar el Animator del icono de la habilidad.
    /// En concreto, la del icono (reloj y contorno).
    /// Esto hace que se pueda cambiar al "estado activo" durante la habilidad.
    /// </summary>
    [SerializeField]
    private Animator TimeAbilityAnimator;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Constante que guarda la vida máxima del jugador.
    /// </summary>
    private const int VIDABASEJUGADOR = 10;

    /// <summary>
    /// Constante que guarda la munición máxima del jugador.
    /// </summary>
    private const int MUNICIONBASEJUGADOR = 6;

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Esta es la vida actual del jugador.
    /// Inicializada en 10 por ser con la que empieza.
    /// </summary>
    private int _vidaJugador = VIDABASEJUGADOR;

    /// <summary>
    /// Esta es la munición actual del jugador.
    /// Inicializada en 6 por ser en la que empieza.
    /// </summary>
    private int _municionJugador = MUNICIONBASEJUGADOR;

    /// <summary>
    /// Este es el contador total de muertes.
    /// </summary>
    private int _totalDeaths = 0;
    
    /// <summary>
    /// Este es el contador total de puntos.
    /// </summary>
    private int _totalPoints = 0;

    /// <summary>
    /// Guarda un tiempo concreto. Usado para esperar en el Update().
    /// </summary>
    private float _t;

    /// <summary>
    /// Variable que guarda la velocidad de ralentización, distinta de 1,00 cuando
    /// la habilidad del jugador está activa. 
    /// Tiene una variable de acceso con un get para que aquellos scripts que la necesitan para 
    /// modificar su velocidad, tengan acceso a ella.
    /// </summary>
    private static float _slowMultiplier = 1f;

    /// <summary>
    /// Indica si el jugador ha muerto.
    /// Se usa para diferenciar que contador usar en el Update().
    /// </summary>
    private bool _playerDied = false;

    /// <summary>
    /// Almacena el highscore del jugador, leido de un archivo.
    /// </summary>
    private int _highScore = 0;

    /// <summary>
    /// Almacena el componente de control del jugador para cambiarle
    /// la sensibilidad con los settings
    /// </summary>
    private playerControlledCursor _playerCursor;

    /// <summary>
    /// Almacena la sensibilidad del jugador, en un intervalo del [0,10].
    /// </summary>
    private float _cursorSensibility = 5f;

    /// <summary>
    /// Indice que indica que color se esta usando actualmente para el StreakColor.
    /// </summary>
    private int _currentStreakColor = 0;

    /// <summary>
    /// Almacena el componente de UIVibration del texto StreakVibration.
    /// </summary>
    private UIVibration _streakVibration;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta al instaciar el componente.
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            DontDestroyOnLoad(this.gameObject);
            _instance = this;
        }
        else
        {
            // Transferencia de configuración del HUD
            GameManager.Instance.TransferManagerSetup(ActLevelMessage, LevelUpMessage, FadeInBlackScreen, FadeOutBlackScreen, FadeInBlueScreen, FadeOutBlueScreen, HabilityLiquid, HabilityShadow, Barrel, Lifes, Bullets, ScoreText, StreakMultiplier, StreakColors, StreakBar, LevelBar, VictoryMusic, NextLevel, TiempoEsperaRespawn, TiempoEsperaSiguienteNivel, MeleeCooldown, highScoreTextUI, streakText);
        }

        foreach (GameObject obj in streakText) // Desactiva los indicadores de puntos 
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// Si hay un highscore, lo muestra en pantalla y de lo contrario lo marca como cero
    /// Método llamado una vez después del Awake(), una vez otros componentes ya
    /// se han inicializado correctamente.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// Desactiva el componente para evitar que se corra updates.
    /// </summary>
    private void Start()
    {
        if (this != _instance)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de este manager al que ya existe.
            // Esto permitirá al GameManager real mantener su estado interno
            // pero acceder a los elementos de la nueva escena
            // o bien olvidar los de la escena previa de la que venimos
            
            // Mensaje de actualización de escena para la carga inicial
            GameManager.Instance.NewSceneUpdate();

            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Se desactiva el componente para no usar el Update() hasta que sea necesario un contador.
            this.enabled = false;
            Init();
        } // if-else somos instancia nueva o no.
        if (SceneManager.GetActiveScene().name == "Menu") LoadScore();
    }

    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    }

    /// <summary>
    /// Se llama cada frame si el componente está activo.
    /// Realiza distintas esperas en función de _playerDied para los métodos de Respawn() y LevelEnds().
    /// </summary>
    private void Update()
    {
        if (_playerDied) // proviene de Respawn()
        {
            if (Time.time - _t > TiempoEsperaRespawn)
            {
                this.enabled = false;
                ReinicioEscena();
            }
        }
        else // proviene de LevelEnds().
        {
            if (Time.time - _t > TiempoEsperaSiguienteNivel)
            {
                if (InputManager.HasInstance()) InputManager.Instance.ActivarInput();
                this.enabled = false;
                ChangeScene(NextLevel);
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos
    #region Propiedades de acceso
    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Propiedad para acceder al valor de _slowMultiplier.
    /// </summary>
    public static float SlowMultiplier
    {
        get
        {
            return _slowMultiplier;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }
    #endregion

    #region Funcionalidad manejo de escenas publicos

    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene

    /// <summary>
    /// Método que se encarga de llevar los procesos tras la muerte del jugador.
    /// Desactiva el input del jugador, inicia un FadeIn de pantalla negra y activa este componente para que en
    /// el Update() se lleve un contador para esperar al FadeIn y luego reiniciar la escena.
    /// Se llama desde HealthChanger, cuando muere el jugador.
    /// </summary>
    public void Respawn()
    {
        // Reinicio de las stats del jugador para que empiecen completas tras reiniciarse la escena.
        _vidaJugador = VIDABASEJUGADOR;
        _municionJugador = MUNICIONBASEJUGADOR;
        _currentStreakColor = 0;
        if (LevelManager.HasInstance())
        {
            _totalPoints = LevelManager.Instance.GetPointsAtStartOfLevel();
            _totalDeaths = LevelManager.Instance.GetKillsAtStartOfLevel();
        }
        else
        {
            _totalPoints = 0;
            _totalDeaths = 0;
        }


        // Animación de pantalla negra.
        if (InputManager.HasInstance()) InputManager.Instance.DesactivarInput();
        if (FadeInBlackScreen != null) FadeInBlackScreen.enabled = true;



        _t = Time.time;
        _playerDied = true;
        this.enabled = true; // comienza el temporizador en el update
    }

    /// <summary>
    /// Este metodo gestiona el final de un nivel.
    /// Se llama cuando hay una victoria de nivel (se mantiene vida y puntaje)
    /// </summary>
    public void LevelEnds()
    {
        _currentStreakColor = 0;

        // Feedback de victoria
        if (InputManager.HasInstance()) InputManager.Instance.DesactivarInput();
        if (FadeInBlackScreen != null) FadeInBlackScreen.enabled = true;
        if (AudioManager.HasInstance()) AudioManager.Instance.PlayMusic(VictoryMusic);

        _t = Time.time;
        _playerDied = false;
        this.enabled = true; // inicia contador en Update().

        SaveScore(_totalPoints);
    }

    /// <summary>
    /// Se llama al ganar el juego, tras derrotar a Suzie.
    /// Igual que LevelEnds pero reinicia las estadisticas.
    /// </summary>
    public void GameEnds()
    {
        LevelEnds(); // inicia el fin de nivel y guarda puntos
        ResetStats(); // reset de stats
    }

    #endregion

    #region Metodos únicos del Hud

    /// <summary>
    /// Este metodo actualiza los puntos, y su HUD.
    /// </summary>
    public void UpdateScoreHUD(int cambioDePuntos)
    {
        _totalPoints += cambioDePuntos;
        if (ScoreText != null) ScoreText.text = _totalPoints.ToString();

    }

    public void SpawnPointIndicator(Vector3 position, int cambioDePuntos)
    {
        if (streakText.Length > 0)
        {
            //Setear el punto
            streakText[actualtext].SetActive(true);
            streakText[actualtext].GetComponent<PointIndicator>().SpawnHere(position, cambioDePuntos);

            //Gestionar lista
            actualtext++;
            if (actualtext >= streakText.Length) actualtext = 0;
        }
    }

    /// <summary>
    /// Este método actualiza la racha de muertes y su HUD.
    /// </summary>
    /// <param name="NuevoScoreJugador"></param>
    public void UpdateStreakMultiplierHUD(int Streak)
    {
        if (StreakMultiplier != null)
        {
            StreakMultiplier.text = "x" + Streak.ToString();
            if (StreakColors.Length > 0)
            {
                // Actualización de color y vibración
                if (_currentStreakColor < StreakColors.Length - 1 && Streak >= StreakColors[_currentStreakColor].StreakToChangeColor)
                {
                    _currentStreakColor++;
                    UpdateStreakMultiplierEffects();
                }
                else if (_currentStreakColor > 0 && Streak < StreakColors[_currentStreakColor-1].StreakToChangeColor)
                {
                    _currentStreakColor--;
                    UpdateStreakMultiplierEffects();
                }
            }
        }
    }

    /// <summary>
    /// Este metodo actualiza la vida y su HUD.
    /// </summary>
    public void UpdateHealthHUD(int NuevaVidaJugador)
    {
        int diff = NuevaVidaJugador - _vidaJugador;
        _vidaJugador = NuevaVidaJugador;

        for (int i = 0; i < Lifes.Length; i++)
        {
            if (Lifes[i] != null)
            {
                // i recorre corazones enteros
                // cada corazon son 2 PV

                // en cada corazón hay que ver si:
                // 1) hay que rellenarlo completo o dejarlo vacio
                // 2) hay que hacer una animación, de daño o de cura
                // para decidir entre si he recibido daño o me he curado seguramente sea
                // más intuitivo calcular  diff = NuevaVidaJugador - vidaAnterior, si es (+) se ha curado, si es (-) ha perdido vida


                // lo primordal al analizar cada corazón es ver si se tiene que hacer la animacion en este o no
                // verlo es más sencillo si primero verificamos el signo de diff

                // Para diff < 0:
                // si 2i <= NuevaVidaJugador < 2(i+1) el corazon necesita animacion

                // if NuevaVidaJugador >= 2(ì+1) entonces se pinta el corazon lleno y listo
                // else if NuevaVidaJugador < 2i entonces se pinta el corazon vacio y listo
                // else, (hace falta animacion) hago un switch con (NuevaVidaJugador - 2i)
                // case 0: animacion corazon medio a corazon vacio
                // case 1: animacion corazon completo a corazon medio
                // --> Aqui se asume que los cambios son de 1 PV, habría que ver con Suzie como queda la perdida de vida

                // Para diff > 0:
                // si 2i < NuevaVidaJugador <= 2(i+1) el corazon necesita animacion
                
                // if NuevaVidaJugador > 2(i+1) se pinta el corazon lleno y listo
                // else if NuevaVidaJugador <= 2i se pinta el corazon vacio y listo
                // else, (hace falta animacion) hago un switch con (NuevaVidaJugador - 2i)
                // case 1: animacion de corazon vacio a corazon medio
                // case 2: animacion de corazon medio a corazon lleno
                // --> De nuevo asumimos que los cambios son de 1 PV y podría quedar raro con curas mayores

                // Para diff = 0: (posible en la transición de escenas)
                // if NuevaVidaJugador >= 2(i+1) se pinta el corazón lleno y listo
                // else if NuevaVidaJugador <= 2i se pinta el corazon vacio y listo
                // else -> mitad de corazon
                if (diff < 0)
                {
                    if (NuevaVidaJugador >= 2 * (i + 1)) Lifes[i].FullHeart();
                    else if (NuevaVidaJugador < 2 * i) Lifes[i].EmptyHeart();
                    else
                    {
                        switch(NuevaVidaJugador - 2 * i)
                        {
                            case 0:
                                Lifes[i].HitToEmpty();
                                break;
                            case 1:
                                Lifes[i].HitToHalf();
                                break;
                        }
                    }
                }
                else if (diff > 0)
                {
                    if (NuevaVidaJugador > 2 * (i + 1)) Lifes[i].FullHeart();
                    else if (NuevaVidaJugador <= 2 * i) Lifes[i].EmptyHeart();
                    else
                    {
                        switch(NuevaVidaJugador - 2 * i)
                        {
                            case 1:
                                Lifes[i].HealToHalf();
                                break;
                            case 2:
                                Lifes[i].HealToFull();
                                break;
                        }
                    }
                }
                else
                {
                    if (NuevaVidaJugador >= 2 * (i + 1)) Lifes[i].FullHeart();
                    else if (NuevaVidaJugador <= 2*i) Lifes[i].EmptyHeart();
                    else Lifes[i].HalfHeart();
                }
            }
        }
    }

    /// <summary>
    /// Este metodo actualiza las balas y su HUD
    /// </summary>
    public void UpdateAmmoHUD(int NuevaMunicionJugador)
    {
        bool recarga = _municionJugador < NuevaMunicionJugador;

        Animator barrelAnimator = Barrel.GetComponent<Animator>();

        _municionJugador = NuevaMunicionJugador;
        for (int i = 0; i < Bullets.Length; i++)
        {
            if (Bullets[i] != null)
            {
                Animator bulletAnimator = Bullets[i].GetComponent<Animator>();
                if (bulletAnimator != null)
                {
                    if ((i < _municionJugador)) bulletAnimator.Play("BulletIdle", 0, 0f);
                    else if ((i == _municionJugador) && !recarga) bulletAnimator.Play("Bullet", 0, 0f);
                }
                else Debug.Log("Falta animator en una de las bullets del barril de recarga");
                if (barrelAnimator != null)
                {
                    if (recarga)
                    {
                        barrelAnimator.Play("RevolverAntiClock", 0, 0f);
                    }
                    else barrelAnimator.Play("RevolverClock", 0, 0f);
                }
            }
        }
        //Debug.Log(_municionJugador);
    }

    /// <summary>
    /// Actualiza el fill ammount de la imagen del liquido de la habilidad.
    /// Se irá llamando durante la duración de la habilidad para indicar cuanto tiempo le queda.
    /// </summary>
    /// <param name="fillAmmount"></param>
    public void UpdateTimeHabilityLiquid(float fillAmmount)
    {
        if (HabilityLiquid != null) HabilityLiquid.UpdateImageFillAmmount(fillAmmount);
    }

    /// <summary>
    /// Actualiza el fill ammount de la imagen del tiempo de la racha.
    /// Se llamará en cada comprobación.
    /// </summary>
    /// <param name="fillAmmount"></param>
    public void UpdateStreakBar(float fillAmmount)
    {
        if (StreakBar != null) StreakBar.UpdateImageFillAmmount(fillAmmount);
    }

    /// <summary>
    /// Actualiza el fill ammount de la imagen del nivel de la habilidad.
    /// se llamará en 
    /// </summary>
    /// <param name="fillAmmount"></param>
    public void UpdateLevelBar(float fillAmmount)
    {
        if (LevelBar != null)
        {
            LevelBar.UpdateImageFillAmmount(fillAmmount);
        }
    }

    /// <summary>
    /// Actualiza en el HUD el texto que dice el nivel actual de la habilidad.
    /// </summary>
    /// <param name="a">Este sería el nuevo valor del nivel</param>
    public void UpdateActLevelText(int a)
    {
        if (ActLevelMessage != null)
        {
            ActLevelMessage.text = " " + a + " ";
        }
    }

    /// <summary>
    /// Llama al método ColorChanging de LevelUpMessage (solo hace que se active dicho componente)
    /// </summary>
    public void ActivateLevelUpText()
    {
        if (LevelUpMessage != null)
        {
            LevelUpMessage.ColorChanging();
        }
    }

    /// <summary>
    /// Actualiza el fill ammount de la imagen de la sombra de la habilidad.
    /// Se irá llamando durante el cooldown de la habilidad para indicar cuánto tiempo queda.
    /// </summary>
    /// <param name="fillAmmount"></param>
    public void UpdateTimeHabilityShadow(float fillAmmount)
    {
        if (HabilityShadow != null) HabilityShadow.UpdateImageFillAmmount(fillAmmount);
    }

    /// <summary>
    /// Empieza el FadeIn de la pantalla azul.
    /// Se llamará cuando empiece la habilidad del jugador, desde su correspondiente script.
    /// </summary>
    public void StartFadeInBlueScreen()
    {
        if (FadeInBlueScreen != null) FadeInBlueScreen.enabled = true;
    }

    /// <summary>
    /// Empieza el FadeOut de la pantalla azul.
    /// Se llamará cuando acabe la habilidad del jugador, desde su correspondiente script.
    /// </summary>
    public void StartFadeOutBlueScreen()
    {
        if (FadeOutBlueScreen != null) FadeOutBlueScreen.enabled = true;
    }

    /// <summary>
    /// Método público que llama al ImageFill que controla la representación del cooldown del ataque melee, para que se actualice al valor que le corresponda.
    /// </summary>
    /// <param name="fillAmount"></param>
    public void UpdateMeleeCooldownShadow(float fillAmount)
    {
        if (MeleeCooldown != null) MeleeCooldown.UpdateImageFillAmmount(fillAmount);
    }
    #endregion

    #region Metodos transferencia de informacion

    /// <summary>
    /// Transfiere datos importantes de un GameManager que ha de destruirse al activo.
    /// Reconfigura el HUD para incluir el de la escena actual.
    /// </summary>
    public void TransferManagerSetup(TextMeshProUGUI ActLevelMessage, ChangeColorAndHide LevelUpMessage, FadeColor FadeInBlackScreen, FadeColor FadeOutBlackScreen, FadeColor FadeInBlueScreen, FadeColor FadeOutBlueScreen,
        ImageFill HabilityLiquid, ImageFill HabilityShadow, GameObject Barrel , HeartUI[] Lifes, GameObject[] Bullets, TextMeshProUGUI ScoreText, TextMeshProUGUI StreakMultiplier, StreakColor[] StreakColors,
        ImageFill StreakBar, ImageFill LevelBar, AudioClip VictoryMusic,
        int NextLevel, float TiempoEsperaRespawn, float TiempoEsperaSiguienteNivel, ImageFill MeleeCooldown, TextMeshProUGUI highScoreTextUI,
        GameObject[] streakText)
    {
        this.ActLevelMessage = ActLevelMessage;
        this.LevelUpMessage = LevelUpMessage;
        this.FadeInBlackScreen = FadeInBlackScreen;
        this.FadeOutBlackScreen = FadeOutBlackScreen;
        this.FadeInBlueScreen = FadeInBlueScreen;
        this.FadeOutBlueScreen = FadeOutBlueScreen;
        this.HabilityLiquid = HabilityLiquid;
        this.HabilityShadow = HabilityShadow;
        this.Barrel = Barrel;
        this.Lifes = Lifes;
        this.Bullets = Bullets;
        this.ScoreText = ScoreText;
        this.StreakMultiplier = StreakMultiplier;
        this.StreakColors = StreakColors;
        this.StreakBar = StreakBar;
        this.LevelBar = LevelBar;
        this.VictoryMusic = VictoryMusic;
        this.NextLevel = NextLevel;
        this.TiempoEsperaRespawn = TiempoEsperaRespawn;
        this.TiempoEsperaSiguienteNivel = TiempoEsperaSiguienteNivel;
        this.MeleeCooldown = MeleeCooldown;
        this.highScoreTextUI = highScoreTextUI;
        this.streakText = streakText;
    }

    /// <summary>
    /// Cuando un GameManager ya existe, esta función es llamada para actualizar
    /// cosas necesarias en la nueva escena desde el GameManager original.
    /// Sirve como indicador de que se ha cargado una nueva escena (que se llama solo
    /// cuando al cargarla hay otro GameManager).
    /// </summary>
    public void NewSceneUpdate()
    {
        // Actualizar HUD del jugador
        UpdateHealthHUD(_vidaJugador);
        UpdateScoreHUD(0);

        // Reiniciar flujo del tiempo (es posible salir de una escena con la habilidad activada, si no se reinicia,
        // se podría mantener la habilidad siempre activa.
        SlowShotOff();

        // Realizar el FadeOut de la pantalla negra al inicio de la escena solo si estaba activo (valor 1).
        this.enabled = false;
        if (FadeOutBlackScreen != null) FadeOutBlackScreen.enabled = true;

        if (InputManager.HasInstance()) InputManager.Instance.ActivarInput();

        Init();
        if (_playerCursor != null)
        {
            _playerCursor.SetCursorSpeed(_cursorSensibility);
        }
    }

    /// <summary>
    /// Método llamado para reiniciar las estadisticas del jugador.
    /// </summary>
    public void ResetStats()
    {
        _vidaJugador = VIDABASEJUGADOR;
        _municionJugador = MUNICIONBASEJUGADOR;
        _totalDeaths = 0;
        _totalPoints = 0;
        _currentStreakColor = 0;
    }

    /// <summary>
    /// Este metodo actualiza la cantidad de muertes y los niveles de habilidad del jugador.
    /// </summary>
    public void AnEnemyDied()
    {
        _totalDeaths += 1;
        if (LevelManager.HasInstance() && LevelManager.Instance.PlayerTransform() != null)
        {
            playerSlowShot playerSlSh = LevelManager.Instance.PlayerTransform().GetComponent<playerSlowShot>();
            if (playerSlSh != null) playerSlSh.PlayerKill(_totalDeaths);
        }
    }

    /// <summary>
    /// Este metodo devuelve el valor int almacenado en _levelPoints, entendido como puntos iniciales
    /// </summary>
    public int TransferInitialPoints()
    {
        return _totalPoints;
    }

    /// <summary>
    /// Este método devuelve el número de muertes almacenado en el GameManager.
    /// </summary>
    public int TransferTotalDeaths()
    {
        return _totalDeaths;
    }

    /// <summary>
    /// Método para preguntarle al GameManager la vida a la que debe aparecer el jugador.
    /// Lo llama el HealthChanger del jugador al inicializarse.
    /// </summary>
    /// <returns></returns>
    public int InitHealthChanger()
    {
        return _vidaJugador;
    }
    #endregion

    #region Funcionalidad SlowShot y Pausa
    /// <summary>
    /// Método público que modifica la velocidad de ralentización consecuencia de la activación de la habilidad del jugador
    /// </summary>
    public void SlowShotOn()
    {
        _slowMultiplier = 0.25f;
        StartFadeInBlueScreen();

        if (TimeAbilityAnimator != null) TimeAbilityAnimator.SetBool("AbilityActive", true);

        if (AudioManager.HasInstance())
            AudioManager.Instance.SetSlowMotionAudio(true);
    }

    /// <summary>
    /// Método público que modifica la velocidad de ralentización consecuencia de la desactivación de la habilidad del jugador
    /// </summary>
    public void SlowShotOff()
    {
        ResumeGame();
        StartFadeOutBlueScreen();
        if (TimeAbilityAnimator != null) TimeAbilityAnimator.SetBool("AbilityActive", false);

        if (AudioManager.HasInstance())
            AudioManager.Instance.SetSlowMotionAudio(false);
    }

    public void PauseGame()
    {
        _slowMultiplier = 0;
    }

    public void ResumeGame()
    {
        _slowMultiplier = 1.00f;
    }
    // NOTA: Los niveles de habilida del jugador se actualizan también en AnEnemyDied(), en la región de Transferencia de información.
    #endregion

    #region Funcionalidad settings

    /// <summary>
    /// Método público para aumentar (en 0.01) la sensibilidad del cursor del jugador.
    /// </summary>
    public void LitSensIncrease()
    {
        _cursorSensibility += 0.1f;
        if (_cursorSensibility > 10f) _cursorSensibility = 10f;
        if (_playerCursor != null)
        {
            _playerCursor.SetCursorSpeed(_cursorSensibility);
        }
    }

    /// <summary>
    /// Método público para aumentar (en 0.1) la sensibilidad del cursor del jugador.
    /// </summary>
    public void BigSensIncrease()
    {
        _cursorSensibility += 1f;
        if (_cursorSensibility > 10f) _cursorSensibility = 10f;
        if (_playerCursor != null)
        {
            _playerCursor.SetCursorSpeed(_cursorSensibility);
        }
    }

    /// <summary>
    /// Método público para disminuir (en 0.01) la sensibilidad del cursor del jugador.
    /// </summary>
    public void LitSensDecrease()
    {
        _cursorSensibility -= 0.1f;
        if (_cursorSensibility < 0) _cursorSensibility = 0;
        if (_playerCursor != null)
        {
            _playerCursor.SetCursorSpeed(_cursorSensibility);
        }
    }

    /// <summary>
    /// Método público para disminuir (en 0.1) la sensibilidad del cursor del jugador.
    /// </summary>
    public void BigSensDecrease()
    {
        _cursorSensibility -= 1f;
        if (_cursorSensibility < 0) _cursorSensibility = 0;
        if (_playerCursor != null)
        {
            _playerCursor.SetCursorSpeed(_cursorSensibility);
        }
    }

    /// <summary>
    /// Método para leer la sensibilidad actual para el cursor del GameManager.
    /// </summary>
    /// <returns></returns>
    public float GetSens()
    {
        return _cursorSensibility;
    }

    #endregion
    public void SaveScore(int score)
    {
        string data = score.ToString();
        string path = Application.persistentDataPath + "/Score.txt";

        if (File.Exists(path))
        {
            string contenido = File.ReadAllText(path);
            int savedScore = int.Parse(contenido);

            if (savedScore >= score)
            {
                return;
            }
        }
        File.WriteAllText(path, data);
        Debug.Log("Puntuación guardada en: " + path);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// Se llama desde el Start() y cada vez que se carga una escena nueva.
    /// </summary>
    private void Init()
    {
        if (LevelManager.HasInstance())
        {
            Transform _player = LevelManager.Instance.PlayerTransform();
            if (_player != null)
            {
                _playerCursor = _player.GetComponentInChildren<playerControlledCursor>();
            }
        }
    }

    /// <summary>
    /// Reinicia la escena actual, activa el FadeOut de la pantalla negra y reactiva el input del jugador.
    /// </summary>
    private void ReinicioEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    

    private void LoadScore()
    {

        if (highScoreTextUI == null)
        {
            highScoreTextUI.text = "0";
            return;
        }

        string path = Application.persistentDataPath + "/Score.txt";

        if (File.Exists(path))
        {

            string file = File.ReadAllText(path);


            highScoreTextUI.text = file;
            _highScore = int.Parse(file);

        }
        //Ponerla en la UI

    }

    /// <summary>
    /// Actualiza el color y la intensidad de vibración del texto del multiplicador de racha.
    /// (!) No verifica si _currentStreakColor es correcta, ha de serlo (se controla en el método
    /// de UpdateStreakMultiplierHUD() ).
    /// </summary>
    private void UpdateStreakMultiplierEffects()
    {
        StreakMultiplier.color = StreakColors[_currentStreakColor].Color;

        // intento de inicializar la vibración (puede ya estar almacenado)
        if (_streakVibration == null)
        {
            _streakVibration = StreakMultiplier.gameObject.GetComponent<UIVibration>();
        }

        // si se consigue o si ya estaba guardada
        if (_streakVibration != null)
        {
            _streakVibration.ChangeIntensity(StreakColors[_currentStreakColor].NewVibration);
        }
    }
    #endregion
} // class GameManager 
// namespace