using System;
using System.Threading.Tasks;
using Azure;
using Azure.AI.Translation.Text;

namespace AzureAIShowcase
{
    public static class TranslationDemo
    {
        // Replace with your own key and endpoint
        private static readonly string TranslatorKey = Environment.GetEnvironmentVariable("TRANSAPI");
        private static readonly string TranslatorEndpoint = "https://api.cognitive.microsofttranslator.com/";
        private static readonly string TranslatorRegion = "northeurope";

        public static async Task RunAsync()
        {
            Console.WriteLine("\n=== Text Translation Demo ===");
            
            if (TranslatorKey == "YOUR_TRANSLATOR_KEY")
            {
                Console.WriteLine("Please set your Translator key and region in the TranslationDemo.cs file.");
                return;
            }

            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Translate Text");
            Console.WriteLine("2. Dictionary Lookup");
            Console.WriteLine("3. Dictionary Examples");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine() ?? "1";

            switch (choice)
            {
                case "1":
                    await TranslateTextAsync();
                    break;
                case "2":
                    await DictionaryLookupAsync();
                    break;
                case "3":
                    await DictionaryExamplesAsync();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private static async Task TranslateTextAsync()
        {
            Console.Write("Enter text to translate: ");
            string textToTranslate = Console.ReadLine() ?? "Hello world!";
            
            Console.Write("Enter target language code (e.g., fr, es, de): ");
            string targetLanguage = Console.ReadLine() ?? "fr";
            
            try
            {
                AzureKeyCredential credential = new AzureKeyCredential(TranslatorKey);
                TextTranslationClient client = new TextTranslationClient(credential, TranslatorRegion);
                
                var response = await client.TranslateAsync(targetLanguage, textToTranslate);
                
                Console.WriteLine("\nTranslation Results:");
                Console.WriteLine($"Source Language: {response.Value[0].DetectedLanguage?.Language ?? "unknown"}");
                Console.WriteLine($"Translated Text: {response.Value[0].Translations[0].Text}");
                Console.WriteLine($"Target Language: {response.Value[0].Translations[0].To}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task DictionaryLookupAsync()
        {
            Console.Write("Enter word to look up: ");
            string word = Console.ReadLine() ?? "example";

            Console.Write("Enter source language code (e.g., en): ");
            string sourceLanguage = Console.ReadLine() ?? "en";

            Console.Write("Enter target language code (e.g., fr): ");
            string targetLanguage = Console.ReadLine() ?? "fr";

            try
            {
                AzureKeyCredential credential = new AzureKeyCredential(TranslatorKey);
                TextTranslationClient client = new TextTranslationClient(credential, TranslatorRegion);

                var response = await client.LookupDictionaryEntriesAsync(sourceLanguage, targetLanguage, word).ConfigureAwait(false);
                var dictionaryEntries = response.Value;
                var dictionaryEntry = dictionaryEntries.FirstOrDefault();

                Console.WriteLine($"For the given input {dictionaryEntry?.Translations?.Count} entries were found in the dictionary.");
                Console.WriteLine($"First entry: '{dictionaryEntry?.Translations?.FirstOrDefault()?.DisplayTarget}', confidence: {dictionaryEntry?.Translations?.FirstOrDefault()?.Confidence:P1}.");
            }
            catch (RequestFailedException exception)
            {
                Console.WriteLine($"Error Code: {exception.ErrorCode}");
                Console.WriteLine($"Message: {exception.Message}");
            }
        }

        private static async Task DictionaryExamplesAsync()
        {
            Console.Write("Enter word to find examples for: ");
            string word = Console.ReadLine() ?? "example";

            Console.Write("Enter target word (translation): ");
            string targetWord = Console.ReadLine() ?? "exemple";

            Console.Write("Enter source language code (e.g., en): ");
            string sourceLanguage = Console.ReadLine() ?? "en";

            Console.Write("Enter target language code (e.g., fr): ");
            string targetLanguage = Console.ReadLine() ?? "fr";

            try
            {
                AzureKeyCredential credential = new AzureKeyCredential(TranslatorKey);
                TextTranslationClient client = new TextTranslationClient(credential, TranslatorRegion);

                var inputTextElements = new[] { new InputTextWithTranslation(word, targetWord) };
                var response = await client.LookupDictionaryExamplesAsync(sourceLanguage, targetLanguage, inputTextElements).ConfigureAwait(false);
                var dictionaryEntries = response.Value;
                var dictionaryEntry = dictionaryEntries.FirstOrDefault();

                Console.WriteLine($"For the given input {dictionaryEntry?.Examples?.Count} examples were found in the dictionary.");
                var firstExample = dictionaryEntry?.Examples?.FirstOrDefault();
                Console.WriteLine($"Example: '{string.Concat(firstExample?.TargetPrefix, firstExample?.TargetTerm, firstExample?.TargetSuffix)}'.");
            }
            catch (RequestFailedException exception)
            {
                Console.WriteLine($"Error Code: {exception.ErrorCode}");
                Console.WriteLine($"Message: {exception.Message}");
            }
        }
    }
}
