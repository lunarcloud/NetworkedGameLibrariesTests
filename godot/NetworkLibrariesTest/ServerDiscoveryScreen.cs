using Godot;
using System;
using System.Net;
using NetDiscovery;
using NetDiscovery.Udp;

public class ServerDiscoveryScreen : Control
{
	IProvider provider;
	IClient client;
	
	public override void _Ready()
	{
		// Create UDP discovery provider (using port 52148)
		provider = new UdpProvider(52148);
		
		// Create a discovery server advertising the server with the identity of "TestServer"
		client = provider.CreateClient();
		client.Discovery += (s, e) => Console.WriteLine($"Discovered: {e.Address}: {e.Identity}");
		
		// Start discovery components
		client.Start();
		provider.Start();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
