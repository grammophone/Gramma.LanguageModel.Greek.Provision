using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gramma.LanguageModel.Provision.EditCommands;

namespace Gramma.LanguageModel.Greek.Provision.EditCommands
{
	/// <summary>
	/// Expecting that a syllable consists of vowels followed by an optional consonants suffix,
	/// this command replaces the original vowels with another vowels string.
	/// </summary>
	[Serializable]
	public class ReplaceVowelsCommand : ReplaceCommand, IEquatable<ReplaceVowelsCommand>, IDeserializationCallback
	{
		#region Private fields

		[NonSerialized]
		private int hashCode;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="targetVowels">The vowels replacement.</param>
		/// <param name="cost">The cost of the edit command.</param>
		public ReplaceVowelsCommand(string targetVowels, float cost = 0.5f)
		{
			if (targetVowels == null) throw new ArgumentNullException("targetVowels");
			
			this.TargetVowels = targetVowels;
			this.Cost = cost;

			ComputeHashCode();
		}

		private void ComputeHashCode()
		{
			this.hashCode = 7 + 23 * (23 * this.TargetVowels.GetHashCode() + this.Cost.GetHashCode());
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The vowels replacement.
		/// </summary>
		public string TargetVowels { get; private set; }

		#endregion

		#region Public methods

		public override string Replace(string baseSyllable)
		{
			if (baseSyllable == null) throw new ArgumentNullException("baseSyllable");

			return this.TargetVowels + baseSyllable.GetConsonantSuffix();
		}

		public override bool Equals(object otherObject)
		{
			var otherCommand = this.EnsureExactType<ReplaceVowelsCommand>(otherObject);

			return Equals(otherCommand);
		}

		public override int GetHashCode()
		{
			return hashCode;
		}

		#endregion

		#region IEquatable<ReplaceVowelsCommand> Members

		public bool Equals(ReplaceVowelsCommand other)
		{
			if (other == null) return false;

			if (this.hashCode != other.hashCode) return false;

			return this.TargetVowels == other.TargetVowels && this.Cost == other.Cost;
		}

		#endregion

		#region IDeserializationCallback Members

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			ComputeHashCode();
		}

		#endregion
	}
}
