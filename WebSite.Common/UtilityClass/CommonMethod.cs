using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebSite.Common.UtilityClass
{
	public class CommonMethod
	{
		///// <summary>
		///// 获取图片
		///// </summary>
		///// <param name="filePath">图片路径</param>
		///// <returns></returns>
		//public static ImageSource GetImageSource(string filePath)
		//{
		//	BitmapImage image = null;
		//	if (File.Exists(filePath))
		//	{
		//		MemoryStream stmimage = new MemoryStream();
		//		System.Drawing.Image imgFullSize = System.Drawing.Image.FromFile(filePath);
		//		imgFullSize.Save(stmimage, System.Drawing.Imaging.ImageFormat.Jpeg);
		//		image = new BitmapImage();
		//		image.BeginInit();
		//		image.StreamSource = stmimage;
		//		image.EndInit();
		//	}
		//	return image;
		//}

		/// <summary>
		/// 创建方法，删除文件夹中的所有文件
		/// </summary>
		/// <param name="file"></param>
		public static void DeleteFile(string filePath)
		{
			try
			{
				if (Directory.Exists(filePath))// 判断文件夹是否存在
				{
					//去除文件夹和子文件的只读属性
					//去除文件夹的只读属性
					DirectoryInfo fileInfo = new DirectoryInfo(filePath);
					fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
					//去除文件的只读属性
					File.SetAttributes(filePath, FileAttributes.Normal);
					//判断文件夹是否还存在
					if (Directory.Exists(filePath))
					{
						foreach (string item in Directory.GetFileSystemEntries(filePath))
						{
							if (File.Exists(item))
							{
								//如果有子文件删除文件
								File.Delete(item);
							}
							else
							{
								//循环递归删除子文件夹 
								DeleteFile(item);
							}
						}
					}
				}
			}
			catch (Exception ex) // 异常处理
			{
				
			}
		}

		/// <summary>
		/// 从一个目录将其内容移动到另一目录  
		/// </summary>
		/// <param name="directorySource">源目录</param>
		/// <param name="directoryTarget">目标目录</param>
		public static void MoveFolderTo(string directorySource, string directoryTarget)
		{
			try
			{
				if (Directory.Exists(directorySource))
				{
					//检查是否存在目的目录  
					if (!Directory.Exists(directoryTarget))
					{
						Directory.CreateDirectory(directoryTarget);
					}
					//先来移动文件  
					DirectoryInfo directoryInfo = new DirectoryInfo(directorySource);
					FileInfo[] files = directoryInfo.GetFiles();
					//移动所有文件  
					foreach (FileInfo file in files)
					{
						//如果自身文件在运行，不能直接覆盖，需要重命名之后再移动  
						if (File.Exists(Path.Combine(directoryTarget, file.Name)))
						{
							if (File.Exists(Path.Combine(directoryTarget, file.Name + ".bak")))
							{
								File.Delete(Path.Combine(directoryTarget, file.Name + ".bak"));
							}
							File.Move(Path.Combine(directoryTarget, file.Name), Path.Combine(directoryTarget, file.Name + ".bak"));

						}
						file.MoveTo(Path.Combine(directoryTarget, file.Name));
						//File.Move(file.FullName, Path.Combine(directoryTarget, file.Name));
					}
					//最后移动目录  
					DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
					foreach (DirectoryInfo dir in directoryInfoArray)
					{
						MoveFolderTo(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
					}
				}
			}
			catch (Exception ex) // 异常处理
			{
				
			}
		}

		/// <summary>
		/// 从一个目录将其内容复制到另一目录
		/// </summary>
		/// <param name="directorySource">源目录</param>
		/// <param name="directoryTarget">目标目录</param>
		public static void CopyFolder(string directorySource, string directoryTarget, IEnumerable<string> targetCollection)
		{
			if (targetCollection != null)
				CopyFolder(directorySource, directoryTarget, o => targetCollection.Any(obj => o.Contains(obj)));
		}

		/// <summary>
		/// 从一个目录将其内容复制到另一目录
		/// </summary>
		/// <param name="directorySource">源目录</param>
		/// <param name="directoryTarget">目标目录</param>
		/// <param name="filePredicate">用于测试文件是否满足条件的函数</param>
		public static void CopyFolder(string directorySource, string directoryTarget, Func<string, bool> filePredicate)
		{
			try
			{
				if (Directory.Exists(directorySource))
				{
					//检查是否存在目的目录  
					if (!Directory.Exists(directoryTarget))
					{
						Directory.CreateDirectory(directoryTarget);
					}
					//先来复制文件  
					DirectoryInfo directoryInfo = new DirectoryInfo(directorySource);
					FileInfo[] files = directoryInfo.GetFiles();
					//复制所有文件  
					foreach (FileInfo file in files)
					{
						if (filePredicate(file.FullName))
							file.CopyTo(Path.Combine(directoryTarget, file.Name));
					}
					//最后复制目录  
					DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
					foreach (DirectoryInfo dir in directoryInfoArray)
					{
						CopyFolder(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name), filePredicate);
					}
				}
			}
			catch (Exception ex) // 异常处理
			{
				
			}
		}

		/// <summary>
		/// 读取文件中的文本集合
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <param name="ignoreEmptyLine">是否忽略空白行</param>
		/// <returns>文本集合</returns>
		public static List<string> ReadFileText(string filePath, bool ignoreEmptyLine = true)
		{
			List<string> ltResult = null;
			if (File.Exists(filePath))
			{
				ltResult = new List<string>();
				string stringLine = string.Empty;
				using (StreamReader sw = new StreamReader(filePath))
				{
					while (!sw.EndOfStream)
					{
						stringLine = sw.ReadLine();
						if (stringLine != null && (!ignoreEmptyLine || (ignoreEmptyLine && stringLine.ObjectToString() != "")))
						{
							ltResult.Add(stringLine);
						}
					}
					sw.Close();
				}
			}
			return ltResult;
		}

		/// <summary>
		/// 往文件中写入文本
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <param name="dataString">文本</param>
		/// <param name="isAppend">是否追加</param>
		public static void WriteTextToFile(string filePath, string dataString, bool isAppend = false)
		{
			WriteTextToFile(filePath, new string[] { dataString }, isAppend);
		}

		/// <summary>
		/// 往文件中写入文本集合
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <param name="dataStrings">文本集合</param>
		/// <param name="isAppend">是否追加</param>
		public static void WriteTextToFile(string filePath, IEnumerable dataStrings, bool isAppend = false)
		{
			using (StreamWriter sw = new StreamWriter(filePath, isAppend))
			{
				if (dataStrings != null)
				{
					foreach (var item in dataStrings)
					{
						sw.WriteLine(item);
					}
				}
				sw.Flush();
				sw.Close();
			}
		}

	}
}
