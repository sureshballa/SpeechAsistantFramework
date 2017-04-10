using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NaturalLanguageProcessor;
using Newtonsoft.Json;

namespace SpeechAsistant {
	class MainClass {
		public static void Main (string [] args) {
			
			var nlp = new NaturalLanguageProcessor.NaturalLanguageProcessor ();
			var intentConfigurations = JsonConvert.DeserializeObject<List<IntentConfiguration>> (File.ReadAllText ("RPRSpeechIntents.json")).ToList ();
			var contextConfigurations = JsonConvert.DeserializeObject<List<ContextConfiguration>> (File.ReadAllText ("RPRScreenContexts.json")).ToList ();

			nlp.SetConfiguration (intentConfigurations, contextConfigurations);

			while (true) {
				Console.WriteLine ("Which screen are you in?");
				string screen = Console.ReadLine ();

				var suggestions = nlp.GetSuggestions (screen);

				if (suggestions != null) {
					Console.WriteLine ("Some things you can ask me:");
					suggestions.ForEach (s => {
						Console.WriteLine (s);
					});
				} else {
					Console.WriteLine ("Sorry, I do not understand the context.");
					break;
				}
					

 				Console.WriteLine ("Okay, go ahead, I am listening");
				string userSearch = Console.ReadLine ();
				var intentResult = nlp.GetMatchingIntent (userSearch);

				if (intentResult != null) {
					Console.WriteLine ("Awesome, I will get it done.");
					Console.WriteLine ("Action: " + intentResult.Action);
					if (intentResult.Parameters != null) {
						foreach (var paramter in intentResult.Parameters) {
							Console.WriteLine ("Parameter Name: " + paramter.Key);
							Console.WriteLine ("Parameter Values: " + string.Join (", ", paramter.Value));
						}
					} else
						Console.WriteLine ("No specific parameters mentioned.");
				} else
					Console.Write ("Sorry, I do not understand that.");
				Console.ReadLine ();
			}
		}
	}
}
