using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NaturalLanguageProcessor {
	public class Entity {
		
		public Entity () {
		}

		[JsonProperty (PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty (PropertyName = "values")]
		public List<string> Values { get; set; }
	}
}
