using System;
using System.Threading.Tasks;

namespace AzureAIShowcase
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Azure AI Services Showcase");
            Console.WriteLine("==========================");
            
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nPlease select a service to demo:");
                Console.WriteLine("1. Text Translation Service");
                Console.WriteLine("2. OCR (Optical Character Recognition)");
                Console.WriteLine("3. Face Detection and Analysis");
                Console.WriteLine("4. Text Analytics (Sentiment Analysis)");
                Console.WriteLine("5. Speech (Speech-to-Text, Text-to-Speech)");
                Console.WriteLine("0. Exit");
                
                Console.Write("\nYour choice: ");
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        await TranslationDemo.RunAsync();
                        break;
                    case "2":
                        await OcrDemo.RunAsync();
                        break;
                    case "3":
                        await FaceDetectionDemo.RunAsync();
                        break;
                    case "4":
                        await TextAnalyticsDemo.RunAsync();
                        break;
                    case "5":
                        await SpeechDemo.RunAsync();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            
            Console.WriteLine("\nThank you for using the Azure AI Services Showcase!");
        }
    }
}
