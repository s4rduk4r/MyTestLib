using System;
using System.Collections.Generic;

namespace MyTestLib
{
	public class TestResults
	{
		/// <summary>
		/// Answer on question
		/// </summary>
		/// <param name="question">Question to answer</param>
		/// <param name="answers">Answer given to question</param>
		public void Answer(TestQuestion question, IList<TestAnswer> answers)
		{
			collectedAnswers.Add (new KeyValuePair<TestQuestion, IList<TestAnswer>> (question, answers));
		}

		/// <summary>
		/// Remove answers on question
		/// </summary>
		/// <param name="question">Question to forget a given answer</param>
		public void ForgetAnswer(TestQuestion question)
		{
			foreach (var answer in collectedAnswers)
			{
				if (answer.Key == question)
				{
					collectedAnswers.Remove (answer);
					return;
				}
			}
		}

		/// <summary>
		/// Get collected answers on specific question
		/// </summary>
		/// <param name="question">Question to collect the answers for</param>
		/// <returns></returns>
		public IList<TestAnswer> Answered(TestQuestion question)
		{
			foreach (var answer in collectedAnswers)
			{
				if (answer.Key == question)
				{
					return answer.Value;
				}
			}
			return new List<TestAnswer>();
		}

		/// <summary>
		/// Get score for a particular question
		/// </summary>
		/// <param name="question">Question to count the score</param>
		/// <returns></returns>
		public float QuestionScore(TestQuestion question)
		{
			float score = 0;
			foreach(var questionInfo in collectedAnswers)
			{
				if(questionInfo.Key == question)
				{
					var answers = questionInfo.Value;

					// Mode rules
					switch (TestState.Mode)
					{
					case TestState.ETestMode.Loyal:
						{
							foreach (var answer in answers)
							{
								if (answer.Correct)
								{
									score += question.CorrectValue;
								}
								else
								{
									score += questionInfo.Key.PunishValue;
								}
							}
							if (score <= 0)
							{
								score = 0;
							}
						}
						break;
					case TestState.ETestMode.Punish:
						{
							foreach (var answer in answers)
							{
								if (answer.Correct)
								{
									score += question.CorrectValue;
								}
								else
								{
									score += questionInfo.Key.PunishValue;
								}
							}
						}
						break;
					default:
						throw new NotImplementedException ("Неизвестный режим работы!\nUnkown test mode!");
					}
				}
			}
			return score;
		}

		/// <summary>
		/// User's score
		/// </summary>
		public float Score
		{
			get
			{
				float score = 0;
				foreach (var questionInfo in collectedAnswers)
				{
					// Correct answer value
					var value = questionInfo.Key.CorrectValue;
					var punish = questionInfo.Key.PunishValue;
					var answers = questionInfo.Value;
					// Mode rules
					switch (TestState.Mode)
					{
					case TestState.ETestMode.Loyal:
						{
							foreach (var answer in answers)
							{
								if (answer.Correct)
								{
									score += value;
								}
								else
								{
									score += punish;
								}
							}
							if (score <= 0)
							{
								score = 0;
							}
						}
						break;
					case TestState.ETestMode.Punish:
						{
							foreach (var answer in answers)
							{
								if (answer.Correct)
								{
									score += value;
								}
								else
								{
									score += punish;
								}
							}
						}
						break;
					default:
						throw new NotImplementedException ("Неизвестный режим работы!\nUnkown test mode!");
					}
				}
				return score;
			}
		}

		/// <summary>
		/// Test state
		/// </summary>
		public TestState TestState;

		public TestResults(TestState ts)
		{
			TestState = ts;
		}

		// Answers collected from user
		List<KeyValuePair<TestQuestion, IList<TestAnswer>>> collectedAnswers = new List<KeyValuePair<TestQuestion, IList<TestAnswer>>>();
	}
}

