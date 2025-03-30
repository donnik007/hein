using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AzureAIShowcase
{
    public static class FaceDetectionDemo
    {
        // Replace with your own key and endpoint
        private static readonly string FaceKey = Environment.GetEnvironmentVariable("FACE_API");
        private static readonly string FaceEndpoint = "https://facehein.cognitiveservices.azure.com/";

        public static async Task RunAsync()
        {
            Console.WriteLine("\n=== Face Detection and Analysis Demo ===");
            
            if (FaceKey == "YOUR_FACE_API_KEY")
            {
                Console.WriteLine("Please set your Face API key and endpoint in the FaceDetectionDemo.cs file.");
                return;
            }

            Console.Write("Enter image URL containing faces: ");
            string imageUrl = Console.ReadLine() ?? "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/detection1.jpg";
            
            try
            {
                IFaceClient faceClient = new FaceClient(
                    new ApiKeyServiceClientCredentials(FaceKey))
                {
                    Endpoint = FaceEndpoint
                };
                
                Console.WriteLine("\nDetecting faces...");
                
                // Define features to detect
                List<FaceAttributeType> faceAttributes = new List<FaceAttributeType>
                {
                    FaceAttributeType.Age,
                    FaceAttributeType.Gender,
                    FaceAttributeType.Emotion,
                    FaceAttributeType.Smile
                };
                
                // Detect faces with attributes
                IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(
                    imageUrl, 
                    returnFaceAttributes: faceAttributes);
                
                Console.WriteLine($"\nDetected {detectedFaces.Count} face(s)");
                
                // Display results for each face
                foreach (var face in detectedFaces)
                {
                    Console.WriteLine($"\nFace location: ({face.FaceRectangle.Left}, {face.FaceRectangle.Top}, {face.FaceRectangle.Width}, {face.FaceRectangle.Height})");
                    Console.WriteLine($"Age: {face.FaceAttributes.Age}");
                    Console.WriteLine($"Gender: {face.FaceAttributes.Gender}");
                    Console.WriteLine($"Smile: {face.FaceAttributes.Smile}");
                    
                    // Find the dominant emotion
                    string dominantEmotion = "";
                    double dominantScore = 0.0;
                    
                    if (face.FaceAttributes.Emotion.Anger > dominantScore) { dominantEmotion = "Anger"; dominantScore = face.FaceAttributes.Emotion.Anger; }
                    if (face.FaceAttributes.Emotion.Contempt > dominantScore) { dominantEmotion = "Contempt"; dominantScore = face.FaceAttributes.Emotion.Contempt; }
                    if (face.FaceAttributes.Emotion.Disgust > dominantScore) { dominantEmotion = "Disgust"; dominantScore = face.FaceAttributes.Emotion.Disgust; }
                    if (face.FaceAttributes.Emotion.Fear > dominantScore) { dominantEmotion = "Fear"; dominantScore = face.FaceAttributes.Emotion.Fear; }
                    if (face.FaceAttributes.Emotion.Happiness > dominantScore) { dominantEmotion = "Happiness"; dominantScore = face.FaceAttributes.Emotion.Happiness; }
                    if (face.FaceAttributes.Emotion.Neutral > dominantScore) { dominantEmotion = "Neutral"; dominantScore = face.FaceAttributes.Emotion.Neutral; }
                    if (face.FaceAttributes.Emotion.Sadness > dominantScore) { dominantEmotion = "Sadness"; dominantScore = face.FaceAttributes.Emotion.Sadness; }
                    if (face.FaceAttributes.Emotion.Surprise > dominantScore) { dominantEmotion = "Surprise"; dominantScore = face.FaceAttributes.Emotion.Surprise; }
                    
                    Console.WriteLine($"Emotion: {dominantEmotion} ({dominantScore:P1})");
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
