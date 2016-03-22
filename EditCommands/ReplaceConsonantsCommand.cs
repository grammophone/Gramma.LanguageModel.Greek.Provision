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
	/// this command replaces the original consonants with another consonants string.
	/// </summary>
	[Serializable]
	public class ReplaceConsonantsCommand : 
		ReplaceCommand, IEquatable<ReplaceConsonantsCommand>, IDeserializationCallback
	{
		#region Private fields

		[NonSerialized]
		private int hashCode;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="targetConsonants">The consonants replacement.</param>
		/// <param name="cost">The cost of the edit command.</param>
		public ReplaceConsonantsCommand(string targetConsonants, float cost = 0.5f)
		{
			if (targetConsonants == null) throw new ArgumentNullException("targetConsonants");

			this.TargetConsonants = targetConsonants;
			this.Cost = cost;

			ComputeHashCode();
		}

		private void ComputeHashCode()
		{
			this.hashCode = 13 + 23 * (23 * this.TargetConsonants.GetHashCode() + this.Cost.GetHashCode());
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The consonants replacement.
		/// </summary>
		public string TargetConsonants { get; private set; }

		#endregion

		#region Public methods

		public override string Replace(string baseSyllable)
		{
			if (baseSyllable == null) throw new ArgumentNullException("baseSyllable");

			return baseSyllable.StripConsonantSuffix() + this.TargetConsonants;
		}

		public override bool Equals(object otherObject)
		{
			return Equals(otherObject as ReplaceConsonantsCommand);
		}

		public override int GetHashCode()
		{
			return hashCode;
		}

		#endregion

		#region IEquatable<ReplaceConsonantsCommand> Members

		public bool Equals(ReplaceConsonantsCommand other)
		{
			if (other == null) return false;

			if (other.hashCode != this.hashCode) return false;

			return this.TargetConsonants == other.TargetConsonants && this.Cost == other.Cost;
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
