using System;
using System.Collections.Generic;

namespace NaturalLanguageProcessor {
	public class IntentResult {
		public IntentResult () {
		}

		public Dictionary<string, List<string>> Parameters { get; set; }

		public string Action { get; set; }
	}
}
