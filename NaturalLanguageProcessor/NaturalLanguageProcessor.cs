using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace NaturalLanguageProcessor {
	public class NaturalLanguageProcessor {
		public NaturalLanguageProcessor () {
		}

		public List<IntentConfiguration> RulesConfigurations { get; private set; }

		public void LoadConfigurationFile (List<IntentConfiguration> rulesConfigurations) {
			this.RulesConfigurations = rulesConfigurations;
		}

		public IntentResult GetMatchingIntent (string speechText) {

			List<string> words = speechText.Split (' ').ToList ();

			//For each config
			var matchingIntents = from i in this.RulesConfigurations
								  where ProcessForMatch (i, words)
								  select i;

			if (matchingIntents.Any ()) {
				var matchingEntity = matchingIntents.FirstOrDefault ();
				var parameters = ProcessForEntities (matchingEntity.Entities, speechText);

				return new IntentResult () {
					Action = matchingEntity.Action,
					Parameters = parameters
				};
			} else
				return null;
		}

		private bool ProcessForMatch (IntentConfiguration intentConfiguration, List<string> words) {
			bool result = false;

			if (ProcessForKeywords (intentConfiguration.Keywords, words))
				if (ProcessForKeywords (intentConfiguration.Verbs, words))
					result = true;


			return result;
		}

		private bool ProcessForKeywords (List<string> keywords, List<string> words) {
			bool result = false;

			var matchingWords = from w in words
								where keywords.Contains (w)
								select w;

			if (matchingWords.Any ())
				result = true;

			return result;
		}

		private Dictionary<string, List<string>> ProcessForEntities (List<Entity> entities, string speechText) {
			Dictionary<string, List<string>> parameters = new Dictionary<string, List<string>> ();
			var words = speechText.Split (' ').ToList ();
			entities.ForEach (entity => {
				if (entity.RegexParametersMetaData == null) {
					var matches = from w in words
								  where entity.Values.Contains (w)
								  select w;

					if (matches.Any ()) {
						parameters.Add (entity.Name, matches.ToList ());
					}
				} else {
					entity.Values.ForEach (val => {
						var match = Regex.Match (speechText, val);
						if (match.Success) {
							var matchedSubstring = match.Value;

							entity.RegexParametersMetaData.ForEach (parameterMetaData => {
								if (parameterMetaData.Type == "number") {
									var values = Regex.Split (matchedSubstring, @"\D+")
									                  .Where (s => !string.IsNullOrWhiteSpace (s)).ToList();

									if (!parameters.ContainsKey (entity.Name))
										parameters.Add (parameterMetaData.Name, new List<string> () { values[entity.RegexParametersMetaData.IndexOf(parameterMetaData)] });
								}
							});
						}
					});
				}
			});

			if (parameters.Count () > 0)
				return parameters;
			else
				return null;
		}
	}
}
