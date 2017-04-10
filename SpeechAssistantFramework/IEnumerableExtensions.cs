using System;
using System.Collections.Generic;
using System.Linq;

namespace NaturalLanguageProcessor {
	public static class IEnumerableExtensions {
		public static void ForEach<T> (this IEnumerable<T> source, Action<T> action) {
			foreach (T element in source)
				action (element);
		}

		/// <summary>
		/// Flattens an enumeration of entities recursively based on an inclusion function
		/// </summary>
		/// <param name="e">E.</param>
		/// <param name="inclusionFunction">Inclusion function.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <remarks>
		/// Creates a list in root/child order.
		/// 
		/// Be careful when calling this on lists that may be very deep, as stack overflows are possible
		/// </remarks>
		public static IEnumerable<T> Flatten<T> (this IEnumerable<T> e, Func<T, IEnumerable<T>> flattenFunction) {
			return e.Concat (e.SelectMany (c => flattenFunction (c).Flatten (flattenFunction)));
		}

		public static IEnumerable<T> FlattenWithFilter<T> (this IEnumerable<T> e, Func<T, IEnumerable<T>> flattenFunction, Func<T, bool> filterFunction) {
			return e.Where (filterFunction).Concat (e.SelectMany (c => flattenFunction (c).Where (filterFunction).Flatten (flattenFunction)));
		}

		public static void ForEachRecursively<T> (this IEnumerable<T> source, Action<T> itemAction, Func<T, IEnumerable<T>> recursionFunction) {
			source.ForEach (i => {
				itemAction (i);
				recursionFunction (i).ForEachRecursively (itemAction, recursionFunction);
			});
		}

		public static TSource MinBy<TSource, TKey> (this IEnumerable<TSource> source,
			Func<TSource, TKey> selector) {
			return source.MinBy (selector, Comparer<TKey>.Default);
		}

		public static TSource MinBy<TSource, TKey> (this IEnumerable<TSource> source,
			Func<TSource, TKey> selector, IComparer<TKey> comparer) {
			// Needs argument null checks

			using (IEnumerator<TSource> sourceIterator = source.GetEnumerator ()) {
				if (!sourceIterator.MoveNext ()) {
					throw new InvalidOperationException ("Sequence was empty");
				}

				TSource min = sourceIterator.Current;
				TKey minKey = selector (min);
				while (sourceIterator.MoveNext ()) {
					TSource candidate = sourceIterator.Current;
					TKey candidateProjected = selector (candidate);

					if (comparer.Compare (candidateProjected, minKey) < 0) {
						min = candidate;
						minKey = candidateProjected;
					}
				}

				return min;
			}
		}


		public static string Join<T> (this IEnumerable<T> source, string separator) {
			return string.Join (separator, source);
		}
	}

}
