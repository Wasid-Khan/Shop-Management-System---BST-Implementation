using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinaryTree
{
    /*Interface for Splay Tree*/
    public interface BT_Interface<E>
    {
        public void insert(E obj);
        E search(int id);
        void delete(int id);
    }

    public class BT_Class : BT_Interface<product>
    {
        public product root;
        public static Boolean isDeleted = false;
        public static List<product> nodesToArray = new List<product>();
        public static Boolean searchFlag = false;

        public void insert(product obj)
        {
            insertHelper(root, obj);
        }
        /*Helper Method for insertion method above*/
        private void insertHelper(product temp, product key)
        {
            if (temp == null)
            {
                root = key;
                return;
            }
            Queue<product> q = new Queue<product>();
            q.Enqueue(temp);

            /* we will traversal level order until we find an empty place in the tree.*/
            while (q.Count!=0)
            {
                temp = q.Peek();
                q.Dequeue();

                if (temp.left == null)
                {
                    temp.left = key;
                    break;
                }
                else
                    q.Enqueue(temp.left);

                if (temp.right == null)
                {
                    temp.right = key;
                    break;
                }
                else
                    q.Enqueue(temp.right);
            }
        }


        public void delete(int id)
        {
			delete(root, id);
		}
		/*Helper methods to the delete method of the binary tree*/
		/*fucntion to delete given node value in binary tree*/
		private void delete(product root, int key)
		{
			if (root == null)
				return;

			if (root.left == null && root.right == null)
			{
				if (root.ID == key)
				{
					root = null;
					return;
				}
				else return;
			}

			Queue<product> q = new Queue<product>();
			q.Enqueue(root);
			product temp = null, keyNode = null;

			/*We will do level order traversal untill we find the empty node in a tree*/
			while (q.Count!=0)
			{
				temp = q.Peek();
				q.Dequeue();

				if (temp.ID == key) keyNode = temp;

				if (temp.left != null) q.Enqueue(temp.left);

				if (temp.right != null) q.Enqueue(temp.right);
			}

			if (keyNode != null)
			{
				int x = temp.ID;
				deleteDeepest(root, temp);
				keyNode.ID = x;
			}
		}

		/*Helper method for delete method to delete the deepest node in tree*/
		private void deleteDeepest(product root, product nodeToDel)
		{
			Queue<product> q = new Queue<product>();
			q.Enqueue(root);
			product temp = null;

			/*We will do level order traversal untill we find the empty node*/
			while (q.Count!=0)
			{
				temp = q.Peek();
				q.Dequeue();

				if (temp == nodeToDel)
				{
					temp = null;
					return;
				}
				if (temp.right != null)
				{
					if (temp.right == nodeToDel)
					{
						temp.right = null;
						return;
					}
					else { q.Enqueue(temp.right); }
				}
				if (temp.left != null)
				{
					if (temp.left == nodeToDel)
					{
						temp.left = null;
						return;
					}
					else { q.Enqueue(temp.left); }
				}
			}
		}

		public product search(int id)
        {
			searchNodeHelper(root, id);
			return root;
		}
		/*Helper method of search Method above*/
		private void searchNodeHelper(product temp, int value)
		{
			//Check whether tree is empty  
			if (root == null)
			{
				Console.WriteLine("Tree is empty");
			}
			else
			{
				/*If the node is found*/
				if (temp.ID == value)
				{
					searchFlag = true;
					Console.WriteLine("ID: " + temp.ID + "\nNAME: " + temp.NAME +
				   "\nBRAND: " + temp.BRAND + "\nPRICE ($): " + temp.COST + "\n");
					return;
				}
				/*Search in left subtree*/
				if (temp.left != null)
				{
					searchNodeHelper(temp.left, value);
				}
				/* Search in right subtree */
				if (temp.right != null)
				{
					searchNodeHelper(temp.right, value);
				}
			}
		}

		/*Inorder traversal of avl tree*/
		public void inorder()
		{
			inorderRecursion(root);
		}

		/*Helper method for in-order method*/
		private void inorderRecursion(product root)
		{
			if (root == null)
				return;
			inorderRecursion(root.left);
			Console.WriteLine("ID: " + root.ID + "\nNAME: " + root.NAME +
				   "\nBRAND: " + root.BRAND + "\nPRICE ($): " + root.COST + "\n");
			inorderRecursion(root.right);
		}


		/*Binary node to array*/
		public void nToA(product root)
		{
			if (root != null)
			{
				nToA(root.left);
				nodesToArray.Add(root);
				nToA(root.right);
			}
		}

		/*Count total records in bst*/
		public int size(product node)
		{
			if (node == null)
				return 0;
			else
				return (size(node.left) + 1 + size(node.right));
		}

	}
	
	/******************************************************************************************
     ******************************************************************************************/
	class BT
    {
        static void Main(string[] args)
        {
			BT_Class bt = new BT_Class();
			string fileName = "stockData.txt";

			/*From file to BST*/
			// Read file using StreamReader. Reads file line by line
			if (File.Exists(fileName))
			{
				using (StreamReader file = new StreamReader(fileName))
				{
					string ln = file.ReadLine();
					while (ln != null)
					{
						String[] tokens = ln.Split(",");
						if (tokens.Length == 1)
						{
							product p = new product(int.Parse(tokens[0]), null, null, null);
							bt.insert(p);
						}
						if (tokens.Length == 2)
						{
							product p = new product(int.Parse(tokens[0]), tokens[1], null, null);
							bt.insert(p);
						}
						if (tokens.Length == 3)
						{
							product p = new product(int.Parse(tokens[0]), tokens[1], tokens[2], null);
							bt.insert(p);
						}
						if (tokens.Length == 4)
						{
							product p = new product(int.Parse(tokens[0]), tokens[1], tokens[2], tokens[3]);
							bt.insert(p);
						}
						ln = file.ReadLine();
					}
					file.Close();
				}
			}
			else
			{
				Console.WriteLine("File not found!");
			}

			/******************* User Menu********************/

			int again;
			do
			{
				Console.WriteLine("======================================");
				Console.WriteLine("  Welcome to Stock Management System");
				Console.WriteLine("======================================");
				Console.WriteLine("\t1. Insert new Record.");
				Console.WriteLine("\t2. Delete Record.");
				Console.WriteLine("\t3. Search Record.");
				Console.WriteLine("\t4. See all Records.");
				Console.WriteLine("\t5. Save & Exit.");
				Console.WriteLine("======================================");
				Console.Write("Enter your choice >> ");
				int a = Convert.ToInt32(Console.ReadLine());

				switch (a)
				{
					case 1:
						Console.WriteLine("_____________________________________");
						Console.Write("Enter ID for Product >>");
						int id = Convert.ToInt32(Console.ReadLine());
						Console.Write("Enter Name of Product >> ");
						String name = Console.ReadLine();
						Console.Write("Enter Brand of Product >>");
						String brand = Console.ReadLine();
						Console.Write("Enter Price of Product >> ");
						String price = Console.ReadLine();

						product cs = new product(id, name, brand, price);
						bt.insert(cs);						
						Console.WriteLine("\n------Inserted Successfully-------");						
						break;

					/*To delete record by id*/
					case 2:
						Console.WriteLine("``````````````````````````````````````");
						Console.Write("Enter ID to Delete >> ");
						int idDel = Convert.ToInt32(Console.ReadLine());

						/*Firstly to search*/
						bt.search(idDel);
						if (BT_Class.searchFlag != false)
						{
							Console.Write("\nConfirm to delete [yes(1)]");
							int conf = Convert.ToInt32(Console.ReadLine());
							if (conf == 1)
							{
								bt.delete(idDel);
								Console.WriteLine("Item Deleted!");
							}
							BT_Class.searchFlag = false;
						}
						else
						{
							Console.WriteLine("Item not present");
						}
						break;

					/*To search record by id*/
					case 3:
						Console.WriteLine("_____________________________________");
						Console.Write("Enter ID to Search >> ");
						int idSrch = Convert.ToInt32(Console.ReadLine());

						bt.search(idSrch);
						if (BT_Class.searchFlag == false)
						{
							Console.WriteLine("Record Not present!");
						}
						else
						{
							BT_Class.searchFlag = false;
						}
						break;

					/*to retrieve all the records*/
					case 4:
						Console.WriteLine("_____________________________________");
						int total = bt.size(bt.root);
						Console.WriteLine("\tTotal Records: " + total);
						Console.WriteLine("``````````````````````````````````````");
						bt.inorder();

						break;

					/*to save records in file and exit*/
					case 5:
						/*First to retrieve records from bst*/
						bt.nToA(bt.root);

						/*Secondly, to shuffle and store in file*/
						try
						{
							// Write file using StreamWriter  
							using (StreamWriter writer = new StreamWriter(fileName))
							{
								//Collections.shuffle(BST_Class.nodesToArray);
								var shuffled = BT_Class.nodesToArray.OrderBy(x => Guid.NewGuid()).ToList();
								for (int i = 0; i < shuffled.Count; i++)
								{
									product temp = BT_Class.nodesToArray[i];
									writer.WriteLine(temp.ID + "," + temp.NAME + "," + temp.BRAND + "," + temp.COST);
								}
							}
						}
						catch (Exception i)
						{
							Console.WriteLine(i.Message);
						}
						/*close the application*/
						Console.WriteLine("__________________________________________________");
						int tot = bt.size(bt.root);
						Console.WriteLine("\n[" + tot + "] Products Data Saved in " + fileName + " File");
						Console.WriteLine("```````````````````````````````````````````````````");
						return;
				}
				Console.WriteLine("_____________________________________");
				Console.WriteLine("\nDo you want to continue ? [yes(1)]");
				again = Convert.ToInt32(Console.ReadLine());
				Console.Clear(); /*to clear screen*/

			} while (again == 1);

		}
	}
}
