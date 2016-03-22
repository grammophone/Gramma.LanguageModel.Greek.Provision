using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gramma.LanguageModel.Provision;
using Gramma.LanguageModel.Provision.EditCommands;
using Gramma.LanguageModel.Greek.Provision.EditCommands;

namespace Gramma.LanguageModel.Greek.Provision
{
	/// <summary>
	/// The syllable services for Ancient Greek.
	/// </summary>
	/// <remarks>
	/// Syllabization does not follow the Ancient Greek grammar but seeks a machine-friendly strategy for segmenting words.
	/// The order of syllables is reversed, then a sentinel character '$' is appended.
	/// </remarks>
	public class GreekSyllabizer : Syllabizer
	{
		#region Private fields

		private GreekLanguageProvider languageProvider;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		public GreekSyllabizer(GreekLanguageProvider languageProvider)
			: base(languageProvider)
		{
			this.languageProvider = languageProvider;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Normalize and break a word into its syllables.
		/// </summary>
		/// <param name="word">The word to segment.</param>
		/// <returns>
		/// Returns the syllabic form of the word.
		/// </returns>
		/// <remarks>
		/// Syllabization does not follow the Ancient Greek grammar but seeks a machine-friendly strategy for segmenting words.
		/// The order of syllables is reversed, then a sentinel character '$' is appended.
		/// The characters are normalized: Capitalization, okseia, bareia and perispomene are removed.
		/// </remarks>
		public override SyllabicWord Segment(string word)
		{
			// If the characters are punctuation, return a single syllable containing the punctuation plus the sentinel.
			if (word.All(c => this.LanguageProvider.SentenceBreaker.IsPunctuation(c)))
				return new SyllabicWord(new string[] { word, "$" });

			// Decompose all precombined diacritics, if any, then recombine them according to the unicode standard.
			// Remove okseia, bareia and perispomene, if length is above 2.
			word = this.LanguageProvider.NormalizeWord(word);

			var syllables = new List<string>(word.Length);

			var syllableBuilder = new StringBuilder(8);

			int i;

			switch (languageProvider.SyllabizerMode)
			{
				case SyllabizerMode.VowelsToConsonants:
					bool isPreviousConsonant;

					for (isPreviousConsonant = false, i = 0; i < word.Length; i++)
					{
						char c = word[i];

						bool isConsonant = CharProcessing.IsConsonant(c);

						if (isConsonant)
						{
							isPreviousConsonant = true;
						}
						else
						{
							if (isPreviousConsonant)
							{
								syllables.Add(syllableBuilder.ToString());
								syllableBuilder.Clear();

								isPreviousConsonant = false;
							}
						}

						syllableBuilder.Append(c);
					}

					if (syllableBuilder.Length > 0) syllables.Add(syllableBuilder.ToString());

					break;

				case SyllabizerMode.ConsonantsToVowels:
					bool isPreviousVowel;

					for (isPreviousVowel = false, i = 0; i < word.Length; i++)
					{
						char c = word[i];

						bool isVowel = CharProcessing.IsVowel(c);

						if (isVowel)
						{
							isPreviousVowel = true;
						}
						else
						{
							if (isPreviousVowel)
							{
								syllables.Add(syllableBuilder.ToString());
								syllableBuilder.Clear();

								isPreviousVowel = false;
							}
						}

						syllableBuilder.Append(c);
					}

					if (syllableBuilder.Length > 0) syllables.Add(syllableBuilder.ToString());

					break;

				default:
					throw new ProvisionException(String.Format("Unsupported syllabizer mode '{0}'", languageProvider.SyllabizerMode));
			}

			syllables.Reverse();

			syllables.Add("$");

			return new SyllabicWord(syllables);
		}

		/// <summary>
		/// Reassembles syllables into a word.
		/// </summary>
		/// <param name="word">The syllabic word to reassemble.</param>
		/// <returns>Returns the reassembled word.</returns>
		/// <remarks>
		/// Removes the sentinel character. 
		/// The characters are normalized: Capitalization, okseia, bareia and perispomene are removed.
		/// </remarks>
		public override string Reassemble(SyllabicWord word)
		{
			var syllables = word.ToArray();

			var stringBuilder = new StringBuilder(word.Count * 5);

			// Reverse order. 
			for (int i = syllables.Length - 1; i >= 0; i--)
			{
				stringBuilder.Append(syllables[i]);
			}

			stringBuilder.Replace("$", String.Empty);

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Get the generalized edit distance between two syllables.
		/// </summary>
		/// <param name="baseSyllable">The base syllable.</param>
		/// <param name="targetSyllable">The compared syllable.</param>
		/// <returns>
		/// Returns a command which, when executed upon the <paramref name="baseSyllable"/>,
		/// yields the <paramref name="targetSyllable"/> as a result.
		/// </returns>
		/// <remarks>
		/// This implementation makes these tests in order:
		/// If the syllables are the same, it returns a <see cref="NoChangeCommand"/> with cost 0.0.
		/// If they share a common consonant suffix, including the empty string, it returns a <see cref="ReplaceVowelsCommand"/> with cost 0.5.
		/// If they share a common vowel prefix, it returns a <see cref="ReplaceConsonantsCommand"/> with cost 0.5.
		/// Else it returns a <see cref="ReplaceAllCommand"/> with cost 1.0.
		/// </remarks>
		public override ReplaceCommand GetDistance(string baseSyllable, string targetSyllable)
		{
			if (baseSyllable == null) throw new ArgumentNullException("baseSyllable");
			if (targetSyllable == null) throw new ArgumentNullException("targetSyllable");

			if (baseSyllable == targetSyllable) return EditCommand.NoChange;

			string baseConsonantSuffix = baseSyllable.GetConsonantSuffix();
			string targetConsonantSuffix = targetSyllable.GetConsonantSuffix();
			string targetVowels = targetSyllable.StripConsonantSuffix();

			if (baseConsonantSuffix == targetConsonantSuffix)
			{
				return new ReplaceVowelsCommand(targetVowels);
			}

			string baseVowels = baseSyllable.StripConsonantSuffix();

			if (baseVowels == targetVowels)
			{
				return new ReplaceConsonantsCommand(targetConsonantSuffix);
			}

			return new ReplaceAllCommand(targetSyllable);
		}

		#endregion
	}
}
