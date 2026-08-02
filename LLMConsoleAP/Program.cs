// See https://aka.ms/new-console-template for more information

using LLMLib;
using RAGLib.Models;
using SearchEngineManager;
using System.Dynamic;

//skill加上一條：如果專案為Console App，則在第一行加上：Console.OutputEncoding = System.Text.Encoding.UTF8;

LLMExtensionHelper lLMExtensionHelper = new LLMExtensionHelper();
await lLMExtensionHelper.AIChatTest6();