using System;

/*
 * Project: MyTest App
 * Module: TestAnswer
 * Author: Andrey N. Glushenkov
 * Date: 03.10.2017
 * Description:
 * TestAnswer module is a container for question answer object.
 * It holds answer text and if this answer is correct.
 */
namespace MyTestLib
{
	public class TestAnswer : ICloneable
	{
		/// <summary>
		/// Answer text
		/// </summary>
		public string Text { get; private set; }

		/// <summary>
		/// Check if this answer is correct
		/// </summary>
		public bool Correct { get; private set; }

		public TestAnswer (string text, bool correct)
		{
			Text = text;
			Correct = correct;
		}

		public object Clone()
		{
			return new TestAnswer(Text, Correct);
		}
	}
}

