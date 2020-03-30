using System;
/*
 * Project: MyTest App
 * Module: TestFileManifest
 * Author: Andrey N. Glushenkov
 * Date: 05.10.2017
 * Description:
 * TestFileManifest holds AES IV and Key to encrypt and decrypt test file.
 */
namespace MyTestLib
{
	namespace Protector
	{
		public class TestFileManifest
		{
			public byte[] IV = null;
			public byte[] Key = null;

			public TestFileManifest()
			{
			}

			public TestFileManifest(byte[] iv, byte[] key)
			{
				IV = iv;
				Key = key;
			}
		}
	}
}
