using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
/*
 * Project: MyTest App
 * Module: TestFileEncDec
 * Author: Andrey N. Glushenkov
 * Date: 05.10.2017
 * Description:
 * TestFileEncDec performs encryption and decryption of MyTest test files. It uses AES
 */
using System.Text.RegularExpressions;

namespace MyTestLib
{
	namespace Protector
	{
		public class TestFileEncDec
		{
			// File extensions of raw MyTest file
			public static string MyTestRaw = ".mytest";
			// Compiled MyTest file. MTC = MyTest Compiled
			public static string MyTestCompiled = ".mtc";

			// Separator bytes for IV and Key
			byte[] Separator = { 0xFA, 0xDA, 0xBA, 0xDA };

			public enum ETestFileMode
			{
				Encode,
				Decode
			}

			public ETestFileMode Mode = ETestFileMode.Decode;
			public string Filename = "";

			// Encrypted or Decrypted file text
			public byte[] Data
			{
				get
				{
					switch (Mode)
					{
						case ETestFileMode.Encode:
							{
								// Generate manifest
								var manifest = GenerateManifest();
								// Encrypt text
								//var ifile = File.OpenRead(Filename);
								var aes = new AesCryptoServiceProvider();
								var encryptor = aes.CreateEncryptor(manifest.Key, manifest.IV);
								byte[] data = null;
								using (var msCrypted = new MemoryStream())
								{
									// Embed manifest
									msCrypted.Write(manifest.IV, 0, manifest.IV.Length);
									msCrypted.Write(Separator, 0, 4);
									msCrypted.Write(manifest.Key, 0, manifest.Key.Length);
									msCrypted.Write(Separator, 0, 4);
									// Append encrypted text to manifest
									using (var cryptoStream = new CryptoStream(msCrypted, encryptor, CryptoStreamMode.Write))
									{
										var ifile = File.OpenRead(Filename);
										ifile.CopyTo(cryptoStream);
										ifile.Close();
									}
									data = msCrypted.ToArray();
								}

								// GZip the result
								var gzipedData = new MemoryStream();
								var gzip = new GZipStream(gzipedData, CompressionLevel.Fastest);
								gzip.Write(data, 0, data.Length);
								gzip.Close();
								return gzipedData.ToArray();
							}
						case ETestFileMode.Decode:
							{
								// GUnZip
								var msData = new MemoryStream();
								using (var gzip = new GZipStream(File.OpenRead(Filename), CompressionMode.Decompress))
								{
									gzip.CopyTo(msData);
									gzip.Close();
								}
								var manifest = new TestFileManifest();
								// Read manifest
								byte[] data = msData.ToArray();
								msData.Seek(0, SeekOrigin.Begin);
								for (int i = 0; i < msData.ToArray().Length; ++i)
								{
									// Separator found
									if (data[i] == Separator[0] && data[i + 1] == Separator[1]
										&& data[i + 2] == Separator[2] && data[i + 3] == Separator[3])
									{
										// IV
										if (manifest.IV == null)
										{
											var size = (int)(i - msData.Position);
											manifest.IV = new byte[size];
											msData.Read(manifest.IV, 0, size);
											msData.Seek(4, SeekOrigin.Current);
										}
										// Key
										else
										{
											var size = (int)(i - msData.Position);
											manifest.Key = new byte[size];
											msData.Read(manifest.Key, 0, size);
											msData.Seek(4, SeekOrigin.Current);
											break;
										}
									}
								}
								var encryptedTextSize = (int)(msData.Length - msData.Position);
								data = new byte[encryptedTextSize];
								msData.Read(data, 0, encryptedTextSize);
								msData = new MemoryStream(data);
								// Decrypt text
								using (var aes = new AesCryptoServiceProvider())
								{
									aes.IV = manifest.IV;
									aes.Key = manifest.Key;
									using (var decryptor = aes.CreateDecryptor())
									{
										var cryptoStream = new CryptoStream(msData, decryptor, CryptoStreamMode.Read);
										var msDecryptedData = new MemoryStream();
										cryptoStream.CopyTo(msDecryptedData);
										return msDecryptedData.ToArray();
									}
								}
							}
					}
					return null;
				}
			}

			public TestFileEncDec(string filename, ETestFileMode mode)
			{
				if (filename == null || filename.Length <= 0)
				{
					throw new ArgumentNullException("TestFileEncDec(): filename is null or zero-length");
				}
				Filename = filename;
				Mode = mode;
			}

			// Generate manifest
			TestFileManifest GenerateManifest()
			{
				var aes = new AesCryptoServiceProvider();
				aes.GenerateIV();
				aes.GenerateKey();
				var manifest = new TestFileManifest(aes.IV, aes.Key);
				return manifest;
			}
		}
	}
}
