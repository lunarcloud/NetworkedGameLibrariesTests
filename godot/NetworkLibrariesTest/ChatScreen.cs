using Godot;
using System;
using System.Text;
using System.Net;
using NetComms;
using NetComms.Tcp;

public class ChatScreen : Node
{
	private LineEdit messageBox;
	
	public override void _Ready()
	{
		var data = GetNode<GlobalData>("/root/GlobalData");
		messageBox = GetNode<LineEdit>("Message");
		
		messageBox.Connect("text_entered", this, nameof(sendMessage));
		
		// Create a TCP communications provider
		var provider = new TcpProvider(52341);
		
		Console.WriteLine("made provider");

		// Create the connection
		var address = IPAddress.Parse(data.server);
		var connection = provider.CreateClient(address);
		Console.WriteLine($"made connection at {address}");
		
		// Hook connection dropped
		connection.ConnectionDropped += (s, e) =>
		{
			Console.WriteLine("Connection Dropped");
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
			var message = $"{text}";
			Console.WriteLine(message);
			// TODO add to list
		};
		
		// Start the connection
		connection.Start();
	}

	public void sendMessage(String message) {
		// Send the notification
		Console.WriteLine($"Sending: {message}");
		//var messageBytes = Encoding.ASCII.GetBytes(message);
		//connection.SendNotification(messageBytes);
		messageBox.Text = "";
	}
}
