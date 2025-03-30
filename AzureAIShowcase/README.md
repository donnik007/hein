# Azure AI Services Showcase

This application demonstrates various Azure AI services for use in presentations and demonstrations. The application includes sample code for:

- Text Translation
- OCR (Optical Character Recognition)
- Face Detection and Analysis
- Text Analytics (Sentiment Analysis, Key Phrase Extraction, Language Detection, Entity Recognition)
- Speech (Speech-to-Text, Text-to-Speech)

## Setup

1. Create Azure AI services resources in the Azure portal:
   - Translator
   - Computer Vision
   - Face API
   - Text Analytics
   - Speech Service

2. Get your API keys and endpoints for each service

3. Update the demo files with your API keys and endpoints:
   - TranslationDemo.cs
   - OcrDemo.cs
   - FaceDetectionDemo.cs
   - TextAnalyticsDemo.cs
   - SpeechDemo.cs

## Running the Application

1. Build the application:
   ```
   dotnet build
   ```

2. Run the application:
   ```
   dotnet run
   ```

3. Follow the on-screen menu to select which AI service you want to demonstrate.

## Notes for Presentation

- Each demo allows you to input your own test data (text or image URLs)
- For image-based services, you can prepare a set of test images to showcase different scenarios
- The code is organized to make it easy to explain how each service works
- Consider preparing some slides to explain:
  - What each AI service does
  - Use cases and industry applications
  - How to set up these services in Azure
  - Pricing models for each service
