using System;
using System.Collections.Generic;
using System.IO;
/*
 * Project: MyTest App
 * Module: TestState
 * Author: Andrey N. Glushenkov
 * Date: 03.10.2017
 * Description:
 * TestState module loads chosen test. It acts as a Question container and counter of correct answers.
 * After the test has ended it shows the resulting statistics.
 */

namespace MyTestLib
{
	using static TestFileParser;
	using Protector;
	
	/// <summary>
	/// MyTest test state object. Stores all the test data.
	/// </summary>
	public class TestState
	{
		// Test modes
		/// <summary>
		/// Test mode
		/// </summary>
		public enum ETestMode
		{
			Loyal,	// Default value
			Punish
		};

		// Mode strings
		/// <summary>
		///  Loyal test mode. No penalties for the wrong answers
		/// </summary>
		static string ModeLoyal = "loyal";
		/// <summary>
		/// Punish test mode. Wrong answers will lower the score
		/// </summary>
		static string ModePunish = "punish";

		/// <summary>
		/// Test name
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Test author
		/// </summary>
		public string Author { get; private set; }

		/// <summary>
		/// Date on last modification of the test
		/// </summary>
		public DateTime LastModified { get; private set; }

		/// <summary>
		/// Test mode 
		/// </summary>
		public ETestMode Mode { get; private set; } = ETestMode.Loyal;

		/// <summary>
		/// Test time. Infinite by default 
		/// </summary>
		public int Time { get; private set; } = TimeInfinite;

		/// <summary>
		/// Test Questions. Each time you get them they are shuffled
		/// </summary>
		public IList<TestQuestion> Questions
		{
			get
			{
				foreach (var question in questions)
				{
					question.Shuffle (rng);
				}
				return questions;
			}
		}

		/// <summary>
		/// Maximum amount of points for test completion
		/// </summary>
		public float Value
		{
			get
			{
				float value = 0;
				foreach (var question in questions)
				{
					value += question.Value;
				}
				return value;
			}
		}

		/// <summary>
		/// Test results
		/// </summary>
		public TestResults Results = null;

		public TestState()
		{
			Results = new TestResults(this);
		}

		// Load test
		public void LoadTest(string testFilePath)
		{
			StringReader testFile = null;
			var testDecryptor = new TestFileEncDec (testFilePath, TestFileEncDec.ETestFileMode.Decode);
			{
				var data = testDecryptor.Data;
				testFile = new StringReader (System.Text.Encoding.UTF8.GetString (data));
			}
			// Read whole text into memory
			//var file = File.OpenText (testFilePath);
			//var testFile = new StringReader (file.ReadToEnd ());
			//file.Close ();
			// Current question being parsed
			TestQuestion question = null;
			string line = null;//testFile.ReadLine();
			do
			{
				line = testFile.ReadLine ();
				// Skip commentary lines
				if (line == null || line.Length == 0 || line [0] == '#')
				{
					continue;
				}
				// Parse the rest of the file
				var keyValue = GetKeyValueFromLine (line);
				switch (keyValue.Key)
				{
					case ETestTags.Test:
						{
							Name = keyValue.Value;
						}
						break;
					case ETestTags.Author:
						{
							Author = keyValue.Value;
						}
						break;
					case ETestTags.Date:
						{
							var dateStr = keyValue.Value.Split ('.');
							LastModified = new DateTime (Convert.ToInt32 (dateStr [2]), Convert.ToInt32 (dateStr [1]), Convert.ToInt32 (dateStr [0]));
						}
						break;
					case ETestTags.Time:
						{
							// If time is infinite, then time = 0
							if (keyValue.Value != "inf")
							{
								Time = Convert.ToInt32 (keyValue.Value);
							}
						}
						break;
					case ETestTags.Mode:
						{
							var modeString = keyValue.Value.ToLower ();
							if (modeString == ModePunish)
							{
								Mode = ETestMode.Punish;
							}
							else
							{
								if (modeString == ModeLoyal)
								{
									Mode = ETestMode.Loyal;
								}
							}
						}
						break;
					case ETestTags.Question:
						{
							// Add previous question to the list
							if (question != null)
							{
								questions.Add (question);
							}
							question = new TestQuestion (keyValue.Value);
						}
						break;
					case ETestTags.Value:
						{
							// Ignore this keyword if no Question keyword encountered
							if (question != null)
							{
								question.Value = Convert.ToSingle (keyValue.Value);
							}
						}
						break;
					case ETestTags.Answer:
						{
							// Ignore this keyword if no Question keyword encountered
							if (question != null)
							{
								var answer = new TestAnswer (keyValue.Value, false);
								question.Answers.Add (answer);
							}
						}
						break;
					case ETestTags.AnswerTrue:
						{
							// Ignore this keyword if no Question keyword encountered
							if (question != null)
							{
								var answer = new TestAnswer (keyValue.Value, true);
								question.Answers.Add (answer);
							}
						}
						break;
					default:
						continue;
				}
			}
			while(line != null && line.Length != 0);
			// Add last question we've parsed
			if (question != null)
			{
				if (!questions.Contains (question))
				{
					questions.Add (question);
				}
			}
			testFile.Close ();
		}

		// Shuffle questions so they don't repeat their order each time
		public void Shuffle()
		{
			var questionsOld = questions;
			questions = new List<TestQuestion> ();
			var count = questionsOld.Count;
			while (questions.Count < count)
			{
				var i = rng.Next (0, questionsOld.Count);
				questions.Add (questionsOld[i]);
				questionsOld.RemoveAt (i);
			}
			questionsOld.Clear ();
			// Shuffle answers in questions
			foreach (var question in questions)
			{
				question.Shuffle (rng);
			}
		}

		// Infinite test time definition
		public static int TimeInfinite = 0;

		List<TestQuestion> questions = new List<TestQuestion>();
		// Shuffle randomizer
		Random rng = new Random();
	}
}

