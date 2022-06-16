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

    //Recebem os valores finais para a execução dos movimentos
    private float executeForwardSpeed;  
    private float executeHorizontalSpeed;
    private float executeVerticalSpeed;

    //Acrobacia
    public float rollSpeed = 90f, rollAcceleration = 3.5f;

    private float rollInput;

    //Sensibilidade do mouse;
    public float lookRateSpeed = 90f;

    //A direção de angulo da nave está onde está o mouse. Assim é importante local o mouse e verificar sua diferença para o centro da tela. Por isso é usado um Vector2.
    private Vector2 lookInput;
    private Vector2 screenCenter;
    private Vector2 mouseDistance;

    //No método Start() o "centro da tela" tanto no X quando no Y é definido. Ele não será um ponto literal no meio da tela, mas sim um pequeno espaço para poder gerar uma suavidade no movimento;
    void Start()
    {
        screenCenter.x = Screen.width * .5f; 
        screenCenter.y = Screen.height * .5f
;       
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        //Aqui é pego a posição do mouse;
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;
        //Logo após pegar a posição do mouse, buscamos a diferença entre ela com aquele espaço do centro da tela definido no Start();
        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, .2f); //Com o método ClampMagnitude() damos a distancia máxima do vector criado com a distancia do mouse até o centro da tela, para o movimento da câmera ganhar suavidade. 
                                                                    //Quanto menor for o valor onde está o ".2f", menos suaviadade terá. 

        //Abaixo é adicinado um Lerp para suavizar o movimento acrobático. O Lerp retorna um movimento entre dois pontos durante um tempo T;
        rollInput = Mathf.Lerp(rollInput, Input.GetAxis("Roll"), rollAcceleration * Time.deltaTime);

        Debug.Log(Input.GetAxisRaw("Roll"));

        //Abaixo defini-se a rotação da nave através das atribuições feitas da posição do mouse até o movimento acrobático;
        transform.Rotate((-mouseDistance.y * lookRateSpeed * Time.deltaTime), (mouseDistance.x * lookRateSpeed * Time.deltaTime), (rollInput * lookRateSpeed * Time.deltaTime),
                          Space.Self);

        //Abaixo é feita as atribuições da movimentação pelos Inputs e adicionada suavização com o Lerp;
        executeForwardSpeed = Mathf.Lerp(executeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forAcceleration * Time.deltaTime);
        Debug.Log(Input.GetAxisRaw("Vertical"));
        executeHorizontalSpeed = Mathf.Lerp(executeHorizontalSpeed, Input.GetAxisRaw("Horizontal") * horizontalSpeed, horizontalAcceleration * Time.deltaTime);
        Debug.Log(Input.GetAxisRaw("Horizontal"));
        executeVerticalSpeed = Mathf.Lerp(executeVerticalSpeed, Input.GetAxisRaw("Hover") * verticalSpeed, verticalAcceleration * Time.deltaTime);
        Debug.Log(Input.GetAxisRaw("Hover"));

        //Depois de feitas todas as atribuições para a movimentação, o movimento é em si executado. 
        transform.position += transform.forward * executeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * executeHorizontalSpeed * Time.deltaTime) + (transform.up * executeVerticalSpeed * Time.deltaTime);
    }
}
