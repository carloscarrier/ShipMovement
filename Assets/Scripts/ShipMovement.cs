using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    //Velocidade dos movimentos frente/tras, esquerda/direita, cima/baixo;
    public float forwardSpeed = 25f;
    public float horizontalSpeed = 7.5f;
    public float verticalSpeed = 5f;

    public float forAcceleration = 2.5f; 
    public float horizontalAcceleration = 2f; 
    public float verticalAcceleration = 2f;

    //Recebem os valores finais para a execu��o dos movimentos
    private float executeForwardSpeed;  
    private float executeHorizontalSpeed;
    private float executeVerticalSpeed;

    //Acrobacia
    public float rollSpeed = 90f, rollAcceleration = 3.5f;

    private float rollInput;

    //Sensibilidade do mouse;
    public float lookRateSpeed = 90f;

    //A dire��o de angulo da nave est� onde est� o mouse. Assim � importante local o mouse e verificar sua diferen�a para o centro da tela. Por isso � usado um Vector2.
    private Vector2 lookInput;
    private Vector2 screenCenter;
    private Vector2 mouseDistance;

    //No m�todo Start() o "centro da tela" tanto no X quando no Y � definido. Ele n�o ser� um ponto literal no meio da tela, mas sim um pequeno espa�o para poder gerar uma suavidade no movimento;
    void Start()
    {
        screenCenter.x = Screen.width * .5f; 
        screenCenter.y = Screen.height * .5f
;       
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        //Aqui � pego a posi��o do mouse;
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;
        //Logo ap�s pegar a posi��o do mouse, buscamos a diferen�a entre ela com aquele espa�o do centro da tela definido no Start();
        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, .2f); //Com o m�todo ClampMagnitude() damos a distancia m�xima do vector criado com a distancia do mouse at� o centro da tela, para o movimento da c�mera ganhar suavidade. 
                                                                    //Quanto menor for o valor onde est� o ".2f", menos suaviadade ter�. 

        //Abaixo � adicinado um Lerp para suavizar o movimento acrob�tico. O Lerp retorna um movimento entre dois pontos durante um tempo T;
        rollInput = Mathf.Lerp(rollInput, Input.GetAxis("Roll"), rollAcceleration * Time.deltaTime);

        Debug.Log(Input.GetAxisRaw("Roll"));

        //Abaixo defini-se a rota��o da nave atrav�s das atribui��es feitas da posi��o do mouse at� o movimento acrob�tico;
        transform.Rotate((-mouseDistance.y * lookRateSpeed * Time.deltaTime), (mouseDistance.x * lookRateSpeed * Time.deltaTime), (rollInput * lookRateSpeed * Time.deltaTime),
                          Space.Self);

        //Abaixo � feita as atribui��es da movimenta��o pelos Inputs e adicionada suaviza��o com o Lerp;
        executeForwardSpeed = Mathf.Lerp(executeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forAcceleration * Time.deltaTime);
        Debug.Log(Input.GetAxisRaw("Vertical"));
        executeHorizontalSpeed = Mathf.Lerp(executeHorizontalSpeed, Input.GetAxisRaw("Horizontal") * horizontalSpeed, horizontalAcceleration * Time.deltaTime);
        Debug.Log(Input.GetAxisRaw("Horizontal"));
        executeVerticalSpeed = Mathf.Lerp(executeVerticalSpeed, Input.GetAxisRaw("Hover") * verticalSpeed, verticalAcceleration * Time.deltaTime);
        Debug.Log(Input.GetAxisRaw("Hover"));

        //Depois de feitas todas as atribui��es para a movimenta��o, o movimento � em si executado. 
        transform.position += transform.forward * executeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * executeHorizontalSpeed * Time.deltaTime) + (transform.up * executeVerticalSpeed * Time.deltaTime);
    }
}
