using System;
using System.Collections.Generic;

namespace MyTestLib
{
	public class TestResults
	{
		// Answer on question
		public void Answer(TestQuestion question, IList<TestAnswer> answers)
		{
			collectedAnswers.Add (new KeyValuePair<TestQuestion, IList<TestAnswer>> (question, answers));
		}

		// Remove answers on question
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

		// Get collected answers on specific question
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

		// Get score for a particular question
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

		// User's score
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

		// Test state
		public TestState TestState;

		public TestResults(TestState ts)
		{
			TestState = ts;
		}

		// Answers collected from user
		List<KeyValuePair<TestQuestion, IList<TestAnswer>>> collectedAnswers = new List<KeyValuePair<TestQuestion, IList<TestAnswer>>>();
	}
}

