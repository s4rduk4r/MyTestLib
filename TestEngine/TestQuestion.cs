using System;
using System.Collections.Generic;

/*
 * Project: MyTest App
 * Module: Question
 * Author: Andrey N. Glushenkov
 * Date: 03.10.2017
 * Description:
 * Question module is a container for question object.
 * It holds question text, possible answers and their values.
 */
namespace MyTestLib
{
	public class TestQuestion
	{
		// Question text
		public string Text
		{
			get
			{
				return text.Length == 0 ? "ERROR: Empty string in ctor TestQuestion()" : text;
			}
		}

		// Question total value. Negative values are treated as zero
		public float Value
		{
			get
			{
				return value;
			}
			set
			{
				if (value < 0)
				{
					this.value = 0;
				}
				else
				{
					this.value = value;
				}
			}
		}

		// Wrong answer penalty
		public float PunishValue
		{
			get
			{
				return -Value / (Answers.Count - CorrectAnswers.Count);
			}
		}

		// Get correct answer value
		public float CorrectValue
		{
			get
			{
				return Value / CorrectAnswers.Count;
			}
		}

		// Answers affiliated with the Question
		public IList<TestAnswer> Answers
		{
			get
			{
				return answers;
			}
			set
			{
				if (value != null)
				{
					answers = (List<TestAnswer>)value;
				}
			}
		}

		// Correct answers
		public IList<TestAnswer> CorrectAnswers
		{
			get
			{
				var correctAnswers = new List<TestAnswer> ();
				foreach (var answer in answers)
				{
					if (answer.Correct)
					{
						correctAnswers.Add (answer);
					}
				}
				return correctAnswers;
			}
		}

		// Shuffle answers so they don't repeat their order each time
		public void Shuffle(Random rng)
		{
			var answersOld = answers;
			answers = new List<TestAnswer> ();
			var count = answersOld.Count;
			while (answers.Count < count)
			{
				var i = rng.Next (0, answersOld.Count);
				answers.Add (answersOld[i]);
				answersOld.RemoveAt (i);
			}
			answersOld.Clear ();
		}

		public TestQuestion (string text)
		{
			this.text = text;
		}

		string text = "";
		float value = 0.0f;
		List<TestAnswer> answers = new List<TestAnswer>();
	}
}

