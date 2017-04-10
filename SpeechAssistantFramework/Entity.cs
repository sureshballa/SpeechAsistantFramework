using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NaturalLanguageProcessor {
	public class Entity {
		
		public Entity () {
		}

		[JsonProperty (PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty (PropertyName = "regexParametersMetaData")]
		public List<RegexParameterMetaData> RegexParametersMetaData { get; set; }

		[JsonProperty (PropertyName = "values")]
		public List<string> Values { get; set; }
	}

	public class RegexParameterMetaData {
		public RegexParameterMetaData () {
		}

		[JsonProperty (PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty (PropertyName = "type")]
		public string Type { get; set; }
	}
}
