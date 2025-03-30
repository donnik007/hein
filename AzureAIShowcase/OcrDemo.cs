using System;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace AzureAIShowcase
{
    public static class OcrDemo
    {
        // Replace with your own key and endpoint
        private static readonly string ComputerVisionKey = Environment.GetEnvironmentVariable("OCR_API");
        private static readonly string ComputerVisionEndpoint = "https://heinocr.cognitiveservices.azure.com/";

        public static async Task RunAsync()
        {
            Console.WriteLine("\n=== OCR (Optical Character Recognition) Demo ===");
            
            if (ComputerVisionKey == "YOUR_COMPUTER_VISION_KEY")
            {
                Console.WriteLine("Please set your Computer Vision key and endpoint in the OcrDemo.cs file.");
                return;
            }

            Console.Write("Enter image URL containing text: ");
            string imageUrl = Console.ReadLine() ?? "https://moderatorsampleimages.blob.core.windows.net/samples/sample1.jpg";
            
            try
            {
                ComputerVisionClient client = new ComputerVisionClient(
                    new ApiKeyServiceClientCredentials(ComputerVisionKey))
                {
                    Endpoint = ComputerVisionEndpoint
                };
                
                Console.WriteLine("\nPerforming OCR...");
                
                // Read text from image
                var textHeaders = await client.ReadAsync(imageUrl);
                string operationLocation = textHeaders.OperationLocation;
                
                // Get the operation ID from the operation location
                string operationId = operationLocation.Substring(operationLocation.LastIndexOf('/') + 1);
                
                // Wait for the operation to complete
                ReadOperationResult results;
                do
                {
                    results = await client.GetReadResultAsync(Guid.Parse(operationId));
                    await Task.Delay(1000);
                } while (results.Status == OperationStatusCodes.Running || 
                         results.Status == OperationStatusCodes.NotStarted);
                
                // Display results
                Console.WriteLine("\nResults:");
                
                foreach (var page in results.AnalyzeResult.ReadResults)
                {
                    foreach (var line in page.Lines)
                    {
                        Console.WriteLine(line.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
