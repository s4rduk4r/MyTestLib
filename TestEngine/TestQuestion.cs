using System;
using System.Collections.Generic;
using System.Threading;

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
		/// <summary>
		/// Question text
		/// </summary>
		public string Text
		{
			get
			{
				return text.Length == 0 ? "ERROR: Empty string in ctor TestQuestion()" : text;
			}
		}

		/// <summary>
		/// Question total value. Negative values are treated as zero
		/// </summary>
		public float Value
		{
			get
			{
				return value;
			}
			set
			{
				mutex.WaitOne();
				if (value < 0)
				{
					this.value = 0;
				}
				else
				{
					this.value = value;
				}
				mutex.ReleaseMutex();
			}
		}

		/// <summary>
		/// Wrong answer penalty
		/// </summary>
		public float PunishValue
		{
			get
			{
				return -Value / (Answers.Count - CorrectAnswers.Count);
			}
		}

		/// <summary>
		/// Get correct answer value
		/// </summary>
		public float CorrectValue
		{
			get
			{
				return Value / CorrectAnswers.Count;
			}
		}

		/// <summary>
		/// Answers affiliated with the Question
		/// </summary>
		public IList<TestAnswer> Answers
		{
			get
			{
				return answers;
			}
			set
			{
				mutex.WaitOne();
				if (value != null)
				{
					answers = (List<TestAnswer>)value;
				}
				mutex.ReleaseMutex();
			}
		}

		/// <summary>
		/// Correct answers
		/// </summary>
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

		/// <summary>
		/// Shuffle answers so they don't repeat their order each time
		/// </summary>
		/// <param name="rng"></param>
		public void Shuffle(Random rng)
		{
			mutex.WaitOne();
			var newOrder = new List<(double Id, TestAnswer Answer)>();
			for (int i = 0; i < answers.Count; i++)
			{
				newOrder.Add((rng.NextDouble(), answers[i]));
			}
			newOrder.Sort(
				(a, b) =>
				{
					if (a.Id > b.Id) return 1;
					if (a.Id == b.Id) return 0;
					if (a.Id < b.Id) return -1;
					return 0;
				}
				);
			for (int i = 0; i < answers.Count; i++)
			{
				answers[i] = newOrder[i].Answer;
			}
			mutex.ReleaseMutex();
		}

		public TestQuestion (string text)
		{
			this.text = text;
		}

		string text = "";
		float value = 0.0f;
		List<TestAnswer> answers = new List<TestAnswer>();
		
		private Mutex mutex = new Mutex();
	}
}

