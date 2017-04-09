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
			nlp.LoadConfigurationFile (JsonConvert.DeserializeObject<List<IntentConfiguration>> (File.ReadAllText ("SpeechConfiguration.json")).ToList ());

			while (true) {
				Console.WriteLine ("Enter your Input");
				string userSearch = Console.ReadLine ();
				var intentResult = nlp.GetMatchingIntent (userSearch);


				if (intentResult != null) {
					Console.WriteLine ("Awesome, I will get it done.");
					Console.WriteLine ("Action: " + intentResult.Action);
					foreach (var paramter in intentResult.Parameters) {
						Console.WriteLine ("Parameter Name: " + paramter.Key);
						Console.WriteLine ("Parameter Values: " + string.Join (", ", paramter.Value));
					}
				} else
					Console.Write ("Sorry, I do not understand that.");
				Console.ReadLine ();
			}
		}
	}
}
