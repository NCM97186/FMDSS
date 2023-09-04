using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FMDSS.Models.MySecurity
{
	public static class SecurityCode
	{
		public static string Encode(string encodeMe)
		{
			try
			{
				string textToEncrypt = encodeMe;//"WaterWorld";
				string ToReturn = "";
				//string publickey = "12345678";
				//string secretkey = "87654321";
				string publickey = "56987153"; //"12345678";
				string secretkey = "35178965";//"87654321";
				byte[] secretkeyByte = { };
				secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
				byte[] publickeybyte = { };
				publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
				MemoryStream ms = null;
				CryptoStream cs = null;
				byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
				using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
				{
					ms = new MemoryStream();
					cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
					cs.Write(inputbyteArray, 0, inputbyteArray.Length);
					cs.FlushFinalBlock();
					ToReturn = Convert.ToBase64String(ms.ToArray());
				}
				return ToReturn;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static string Decode(string decodeMe)
		{
			try
			{
				string textToDecrypt = decodeMe; //"6+PXxVWlBqcUnIdqsMyUHA==";
				string ToReturn = "";
				string publickey = "56987153"; //"12345678";
				string secretkey = "35178965";//"87654321";
				byte[] privatekeyByte = { };
				privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
				byte[] publickeybyte = { };
				publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
				MemoryStream ms = null;
				CryptoStream cs = null;
				byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
				inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
				using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
				{
					ms = new MemoryStream();
					cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
					cs.Write(inputbyteArray, 0, inputbyteArray.Length);
					cs.FlushFinalBlock();
					Encoding encoding = Encoding.UTF8;
					ToReturn = encoding.GetString(ms.ToArray());
				}
				return ToReturn;
			}
			catch (Exception ae)
			{
				throw new Exception(ae.Message, ae.InnerException);
			}
		}
		public static string EncodeUrl(string encodeMe)
		{
			try
			{
				string textToEncrypt = encodeMe;//"WaterWorld";
				string ToReturn = "";
				//string publickey = "12345678";
				//string secretkey = "87654321";
				string publickey = "76987183"; //"12345678";
				string secretkey = "38178967";//"87654321";
				byte[] secretkeyByte = { };
				secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
				byte[] publickeybyte = { };
				publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
				MemoryStream ms = null;
				CryptoStream cs = null;
				byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
				using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
				{
					ms = new MemoryStream();
					cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
					cs.Write(inputbyteArray, 0, inputbyteArray.Length);
					cs.FlushFinalBlock();
					ToReturn = Convert.ToBase64String(ms.ToArray());
				}
				return ToReturn;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static string DecodeUrl(string decodeMe)
		{
			try
			{
				string textToDecrypt = decodeMe; //"6+PXxVWlBqcUnIdqsMyUHA==";
				string ToReturn = "";
				string publickey = "76987183"; //"12345678";
				string secretkey = "38178967";//"87654321";
				
				byte[] privatekeyByte = { };
				privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
				byte[] publickeybyte = { };
				publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
				MemoryStream ms = null;
				CryptoStream cs = null;
				byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
				inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
				using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
				{
					ms = new MemoryStream();
					cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
					cs.Write(inputbyteArray, 0, inputbyteArray.Length);
					cs.FlushFinalBlock();
					Encoding encoding = Encoding.UTF8;
					ToReturn = encoding.GetString(ms.ToArray());
				}
				return ToReturn;
			}
			catch (Exception ae)
			{
				throw new Exception(ae.Message, ae.InnerException);
			}
		}
		public static string RandomString(int size, bool lowerCase = false)
		{
			var builder = new StringBuilder(size);

			// Unicode/ASCII Letters are divided into two blocks
			// (Letters 65–90 / 97–122):
			// The first group containing the uppercase letters and
			// the second group containing the lowercase.  
			Random _random = new Random();
			
			// char is a single Unicode character  
			char offset = lowerCase ? 'a' : 'A';
			const int lettersOffset = 26; // A...Z or a..z: length=26  

			for (var i = 0; i < size; i++)
			{
				var @char = (char)_random.Next(offset, offset + lettersOffset);
				builder.Append(@char);
			}

			return lowerCase ? builder.ToString().ToLower() : builder.ToString();
		}
	}
}