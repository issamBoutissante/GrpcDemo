﻿using Grpc.Net.Client;
using GrpcDemo.Shared;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace GrpcDemo.WpfClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Button click event handler to trigger the gRPC call
        private void CallServerButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable button during the call to prevent multiple requests
            CallServerButton.IsEnabled = false;

            // Get the name from the TextBox
            string name = NameTextBox.Text;

            // Run the gRPC call in a background thread
            Task.Run(() => CallGrpcServerAsync(name));
        }

        // Method to call the gRPC server asynchronously
        private async Task CallGrpcServerAsync(string name)
        {
            try
            {
                // Create a channel pointing to the gRPC server address
                var channel = GrpcChannel.ForAddress("https://localhost:7259");

                // Create the Greeter client from the shared library
                var client = new Greeter.GreeterClient(channel);

                // Call the SayHello method with the user's name
                var reply = await client.SayHelloAsync(new HelloRequest { Name = name });

                // Use Dispatcher to update the UI thread
                Dispatcher.Invoke(() =>
                {
                    // Set the response in the Label
                    ResponseLabel.Content = $"Server Response: {reply.Message}";
                    // Re-enable the button
                    CallServerButton.IsEnabled = true;
                });
            }
            catch (Exception ex)
            {
                // Handle errors and update the UI
                Dispatcher.Invoke(() =>
                {
                    ResponseLabel.Content = $"Error: {ex.Message}";
                    CallServerButton.IsEnabled = true;
                });
            }
        }
    }
}
