using System;
using System.Threading.Tasks;
using Azure;
using Azure.AI.TextAnalytics;

namespace AzureAIShowcase
{
    public static class TextAnalyticsDemo
    {
        // Replace with your own key and endpoint
        private static readonly string TextAnalyticsKey = Environment.GetEnvironmentVariable("TEXTANAPI");
        private static readonly string TextAnalyticsEndpoint = "https://textanhein.cognitiveservices.azure.com/";

        public static async Task RunAsync()
        {
            Console.WriteLine("\n=== Text Analytics Demo ===");
            
            if (TextAnalyticsKey == "YOUR_TEXT_ANALYTICS_KEY")
            {
                Console.WriteLine("Please set your Text Analytics key and endpoint in the TextAnalyticsDemo.cs file.");
                return;
            }

            Console.Write("Enter text for sentiment analysis: ");
            string text = Console.ReadLine() ?? "I had a wonderful experience! The rooms were beautiful and the service was amazing.";
            
            try
            {
                var credential = new AzureKeyCredential(TextAnalyticsKey);
                var client = new TextAnalyticsClient(new Uri(TextAnalyticsEndpoint), credential);
                
                // Analyze sentiment
                Console.WriteLine("\nAnalyzing sentiment...");
                DocumentSentiment sentimentResult = await client.AnalyzeSentimentAsync(text);
                
                Console.WriteLine($"Sentiment: {sentimentResult.Sentiment}");
                Console.WriteLine($"Positive score: {sentimentResult.ConfidenceScores.Positive:P1}");
                Console.WriteLine($"Neutral score: {sentimentResult.ConfidenceScores.Neutral:P1}");
                Console.WriteLine($"Negative score: {sentimentResult.ConfidenceScores.Negative:P1}");
                
                // Extract key phrases
                Console.WriteLine("\nExtracting key phrases...");
                var keyPhrasesResponse = await client.ExtractKeyPhrasesAsync(text);
                
                Console.WriteLine("Key phrases:");
                foreach (string keyPhrase in keyPhrasesResponse.Value)
                {
                    Console.WriteLine($"  {keyPhrase}");
                }
                
                // Detect language
                Console.WriteLine("\nDetecting language...");
                var languageResponse = await client.DetectLanguageAsync(text);
                
                Console.WriteLine($"Detected language: {languageResponse.Value.Name} ({languageResponse.Value.Iso6391Name}) with confidence {languageResponse.Value.ConfidenceScore:P1}");
                
                // Recognize entities
                Console.WriteLine("\nRecognizing entities...");
                var entityResponse = await client.RecognizeEntitiesAsync(text);
                
                Console.WriteLine("Named entities:");
                foreach (var entity in entityResponse.Value)
                {
                    Console.WriteLine($"  {entity.Text} ({entity.Category}) - Confidence: {entity.ConfidenceScore:P1}");
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
