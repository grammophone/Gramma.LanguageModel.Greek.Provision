using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gramma.LanguageModel.Provision;

namespace Gramma.LanguageModel.Greek.Provision
{
	/// <summary>
	/// Breaks Ancient Greek sentences into words and punctuation.
	/// </summary>
	public class GreekSentenceBreaker : SentenceBreaker
	{
		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		public GreekSentenceBreaker(LanguageProvider languageProvider)
			: base(languageProvider)
		{

		}

		#endregion

		#region Protected methods

		protected override string GetSentenceDelimiters()
		{
			return "\r\n.;:·…!";
		}

		protected override string GetWordDelimiters()
		{
			return " \r\n.;:·…!-()";
		}

		protected override string GetPunctuationCharacters()
		{
			return ".,;:·…!-()";
		}

		#endregion
	}
}
