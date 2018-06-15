using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.DALFactory.XmlModel
{
	public class EntityModel
	{
		//[XmlAttribute]
		public string Key { get; set; }
		public string AssemblyPath { get; set; }
		public string NameSpace { get; set; }
		public string FullName { get; set; }
		public string Name { get; set; }
	}
}
