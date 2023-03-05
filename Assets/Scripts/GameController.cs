using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour
{

    private InputData _inputData;
    public GameObject spider;

    public GameObject xrRig;

    private bool spawned;
    private bool spawnedGround;

    public TextMeshProUGUI spiderInformation;
    public int tableSpiders;
    public int floorSpiders;

    public int currentLevel;

    private GameObject[] spidersList;

    public bool spiderToggle;

    public float spiderSize;

    public Text messageReceiver;

    private string[] messageText;


    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private TcpClient connectedTcpClient;

    // Start is called before the first frame update
    void Start()
    {
        _inputData = GetComponent<InputData>();
        spawned = false;
        spawnedGround = false;
        tableSpiders = 0;
        floorSpiders = 0;
        updateText(tableSpiders, floorSpiders, spiderInformation);
        // 0 - intro, 1 - Living Room, 2 - Office, Low Height - 3, Medium Height - 4, High Height - 5, 6 - Classroom, 7 - Theater
        currentLevel = 0;

        tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingRequests)); 		
		tcpListenerThread.IsBackground = true; 		
		tcpListenerThread.Start(); 

        messageReceiver.text = "Your IP is: " + GetLocalIPAddress();
    }

    // Update is called once per frame
    void Update()
    {
        //if(currentLevel == 0)
            //messageReceiver.text = messageText;
        if(messageText != null && messageText[0] != "voltar")
        {
            switch(messageText[0]){
                case "iniciar":
                    switch(messageText[1]){
                        case "aranhas":
                            if(messageText[2] == "office"){
                                spiderSize = float.Parse(messageText[3], CultureInfo.InvariantCulture);
                                spiderToggle = true;
                                xrRig.transform.position = new Vector3(20f,0,-0.5f);
                                currentLevel = 2;
                            }
                            else if(messageText[2] == "livingroom"){
                                spiderSize = float.Parse(messageText[3], CultureInfo.InvariantCulture);
                                spiderToggle = true;
                                xrRig.transform.position = new Vector3(0,0,-0.5f);
                                currentLevel = 1;
                            }
                        break;
                        case "alturas":
                            if(messageText[2] == "baixa"){
                                xrRig.transform.position = new Vector3(-69.7f,31.4f,134.9f);
                                currentLevel = 3;
                            }
                            else if(messageText[2] == "media"){
                                xrRig.transform.position = new Vector3(-36.3f,36.6f,134.9f);
                                currentLevel = 4;
                            }
                            else if(messageText[2] == "alta"){
                                xrRig.transform.position = new Vector3(-12.2f,55.4f,134.9f);
                                currentLevel = 5;
                            }
                        break;
                    }
                break;   
                case "aranhas":
                    if(currentLevel == 1 || currentLevel == 2){
                        switch(messageText[1]){
                            case "tamanho":
                                spiderSize = float.Parse(messageText[2], CultureInfo.InvariantCulture);
                            break;
                            case "eliminar":
                                if(spidersList != null)
                                    if(spidersList.Length > 0)
                                        foreach (var sp in spidersList)
                                            Destroy(sp);
                                tableSpiders = 0;
                                floorSpiders = 0;
                                updateText(tableSpiders, floorSpiders, spiderInformation);
                            break;
                            case "criar":
                                if(currentLevel == 1){ 
                                    if(messageText[2] == "chao"){
                                        try{
                                            int vezes = int.Parse(messageText[3]);
                                            for (int i = 0; i < vezes; i++){
                                                Instantiate(spider, new Vector3(UnityEngine.Random.Range(-5.0f,5.3f), 0.0f, UnityEngine.Random.Range(-1.7f,4f)), Quaternion.Euler(0,180,0));
                                                spidersList = GameObject.FindGameObjectsWithTag("Aranhas");
                                                floorSpiders += 1;
                                                updateText(tableSpiders, floorSpiders, spiderInformation);
                                            }
                                        }
                                        catch{
                                            Debug.Log("error");
                                        }
                                        
                                        
                                    }else if(messageText[2] == "mesa"){
                                        try{
                                          int vezes = int.Parse(messageText[3]);
                                          for (int i = 0; i < vezes; i++){
                                            Instantiate(spider, new Vector3(UnityEngine.Random.Range(-1.0f,1.0f), 0.75f, UnityEngine.Random.Range(0.5f,1.2f)), Quaternion.Euler(0,180,0));
                                            spidersList = GameObject.FindGameObjectsWithTag("Aranhas");
                                            tableSpiders += 1;
                                            updateText(tableSpiders, floorSpiders, spiderInformation);
                                          }  
                                        }
                                        catch{
                                            Debug.Log("error");
                                        }
                                        
                                    }

                                } else if(currentLevel == 2){
                                    try{
                                        int vezes = int.Parse(messageText[2]);
                                        for (int i = 0; i < vezes; i++){
                                            Instantiate(spider, new Vector3(UnityEngine.Random.Range(15.0f,25.3f), 0.0f, UnityEngine.Random.Range(-1.7f,4f)), Quaternion.Euler(0,180,0));
                                            spidersList = GameObject.FindGameObjectsWithTag("Aranhas");
                                            floorSpiders += 1;
                                            updateText(tableSpiders, floorSpiders, spiderInformation);
                                        }
                                    }
                                    catch {
                                        Debug.Log("error");
                                    }
                                    
                                }

                            break;
                        }


                    }
                        
                break; 
            }
            Array.Clear(messageText, 0, messageText.Length);
        }
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool valuePrimary) || (messageText != null && messageText[0] == "voltar"))
        {
            if ((valuePrimary && currentLevel != 0) || (messageText != null && messageText[0] == "voltar" && currentLevel != 0)){
                xrRig.transform.position = new Vector3(-7.82f, 18f, -14.54f);
                if (currentLevel < 3)
                    if(spidersList != null)
                        if(spidersList.Length > 0)
                            foreach (var sp in spidersList)
                                Destroy(sp);
                currentLevel = 0;
                tableSpiders = 0;
                floorSpiders = 0;
                updateText(tableSpiders, floorSpiders, spiderInformation);
            }
            //messageText.Clear();
            if(messageText != null)
                Array.Clear(messageText, 0, messageText.Length);

        }


        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool valueGrip))
        {
            if(spiderToggle)
                if(currentLevel == 1){
                    if(valueGrip && spawned == false){
                        Instantiate(spider, new Vector3(UnityEngine.Random.Range(-1.0f,1.0f), 0.75f, UnityEngine.Random.Range(0.5f,1.2f)), Quaternion.Euler(0,180,0));
                        spidersList = GameObject.FindGameObjectsWithTag("Aranhas");
                        spawned = true;
                        tableSpiders += 1;
                        updateText(tableSpiders, floorSpiders, spiderInformation);
                    }
                    else if(!valueGrip)
                        spawned = false;
                }
            
        }
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool valueTrigger))
        {
            if(currentLevel == 3 && valueTrigger)
                xrRig.transform.position = new Vector3(-69.7f, 31.4f, 134.9f);
            else if(currentLevel == 4 && valueTrigger)
                xrRig.transform.position = new Vector3(-36.3f, 36.6f, 134.9f);
            else if(currentLevel == 5 && valueTrigger)
                xrRig.transform.position = new Vector3(-12.2f, 55.4f, 134.9f);
            if(spiderToggle){
                if(currentLevel == 1){
                    if(valueTrigger && spawnedGround == false){
                        Instantiate(spider, new Vector3(UnityEngine.Random.Range(-5.0f,5.3f), 0.0f, UnityEngine.Random.Range(-1.7f,4f)), Quaternion.Euler(0,180,0));
                        spidersList = GameObject.FindGameObjectsWithTag("Aranhas");
                        spawnedGround = true;
                        floorSpiders += 1;
                        updateText(tableSpiders, floorSpiders, spiderInformation);
                    }
                    else if(!valueTrigger)
                        spawnedGround = false;
                }
                else if(currentLevel == 2){
                    if(valueTrigger && spawnedGround == false){
                        Instantiate(spider, new Vector3(UnityEngine.Random.Range(15.0f,25.3f), 0.0f, UnityEngine.Random.Range(-1.7f,4f)), Quaternion.Euler(0,180,0));
                        spidersList = GameObject.FindGameObjectsWithTag("Aranhas");
                        spawnedGround = true;
                        floorSpiders += 1;
                        updateText(tableSpiders, floorSpiders, spiderInformation);
                    }
                    else if(!valueTrigger)
                        spawnedGround = false;
                }
            }
            
            
        }

    }

    public void updateText(int tableSpiders, int floorSpiders, TextMeshProUGUI spiderInformation){
        spiderInformation.text = "Table Spiders: " + tableSpiders.ToString() + "\nFloor Spiders: " + floorSpiders.ToString();
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
    private void ListenForIncommingRequests () { 		
		try { 			
			// Create listener on localhost port 8052. 			
			//tcpListener = new TcpListener(IPAddress.Parse("192.168.1.189"), 8052); 		
            tcpListener = new TcpListener(IPAddress.Parse(GetLocalIPAddress()), 8052); 		
			tcpListener.Start();              
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[1024];  			
			while (true) { 				
				using (connectedTcpClient = tcpListener.AcceptTcpClient()) { 					
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream()) { 						
						int length; 						
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);  							
							// Convert byte array to string message. 							
							string clientMessage = Encoding.ASCII.GetString(incommingData); 							
							Debug.Log("client message received as: " + clientMessage); 	
                            messageText = clientMessage.Split(' ');					
						} 					
					} 				
				} 			
			} 		
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 		
		}     
	}  	

    private void SendMessage() { 		
		if (connectedTcpClient == null) {             
			return;         
		}  		
		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite) {                 
				string serverMessage = "This is a message from your server."; 			
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage); 				
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);               
				Debug.Log("Server sent his message - should be received by client");           
			}       
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	} 
}
