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
	public class TestAnswer
	{
		// Answer text
		public string Text
		{
			get
			{
				return text;
			}
		}

		// Check if this answer is correct
		public bool Correct
		{
			get
			{
				return correct;
			}
		}

		public TestAnswer (string text, bool correct)
		{
			this.text = text;
			this.correct = correct;
		}

		string text;
		bool correct;
	}
}

