using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Grammophone.LanguageModel.Greek.Provision
{
	/// <summary>
	/// Functions for character processing.
	/// </summary>
	public static class CharProcessing
	{
		#region Private fields

		private static CultureInfo greekCultureInfo = new CultureInfo("el");

		#endregion

		#region Vowel functions

		/// <summary>
		/// Returns true if the given character is a greek consonant.
		/// </summary>
		public static bool IsConsonant(char c)
		{
			switch (c)
			{
				case 'Β':
				case 'Γ':
				case 'Δ':
				case 'Ζ':
				case 'Θ':
				case 'Κ':
				case 'Λ':
				case 'Μ':
				case 'Ν':
				case 'Ξ':
				case 'Π':
				case 'Ρ':
				case 'Σ':
				case 'Τ':
				case 'Φ':
				case 'Χ':
				case 'Ψ':
				case 'β':
				case 'γ':
				case 'δ':
				case 'ζ':
				case 'θ':
				case 'κ':
				case 'λ':
				case 'μ':
				case 'ν':
				case 'ξ':
				case 'π':
				case 'ρ':
				case 'ῥ':
				case 'ῤ':
				case 'σ':
				case 'ς':
				case 'τ':
				case 'φ':
				case 'χ':
				case 'ψ':
					return true;

				default:
					return false;
			}
		}

		/// <summary>
		/// Returns true if the given character is a greek consonant.
		/// </summary>
		/// <remarks>
		/// Takes care for both precombined and composed diacritics.
		/// If the character is a diacritic composition character, it is treated as a vowel.
		/// </remarks>
		public static bool IsVowel(char c)
		{
			// First, check the modern greek range characters and rho with daseia and psile.

			switch (c)
			{
				case 'Α':
				case 'Ά':
				case 'Ε':
				case 'Έ':
				case 'Η':
				case 'Ή':
				case 'Ι':
				case 'Ί':
				case 'Ϊ':
				case 'Ο':
				case 'Ό':
				case 'Υ':
				case 'Ύ':
				case 'Ϋ':
				case 'Ω':
				case 'Ώ':
				case 'α':
				case 'ά':
				case 'ε':
				case 'έ':
				case 'η':
				case 'ή':
				case 'ι':
				case 'ί':
				case 'ϊ':
				case 'ΐ':
				case 'ῒ':
				case 'ῗ':
				case 'ο':
				case 'ό':
				case 'υ':
				case 'ύ':
				case 'ϋ':
				case 'ΰ':
				case 'ῢ':
				case 'ῧ':
				case 'ω':
				case 'ώ':
					return true;

				case 'ῤ':
				case 'ῥ':
					return false;
			}

			// With the rho excluded previously, treat all extended greek unicode range 1F00 ~ 1FFF as vowels.
			// This will work for both composed vowels and precombined diacritics, as all of them belong to the same range.

			return c >= '\u1F00' && c < '\u1FFF';
		}

		#endregion

		#region Consonant functions

		/// <summary>
		/// Returns true if a given character is nasal consonant.
		/// </summary>
		public static bool IsNasal(char c)
		{
			switch (c)
			{
				case 'μ':
				case 'ν':
				case 'Μ':
				case 'Ν':
					return true;

				default:
					return false;
			}
		}

		/// <summary>
		/// Returns true if a given character is liquid consonant.
		/// </summary>
		public static bool IsLiquid(char c)
		{
			switch (c)
			{
				case 'λ':
				case 'ρ':
				case 'ῤ':
				case 'ῥ':
				case 'Λ':
				case 'Ρ':
					return true;

				default:
					return false;
			}
		}

		/// <summary>
		/// Returns true if a given character is sharp consonant.
		/// </summary>
		public static bool IsSharp(char c)
		{
			switch (c)
			{
				case 'κ':
				case 'π':
				case 'τ':
				case 'Κ':
				case 'Π':
				case 'Τ':
					return true;

				default:
					return false;
			}
		}

		/// <summary>
		/// Returns true if a given character is soft consonant.
		/// </summary>
		public static bool IsSoft(char c)
		{
			switch (c)
			{
				case 'χ':
				case 'φ':
				case 'θ':
				case 'Χ':
				case 'Φ':
				case 'Θ':
					return true;

				default:
					return false;
			}
		}

		/// <summary>
		/// Returns true if a given character is double consonant.
		/// </summary>
		public static bool IsDouble(char c)
		{
			switch (c)
			{
				case 'ξ':
				case 'ψ':
				case 'ζ':
				case 'Ξ':
				case 'Ψ':
				case 'Ζ':
					return true;

				default:
					return false;
			}
		}

		/// <summary>
		/// Turn a given consonant to its sharp counterpart.
		/// </summary>
		/// <param name="c">The consonant character to convert.</param>
		/// <returns>
		/// If the character is a soft consonant, it returns the converted character, else returns the original character.
		/// </returns>
		public static char SharpenIfSoft(char c)
		{
			switch (c)
			{
				case 'φ':
					return 'π';

				case 'χ':
					return 'κ';

				case 'θ':
					return 'τ';

				case 'Φ':
					return 'π';

				case 'Χ':
					return 'Κ';

				case 'Θ':
					return 'Τ';

				default:
					return c;
			}
		}

		#endregion

		#region String functions

		/// <summary>
		/// Returns the longest prefix of a string whose characters match a predicate.
		/// </summary>
		/// <param name="chars">The string.</param>
		/// <param name="predicate">The condition.</param>
		/// <returns>
		/// Returns the longest prefix of the <paramref name="chars"/> string whose characters match the <paramref name="predicate"/>.
		/// </returns>
		public static string TakeWhile(this string chars, Func<char, bool> predicate)
		{
			for (int i = 0; i < chars.Length; i++)
			{
				char c = chars[i];

				if (!predicate(c)) return chars.Substring(0, i);
			}

			return chars;
		}

		/// <summary>
		/// Strips the longest prefix of a string whose characters match a predicate.
		/// </summary>
		/// <param name="chars">The string.</param>
		/// <param name="predicate">The condition.</param>
		/// <returns>
		/// Returns the suffix after stripping the longest prefix 
		/// of the <paramref name="chars"/> string whose characters match the <paramref name="predicate"/>.
		/// </returns>
		public static string SkipWhile(this string chars, Func<char, bool> predicate)
		{
			for (int i = 0; i < chars.Length; i++)
			{
				char c = chars[i];

				if (!predicate(c)) return chars.Substring(i);
			}

			return String.Empty;
		}

		/// <summary>
		/// Returns the consonants prefix of a string.
		/// </summary>
		public static string GetConsonantPrefix(this string chars)
		{
			return chars.TakeWhile(IsConsonant);
		}

		/// <summary>
		/// Get the consonants suffix of a string.
		/// </summary>
		public static string GetConsonantSuffix(this string chars)
		{
			return chars.SkipWhile(c => !IsConsonant(c));
		}

		/// <summary>
		/// Returns the vowels prefix of a string.
		/// </summary>
		public static string GetVowelPrefix(this string chars)
		{
			return chars.TakeWhile(IsVowel);
		}

		/// <summary>
		/// Get the vowels suffix of a string.
		/// </summary>
		public static string GetVowelsSuffix(this string chars)
		{
			return chars.SkipWhile(c => !IsVowel(c));
		}

		/// <summary>
		/// Strips the consonants prefix of a string.
		/// </summary>
		public static string StripConsonantPrefix(this string chars)
		{
			return chars.SkipWhile(IsConsonant);
		}

		/// <summary>
		/// Strips the consonants suffix of a string.
		/// </summary>
		public static string StripConsonantSuffix(this string chars)
		{
			return chars.TakeWhile(c => !IsConsonant(c));
		}

		/// <summary>
		/// Strips the vowals prefix from a string.
		/// </summary>
		public static string StripVowelPrefix(this string chars)
		{
			return chars.SkipWhile(IsVowel);
		}

		/// <summary>
		/// Strips the vowals suffix from a string.
		/// </summary>
		public static string StripVowelSuffix(this string chars)
		{
			return chars.TakeWhile(c => !IsVowel(c));
		}

		/// <summary>
		/// Get the vowels infix of a string.
		/// </summary>
		public static string GetVowelInfix(this string chars)
		{
			return chars.StripConsonantPrefix().GetVowelPrefix();
		}

		/// <summary>
		/// Normalize a string by setting to all lower-case and 
		/// changing any vareia to okseia.
		/// </summary>
		public static string ChangeVareiaToOkseia(this string chars)
		{
			if (chars.Length == 0) return String.Empty;

			var stringBuilder = new StringBuilder(chars.Length);

			for (int i = 0; i < chars.Length - 1; i++)
			{
				char c = chars[i];

				stringBuilder.Append(c.ChangeVareiaToOkseia());
			}

			char lastc = chars[chars.Length - 1];
			if (lastc == 'σ') lastc = 'ς';

			stringBuilder.Append(lastc.ChangeVareiaToOkseia());

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Normalize a string by setting to all lower-case and stripping
		/// okseia, bareia, perispomene.
		/// </summary>
		public static string RemoveTonos(this string chars)
		{
			if (chars.Length == 0) return String.Empty;

			var stringBuilder = new StringBuilder(chars.Length);

			for (int i = 0; i < chars.Length - 1; i++)
			{
				char c = chars[i];

				stringBuilder.Append(c.RemoveTonos());
			}

			char lastc = chars[chars.Length - 1];
			if (lastc == 'σ') lastc = 'ς';

			stringBuilder.Append(lastc.RemoveTonos());

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Change the character to lower case and 
		/// convert vareia to okseia, if any.
		/// </summary>
		/// <param name="c">The character to convert.</param>
		/// <returns>Returns the lower-case converted character.</returns>
		public static char ChangeVareiaToOkseia(this char c)
		{
			c = Char.ToLower(c, greekCultureInfo);

			switch (c)
			{
				case 'ὰ':
					c = 'ά';
					break;

				case 'ἂ':
					c = 'ἄ';
					break;

				case 'ἃ':
					c = 'ἅ';
					break;

				case 'ὲ':
					c = 'έ';
					break;

				case 'ἒ':
					c = 'ἔ';
					break;

				case 'ἓ':
					c = 'ἕ';
					break;

				case 'ὴ':
					c = 'ή';
					break;

				case 'ἢ':
					c = 'ἤ';
					break;

				case 'ἣ':
					c = 'ἥ';
					break;

				case 'ὶ':
					c = 'ί';
					break;

				case 'ἲ':
					c = 'ἴ';
					break;

				case 'ἳ':
					c = 'ἵ';
					break;

				case 'ὸ':
					c = 'ό';
					break;

				case 'ὂ':
					c = 'ὄ';
					break;

				case 'ὃ':
					c = 'ὅ';
					break;

				case 'ὺ':
					c = 'ύ';
					break;

				case 'ὒ':
					c = 'ὔ';
					break;

				case 'ὓ':
					c = 'ὕ';
					break;

				case 'ὼ':
					c = 'ώ';
					break;

				case 'ὢ':
					c = 'ὤ';
					break;

				case 'ὣ':
					c = 'ὥ';
					break;

			}

			return c;
		}

		/// <summary>
		/// Normalize a character by setting to all lower-case and stripping
		/// okseia, bareia, perispomene.
		/// </summary>
		public static char RemoveTonos(this char c)
		{
			c = Char.ToLower(c, greekCultureInfo);

			switch (c)
			{
				case 'ά':
				case 'ὰ':
				case 'ᾶ':
					c = 'α';
					break;

				case 'ᾴ':
				case 'ᾲ':
				case 'ᾷ':
					c = 'ᾳ';
					break;

				case 'ἄ':
				case 'ἂ':
				case 'ἆ':
					c = 'ἀ';
					break;

				case 'ἅ':
				case 'ἃ':
				case 'ἇ':
					c = 'ἁ';
					break;

				case 'ᾄ':
				case 'ᾂ':
				case 'ᾆ':
					c = 'ᾀ';
					break;

				case 'ᾅ':
				case 'ᾃ':
				case 'ᾇ':
					c = 'ᾁ';
					break;

				case 'έ': // 03AD (in modern greek range)
				case '\u1F73': // ("έ" in ancient greek range)
				case 'ὲ':
					c = 'ε';
					break;

				case 'ἔ':
				case 'ἒ':
					c = 'ἐ';
					break;

				case 'ἕ':
				case 'ἓ':
					c = 'ἑ';
					break;

				case 'ή':
				case 'ὴ':
				case 'ῆ':
					c = 'η';
					break;

				case 'ῄ':
				case 'ῂ':
				case 'ῇ':
					c = 'ῃ';
					break;

				case 'ἤ':
				case 'ἢ':
				case 'ἦ':
					c = 'ἠ';
					break;

				case 'ἥ':
				case 'ἣ':
				case 'ἧ':
					c = 'ἡ';
					break;

				case 'ᾔ':
				case 'ᾒ':
				case 'ᾖ':
					c = 'ᾐ';
					break;

				case 'ᾕ':
				case 'ᾓ':
				case 'ᾗ':
					c = 'ᾑ';
					break;

				case 'ί':
				case 'ὶ':
				case 'ῖ':
					c = 'ι';
					break;

				case 'ΐ':
				case 'ῒ':
				case 'ῗ':
					c = 'ϊ';
					break;

				case 'ἴ':
				case 'ἲ':
				case 'ἶ':
					c = 'ἰ';
					break;

				case 'ἵ':
				case 'ἳ':
				case 'ἷ':
					c = 'ἱ';
					break;

				case 'ό':
				case 'ὸ':
					c = 'ο';
					break;

				case 'ὄ':
				case 'ὂ':
					c = 'ὀ';
					break;

				case 'ὅ':
				case 'ὃ':
					c = 'ὁ';
					break;

				case 'ύ':
				case 'ὺ':
				case 'ῦ':
					c = 'υ';
					break;

				case 'ὔ':
				case 'ὒ':
				case 'ὖ':
					c = 'ὐ';
					break;

				case 'ὕ':
				case 'ὓ':
				case 'ὗ':
					c = 'ὑ';
					break;

				case 'ΰ':
				case 'ῢ':
				case 'ῧ':
					c = 'ϋ';
					break;

				case 'ώ':
				case 'ὼ':
				case 'ῶ':
					c = 'ω';
					break;

				case 'ῲ':
				case 'ῷ':
				case 'ῴ':
					c = 'ῳ';
					break;

				case 'ὤ':
				case 'ὢ':
				case 'ὦ':
					c = 'ὠ';
					break;

				case 'ὥ':
				case 'ὣ':
				case 'ὧ':
					c = 'ὡ';
					break;

				case 'ᾤ':
				case 'ᾢ':
				case 'ᾦ':
					c = 'ᾠ';
					break;

				case 'ᾥ':
				case 'ᾣ':
				case 'ᾧ':
					c = 'ᾡ';
					break;
			}

			return c;
		}

		#endregion
	}
}
