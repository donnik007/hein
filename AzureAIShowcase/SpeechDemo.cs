using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace AzureAIShowcase
{
    public static class SpeechDemo
    {
        // Replace with your own key and region
        private static readonly string SpeechKey = Environment.GetEnvironmentVariable("SPEECHAPI");
        private static readonly string SpeechRegion = "northeurope"; // e.g., westeurope

        public static async Task RunAsync()
        {
            Console.WriteLine("\n=== Speech Service Demo ===");
            
            if (SpeechKey == "YOUR_SPEECH_KEY")
            {
                Console.WriteLine("Please set your Speech service key and region in the SpeechDemo.cs file.");
                return;
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Speech-to-Text (Speech Recognition)");
                Console.WriteLine("2. Text-to-Speech (Speech Synthesis)");
                Console.WriteLine("0. Return to Main Menu");
                
                Console.Write("\nYour choice: ");
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        await SpeechToTextAsync();
                        break;
                    case "2":
                        await TextToSpeechAsync();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static async Task SpeechToTextAsync()
        {
            Console.WriteLine("\n--- Speech-to-Text Demo ---");
            Console.WriteLine("Speak into your microphone when prompted...");
            
            try
            {
                // Configure speech recognition
                var speechConfig = SpeechConfig.FromSubscription(SpeechKey, SpeechRegion);
                
                // Default language is en-US, but you can specify a different language
                Console.Write("Enter language code (or press Enter for en-US): ");
                string languageCode = Console.ReadLine();
                if (!string.IsNullOrEmpty(languageCode))
                {
                    speechConfig.SpeechRecognitionLanguage = languageCode;
                }
                else
                {
                    speechConfig.SpeechRecognitionLanguage = "en-US";
                }
                
                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
                using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
                
                Console.WriteLine("\nSpeak now...");
                
                var result = await recognizer.RecognizeOnceAsync();
                
                // Print results
                switch (result.Reason)
                {
                    case ResultReason.RecognizedSpeech:
                        Console.WriteLine($"RECOGNIZED: {result.Text}");
                        break;
                    case ResultReason.NoMatch:
                        Console.WriteLine("NOMATCH: Speech could not be recognized.");
                        break;
                    case ResultReason.Canceled:
                        var cancellation = CancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");
                        
                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task TextToSpeechAsync()
        {
            Console.WriteLine("\n--- Text-to-Speech Demo ---");
            
            try
            {
                // Configure speech synthesis
                var speechConfig = SpeechConfig.FromSubscription(SpeechKey, SpeechRegion);
                
                // Get voice options
                Console.Write("Enter text to convert to speech: ");
                string textToSpeak = Console.ReadLine() ?? "Hello! This is a demonstration of text to speech using Azure AI services.";
                
                Console.WriteLine("\nChoose a voice:");
                Console.WriteLine("1. en-US-JennyNeural (English, US, Female)");
                Console.WriteLine("2. en-US-GuyNeural (English, US, Male)");
                Console.WriteLine("3. en-GB-SoniaNeural (English, UK, Female)");
                Console.WriteLine("4. fr-FR-DeniseNeural (French, Female)");
                Console.WriteLine("5. de-DE-KatjaNeural (German, Female)");
                Console.WriteLine("6. es-ES-ElviraNeural (Spanish, Female)");
                
                Console.Write("\nYour choice (or press Enter for default): ");
                string voiceChoice = Console.ReadLine();
                
                switch (voiceChoice)
                {
                    case "1":
                        speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";
                        break;
                    case "2":
                        speechConfig.SpeechSynthesisVoiceName = "en-US-GuyNeural";
                        break;
                    case "3":
                        speechConfig.SpeechSynthesisVoiceName = "en-GB-SoniaNeural";
                        break;
                    case "4":
                        speechConfig.SpeechSynthesisVoiceName = "fr-FR-DeniseNeural";
                        break;
                    case "5":
                        speechConfig.SpeechSynthesisVoiceName = "de-DE-KatjaNeural";
                        break;
                    case "6":
                        speechConfig.SpeechSynthesisVoiceName = "es-ES-ElviraNeural";
                        break;
                    default:
                        speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";
                        break;
                }
                
                // Ensure output directory exists
                string outputDirectory = "/workspaces/hein/output";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save audio to file
                string outputFilePath = Path.Combine(outputDirectory, "speech_output.wav");
                using var audioConfig = AudioConfig.FromWavFileOutput(outputFilePath);
                using var synthesizer = new SpeechSynthesizer(speechConfig, audioConfig);
                
                Console.WriteLine("\nConverting text to speech...");
                
                var result = await synthesizer.SpeakTextAsync(textToSpeak);
                
                // Check result
                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                {
                    Console.WriteLine($"Speech synthesis completed successfully. Audio saved to: {outputFilePath}");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");
                    
                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
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
