# CollectAnswersTool

Purpose of this tool to read questions from a CSV file and 
collect answers from LLM and write answers back to the CSV file. 
This file can be used then to evaluate the answers.

## Configuration

### CollectAnswersTool/App.config

**CSVFilePath:** Path to the CSV file containing questions and ground truth. 
Model answer will be written to column 3, so currently context not handled.

**ChatbotUrl:** URL of the chatbot to get answers from. 
Current version of this tool prepared to the following request and response format:

Request: {
  "question": "string",
  "conversationId": "string" //Send null to start a new conversation
}

Response: {
  "role": "string",
  "contentPath": "string",
  "result": "string", //Ai Response
  "messageDate": "2024-06-26T07:11:20.688Z",
  "dailyRequestCount": 0,
  "dailyRemainingRequestCount": 0
}

### ChatbotApi/appsettings.json

Only needed to configure if you want to emulate a chatbot.

**ModelDeploymentName:** "Azure OpenAI model deployment name"

**Endpoint:** "Azure OpenAI endpoint"

**ApiKey:** "Azure OpenAI API key"
