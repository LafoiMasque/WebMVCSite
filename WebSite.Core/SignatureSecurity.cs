using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Core
{
	public class SignatureSecurity
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="secretKey">密钥</param>
		/// <param name="plain">简单的</param>
		/// <returns></returns>
		public static string HmacSHA256(string secretKey, string plain)
		{
			var keyBytes = Encoding.UTF8.GetBytes(secretKey);
			var plainBytes = Encoding.UTF8.GetBytes(plain);

			using (var hmacsha256 = new HMACSHA256(keyBytes))
			{
				var sb = new StringBuilder();
				var hashValue = hmacsha256.ComputeHash(plainBytes);
				foreach (byte x in hashValue)
				{
					sb.Append(String.Format("{0:x2}", x));
				}
				return sb.ToString();
			}
		}

		public static string MakeSignPlain(SortedDictionary<string, string> queryString, string time, string random)
		{
			var sb = new StringBuilder();
			foreach (var keyValue in queryString)
			{
				sb.AppendFormat("{0}={1}&", keyValue.Key, keyValue.Value);
			}
			if (sb.Length > 1)
			{
				sb.Remove(sb.Length - 1, 1);
			}
			sb.Append(time);
			sb.Append(random);

			return sb.ToString().ToUpper();
		}

		public static bool Valid(string requestSign, string signPlain, string time, string secretKey)
		{
			bool isOK = false;
			if (!(string.IsNullOrEmpty(time) || string.IsNullOrEmpty(requestSign) || string.IsNullOrEmpty(signPlain)))
			{
				//is in range
				var now = DateTime.Now;
				long requestTime = 0;
				if (long.TryParse(time, out requestTime))
				{
					var max = now.AddMinutes(5).ToString("yyyyMMddHHmmss");
					var min = now.AddMinutes(-5).ToString("yyyyMMddHHmmss");
					if (long.Parse(max) >= requestTime && long.Parse(min) <= requestTime)
					{
						//hashmac
						var sign = HmacSHA256(secretKey, signPlain);
						isOK = requestSign.Equals(sign, StringComparison.CurrentCultureIgnoreCase);
					}
				}
			}
			return isOK;
		}
	}
}
