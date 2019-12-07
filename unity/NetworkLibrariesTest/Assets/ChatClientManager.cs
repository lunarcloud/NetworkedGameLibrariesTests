using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using NetComms;
using NetComms.Tcp;
using UnityEngine;

public class ChatClientManager : MonoBehaviour
{
    public Transform chatHistory;
    
    public UnityEngine.UI.ScrollRect chatScroll;

    public UnityEngine.UI.Text chatLogTemplate;

    public UnityEngine.UI.InputField messageBox;

    // defaults to 204.48.22.8
    private IPAddress address;

    private IConnection connection;

    private Queue<string> messages = new Queue<string>();

    public void CreateConnection(string address)
    {
        this.address = IPAddress.Parse(address);
        CreateConnection();
    }

    public void CreateConnection(IPAddress address)
    {
        this.address = address;
        CreateConnection();
    }

    public void CreateConnection()
    {
		// Create a TCP communications provider
		var provider = new TcpProvider(52341);
		
		// Create the connection
		connection = provider.CreateClient(address);
		Debug.Log($"made connection with {address}");
		
		// Hook connection dropped
		connection.ConnectionDropped += (s, e) =>
		{
			Debug.Log("Connection Dropped");
		};

		// Hook transactions
		connection.Transaction += (s, e) =>
		{
			e.Transaction.SendResponse(new byte[0]);
		};

		// Hook notifications
		connection.Notification += (s, e) =>
		{
			var text = Encoding.ASCII.GetString(e.Notification);
            messages.Enqueue($"{text}");
		};
		
		// Start the connection
		connection.Start();
    }

    public void CloseConnection() {
        connection = null;
    }

    // Update is called once per frame
    public void SendMessageBoxTextToServer()
    {
        if (messageBox.text == "") return;

        // If not on mobile, make sure the enter key was what ended the input
        #if UNITY_STANDALONE || UNITY_EDITOR
            if(!Input.GetKeyDown(KeyCode.Return)) return;
        #endif

        // Send the notification
		Debug.Log($"Sending: {messageBox.text}");
		var messageBytes = Encoding.ASCII.GetBytes(messageBox.text);
		connection.SendNotification(messageBytes);
        messageBox.Select();
        messageBox.text = "";
        messageBox.ActivateInputField();
    }
    void Update() {
        if (messages.Count > 0) {
            var message = messages.Dequeue();
		    Debug.Log($"Received: {message}");
			addMessageToScreen(message);
        }
    }

    private void addMessageToScreen(string message) {
        UnityEngine.UI.Text text = Instantiate(chatLogTemplate, chatHistory);
        text.text = message;

        Canvas.ForceUpdateCanvases();
        chatScroll.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }
}
