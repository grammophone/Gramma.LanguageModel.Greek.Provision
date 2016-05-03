using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grammophone.LanguageModel.Provision;
using Grammophone.LanguageModel.Grammar;
using Grammophone.GenericContentModel;
using System.Globalization;

namespace Grammophone.LanguageModel.Greek.Provision
{
	/// <summary>
	/// Provider for the Ancient Greek language.
	/// </summary>
	public class GreekLanguageProvider : LanguageProvider
	{
		#region Private fields

		private GreekSyllabizer syllabizer;

		private GreekSentenceBreaker sentenceBreaker;

		private CultureInfo greekCultureInfo;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		public GreekLanguageProvider()
		{
			syllabizer = new GreekSyllabizer(this);
			sentenceBreaker = new GreekSentenceBreaker(this);
			greekCultureInfo = new CultureInfo("el");

			this.SyllabizerMode = SyllabizerMode.VowelsToConsonants;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Display name of the Greek language.
		/// </summary>
		public override string LanguageName
		{
			get { return "Ἑλληνικά"; }
		}

		/// <summary>
		/// It seems that "grc" is the locale for Ancient Greek.
		/// </summary>
		public override string LanguageKey
		{
			get { return "grc"; }
		}

		/// <summary>
		/// Get the syllable services for Ancient Greek.
		/// </summary>
		/// <remarks>
		/// Syllabization does not follow the Ancient Greek grammar but seeks a machine-friendly strategy for segmenting words.
		/// </remarks>
		public override Syllabizer Syllabizer
		{
			get { return syllabizer; }
		}

		/// <summary>
		/// Get the services for breaking a sentence into words and punctuation of Ancient Greek.
		/// </summary>
		public override SentenceBreaker SentenceBreaker
		{
			get { return sentenceBreaker; }
		}

		/// <summary>
		/// The syllabization policy.
		/// Default is <see cref="Greek.SyllabizerMode.VowelsToConsonants"/>.
		/// </summary>
		public SyllabizerMode SyllabizerMode { get; set; }

		#endregion

		#region Public methods

		/// <summary>
		/// Converts a word to lower case, and if it has length greater than three, 
		/// it strips okseia, vareia, perispomene.
		/// </summary>
		public override string NormalizeWord(string word)
		{
			if (word == null) throw new ArgumentNullException("word");

			word = word.Trim().ToLower(greekCultureInfo).Normalize(NormalizationForm.FormC);

			if (word.Length == 0) return String.Empty;

			char lastCharacter = word[word.Length - 1];

			// Replace other form of apostrophes with the greek apostrophe.
			switch (lastCharacter)
			{
				case '’': // This is the single quote instead of the apostrophe.
				case '\'': // this is normal ASCII apostrophe.
				case '\x1FBD': // This is "coronis".
					{
						StringBuilder formBuilder = new StringBuilder(word.Length);
							
						formBuilder.Append(word.Substring(0, word.Length - 1));
						formBuilder.Append('᾿'); // Append the correct greek apostrophe \x1FBF.
							
						word = formBuilder.ToString();
					}
					break;
			}

			word = word.Replace('˙', '·'); // Correct the type of upper stop.

			if (word.Length <= 3) return word.ChangeVareiaToOkseia();

			if (word.All(c => c == '.')) return "…";

			return word.RemoveTonos();
		}
		
		#endregion

		#region Protected methods

		protected override GrammarModel CreateGrammarModel()
		{
			var inflectionTypes = new List<InflectionType>();

			// Case inflection type
			var caseInflections = new Inflection[]
			{
				CreateInflection("nom sg", "ὀνομασικὴ ἑνικοῦ"),
				CreateInflection("gen sg", "γενικὴ ἑνικοῦ"),
				CreateInflection("dat sg", "δοτικὴ ἑνικοῦ"),
				CreateInflection("acc sg", "αἰτιατικὴ ἑνικοῦ"),
				CreateInflection("voc sg", "κλητικὴ ἑνικοῦ"),
				CreateInflection("nom pl", "ὀνομασικὴ πληθυντικοῦ"),
				CreateInflection("gen pl", "γενικὴ πληθυντικοῦ"),
				CreateInflection("dat pl", "δοτικὴ πληθυντικοῦ"),
				CreateInflection("acc pl", "αἰτιατικὴ πληθυντικοῦ"),
				CreateInflection("voc pl", "κλητικὴ πληθυντικοῦ"),
				CreateInflection("nom dual", "ὀνομασικὴ δυϊκοῦ"),
				CreateInflection("gen dual", "γενικὴ δυϊκοῦ"),
				CreateInflection("dat dual", "δοτικὴ δυϊκοῦ"),
				CreateInflection("acc dual", "αἰτιατικὴ δυϊκοῦ"),
				CreateInflection("voc dual", "κλητικὴ δυϊκοῦ")
			};

			var caseInflectionType = CreateInflectionType("case", "πτῶσις", caseInflections);

			inflectionTypes.Add(caseInflectionType);

			// Gender inflection type.
			var genderInflections = new Inflection[]
			{
				CreateInflection("masc", "ἀρσενικόν"),
				CreateInflection("fem", "θηλυκόν"),
				CreateInflection("neut", "οὐδέτερον")
			};

			var genderInflectionType = CreateInflectionType("gender", "γένος", genderInflections);

			inflectionTypes.Add(genderInflectionType);

			// Person inflection type.
			var personInflections = new Inflection[]
			{
				CreateInflection("1st sg", "πρῶτον ἑνικοῦ"),
				CreateInflection("2nd sg", "δεύτερον ἑνικοῦ"),
				CreateInflection("3rd sg", "τρίτον ἑνικοῦ"),
				CreateInflection("1st pl", "πρῶτον πληθυντικοῦ"),
				CreateInflection("2nd pl", "δεύτερον πληθυντικοῦ"),
				CreateInflection("3rd pl", "τρίτον πληθυντικοῦ"),
				CreateInflection("2nd dual", "δεύτερον δυϊκοῦ"),
				CreateInflection("3rd dual", "τρίτον δυϊκοῦ")
			};

			var personInflectionType = CreateInflectionType("person", "πρόσωπον", personInflections);

			inflectionTypes.Add(personInflectionType);

			// Tense inflection type.
			var tenseInflections = new Inflection[]
			{
				CreateInflection("pres", "ἐνεστώς"),
				CreateInflection("imperf", "παρατατικός"),
				CreateInflection("fut", "μέλλων"),
				CreateInflection("aor", "ἀόριστος"),
				CreateInflection("perf", "παρακείμενος"),
				CreateInflection("plup", "ὑπερσυντέλικος"),
				CreateInflection("futperf", "συντετελεσμένος μέλλων")
			};

			var tenseInflectionType = CreateInflectionType("tense", "χρόνος", tenseInflections);

			inflectionTypes.Add(tenseInflectionType);

			//// Participle tense inflection type.
			//var participleTenseInflections = new Inflection[]
			//{
			//  CreateInflection("pres", "ἐνεστώς"),
			//  CreateInflection("fut", "μέλλων"),
			//  CreateInflection("aor", "ἀόριστος"),
			//  CreateInflection("perf", "παρακείμενος")
			//};

			//var participleTenseInflectionType = CreateInflectionType("participle tense", "χρόνος", tenseInflections);

			//inflectionTypes.Add(participleTenseInflectionType);

			// Mood inflection types.
			var moodInflections = new Inflection[]
			{
				CreateInflection("ind", "ὁριστική"),
				CreateInflection("subj", "ὑποτακτική"),
				CreateInflection("opt", "εὐκτική"),
				CreateInflection("imperat", "προστακτική"),
				CreateInflection("inf", "ἀπαρέμφατον")
			};

			var moodInflectionType = CreateInflectionType("mood", "ἔγκλισις", moodInflections);

			inflectionTypes.Add(moodInflectionType);

			// Voice inflection types.
			var voiceInflections = new Inflection[]
			{
				CreateInflection("act", "ἐνεργητική"),
				CreateInflection("mid", "μέση"),
				CreateInflection("mp", "μεσοπαθητική"),
				CreateInflection("pass", "παθητική")
			};

			var voiceInflectionType = CreateInflectionType("voice", "φωνή", voiceInflections);

			inflectionTypes.Add(voiceInflectionType);

			// Degree inflection types.
			var degreeInflections = new Inflection[] 
			{
				CreateInflection("pos", "θετικός"),
				CreateInflection("comp", "συγκριτικός"),
				CreateInflection("superl", "ὑπερθετικός")
			};

			var degreeInflectionType = CreateInflectionType("degree", "βαθμός", degreeInflections);

			inflectionTypes.Add(degreeInflectionType);

			var tagTypes = new TagType[]
			{
				// START and END tag types
				CreateTagType("[SENTENCE BOUNDS]", "[ὅριον προτάσεως]", true),

				// Punctuation
				CreateTagType("[PUNCTUATION]", "[στίξις]", true),

				// Grammatical part-of-speech types.
				CreateTagType("article", "ἄρθρον", false),
				CreateTagType("noun", "οὐσιαστικόν", false),
				CreateTagType("adj", "ἐπίθετον", false),
				CreateTagType("pron", "ἀντωνυμία", false),
				CreateTagType("verb", "ῥῆμα", false),
				CreateTagType("part", "μετοχή", false),
				CreateTagType("adv", "ἐπίῤῥημα", false),
				CreateTagType("prep", "πρόθεσις", true),
				CreateTagType("conj", "σύνδεσμος", true),
				CreateTagType("exclam", "ἐπιφώνημα", true),
				CreateTagType("partic", "μόριον", true),
				CreateTagType("numeral", "ἀριθμητικόν", false)
			};

			// Many-to-many relationship between tag types and inflection types.

			Tuple<string, string>[] tagTypeToInflectionTypeRelationship = 
			{
				new Tuple<string, string>("article", "case"),
				new Tuple<string, string>("article", "gender"),
				
				new Tuple<string, string>("noun", "case"),
				new Tuple<string, string>("noun", "gender"),
				
				new Tuple<string, string>("adj", "case"),
				new Tuple<string, string>("adj", "gender"),
				new Tuple<string, string>("adj", "degree"),
				
				new Tuple<string, string>("pron", "case"),
				new Tuple<string, string>("pron", "gender"),
				
				new Tuple<string, string>("verb", "person"),
				new Tuple<string, string>("verb", "tense"),
				new Tuple<string, string>("verb", "mood"),
				new Tuple<string, string>("verb", "voice"),

				new Tuple<string, string>("part", "case"),
				new Tuple<string, string>("part", "gender"),
				//new Tuple<string, string>("part", "participle tense"),
				new Tuple<string, string>("part", "tense"),
				new Tuple<string, string>("part", "voice"),

				new Tuple<string, string>("adv", "degree")
			};

			return new GrammarModel(tagTypes, inflectionTypes, tagTypeToInflectionTypeRelationship);
		}

		protected override Tag GetStartTag()
		{
			var grammarModel = this.GrammarModel;

			var tagType = grammarModel.TagTypes["[SENTENCE BOUNDS]"];

			return grammarModel.GetTag(tagType, null, "[ΑΡΧΗ]");
		}

		protected override Tag GetEndTag()
		{
			var grammarModel = this.GrammarModel;

			var tagType = grammarModel.TagTypes["[SENTENCE BOUNDS]"];

			return grammarModel.GetTag(tagType, null, "[ΤΕΛΟΣ]");
		}

		#endregion

		#region Private methods

		private Tag[] GetTagInflectionCombinations(TagType tagType)
		{
			if (tagType == null) throw new ArgumentNullException("tagType");

			var grammarModel = this.GrammarModel;

			var inflectionsCombinations = GetInflectionCombinations(tagType.InflectionTypes);

			var tags = new Tag[inflectionsCombinations.Length];

			for (int i = 0; i < inflectionsCombinations.Length; i++)
			{
				tags[i] = grammarModel.GetTag(tagType, inflectionsCombinations[i]);
			}

			return tags;
		}

		#endregion
	}
}
