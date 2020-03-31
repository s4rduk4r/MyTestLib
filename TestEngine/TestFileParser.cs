using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace MyTestLib
{
	internal static class TestFileParser
	{
		// Auxiliary methods and data
		// Known tags
		public enum ETestTags
		{
			NULL,	// No tag from the list below
			Test,
			Author,
			Date,
			Time,
			Mode,
			Question,
			Value,
			Answer,
			AnswerTrue
		}

		static string CommonTag = "\\s*[=]\\s*(.+)";
		static string TestTag = string.Format ("^Test{0}", CommonTag);
		static string AuthorTag = string.Format("^Author{0}", CommonTag);
		static string DateTag = string.Format("^Date{0}", CommonTag);
		static string TimeTag = string.Format("^Time{0}", CommonTag);
		static string ModeTag = string.Format("^Mode{0}", CommonTag);
		static string QuestionTag = string.Format("^Question{0}", CommonTag);
		static string ValueTag = string.Format("^Value{0}", CommonTag);
		static string AnswerTag = string.Format("^Answer{0}", CommonTag);
		static string AnswerTrueTag = string.Format ("^Answer[+]{0}", CommonTag);

		// Empty 
		public static KeyValuePair<ETestTags, string> KeyValueNone = new KeyValuePair<ETestTags, string>(ETestTags.NULL, null);

		public static KeyValuePair<ETestTags, string> GetKeyValueFromLine(string line)
		{
			// Test
			var m = Regex.Match (line, TestTag);
			if (m.Success)
			{
				return new KeyValuePair<ETestTags, string> (ETestTags.Test, m.Groups[1].Value);
			}
			// Author
			m = Regex.Match(line, AuthorTag);
			if (m.Success)
			{
				return new KeyValuePair<ETestTags, string> (ETestTags.Author, m.Groups[1].Value);
			}
			// Date
			m = Regex.Match(line, DateTag);
			if (m.Success)
			{
				return new KeyValuePair<ETestTags, string> (ETestTags.Date, m.Groups[1].Value);
			}
			// Time
			m = Regex.Match(line, TimeTag);
			if (m.Success)
			{
				return new KeyValuePair<ETestTags, string> (ETestTags.Time, m.Groups[1].Value);
			}
			// Mode
			m = Regex.Match(line, ModeTag);
			if (m.Success)
			{
				return new KeyValuePair<ETestTags, string> (ETestTags.Mode, m.Groups[1].Value);
			}
			// Question
			m = Regex.Match(line, QuestionTag);
			if (m.Success)
			{
				return new KeyValuePair<ETestTags, string> (ETestTags.Question, m.Groups[1].Value);
			}
			// Value
			m = Regex.Match(line, ValueTag);
			if (m.Success)
			{
				return new KeyValuePair<ETestTags, string> (ETestTags.Value, m.Groups[1].Value);
			}
			// Answer
			m = Regex.Match(line, AnswerTag);
			if (m.Success)
			{
				return new KeyValuePair<ETestTags, string> (ETestTags.Answer, m.Groups[1].Value);
			}
			// AnswerTrue
			m = Regex.Match(line, AnswerTrueTag);
			if (m.Success)
			{
				return new KeyValuePair<ETestTags, string> (ETestTags.AnswerTrue, m.Groups[1].Value);
			}

			// Nothing has been found
			return KeyValueNone;
		}
	}
}

