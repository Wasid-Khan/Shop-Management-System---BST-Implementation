using System;
using System.Collections.Generic;
using System.Text;

namespace AVL_Tree
{
	public class product
	{
		/*Private variables*/
		private int id { set; get; }
		public int height;
		private String name { set; get; }
		private String brand { set; get; }
		private String cost { set; get; }
		public product left;
		public product right;

		/*product class constructor*/
		public product(int id, String name, String brand, String cost)
		{
			this.id = id;
			this.height = 1;
			this.name = name;
			this.brand = brand;
			this.cost = cost;
			this.left = null;
			this.right = null;
		}

		public int ID
		{
			get { return id; }
			set { id = value; }
		}
		public string NAME
		{
			get { return name; }
			set { name = value; }
		}
		public string BRAND
		{
			get { return brand; }
			set { brand = value; }
		}
		public string COST
		{
			get { return cost; }
			set { cost = value; }
		}

	}
}
