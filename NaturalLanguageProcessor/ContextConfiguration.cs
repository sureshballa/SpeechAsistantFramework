using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NaturalLanguageProcessor {
	public class ContextConfiguration {
		public ContextConfiguration () {
		}

		[JsonProperty (PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty (PropertyName = "intents")]
		public List<string> Intents { get; set; }
	}
}
