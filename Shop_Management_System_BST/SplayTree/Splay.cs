using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SplayTree
{
    /*Interface for Splay Tree*/
    public interface SplayTree_Interface<E>
    {
        public void insert(E obj);
        E search(int id);
        void delete(int id);
    }

    /*Splay Tree class*/
    public class SplayTree_Class : SplayTree_Interface<product>
    {
        public product root;
        public static Boolean isDuplicate = false;
        public static List<product> nodesToArray = new List<product>();

		/* A Helper Method to right rotate subtree rooted with y */
		private product rightRotate(product x)
		{
			product y = x.left;
			x.left = y.right;
			y.right = x;
			return y;
		}

		/* A helper method to left rotate subtree rooted with x */
		private product leftRotate(product x)
		{
			product y = x.right;
			x.right = y.left;
			y.left = x;
			return y;
		}

		/*
		 * This is the method which is used to bring the key at root
		 * if the key is present in the tree, if the key is not present 
		 * in the tree then it brings the last accessed item at root in 
		 * the tree. This method modifies the tree and returns the new
		 * root of the tree.
		*/
		private product splay(product root, int key)
		{
			/*First Case; if root is null or key is on the root.*/
			if (root == null || root.ID == key)
				return root;

			/*Second Case: Key lies in left subtree */
			if (root.ID > key)
			{
				/* Key is not in tree, then we will return*/
				if (root.left == null) return root;

				/*1: Zig-Zig [left left]*/
				if (root.left.ID > key)
				{
					/* Firstly bring the key as root of left-left recursily */
					root.left.left = splay(root.left.left, key);

					/* Then do first rotation for root */
					root = rightRotate(root);
				}
				else if (root.left.ID < key) /*2: Zig-Zag [Left Right] */
				{
					/* First bring the key as root of left-right recursively */
					root.left.right = splay(root.left.right, key);

					/* Then do first rotation for root.left */
					if (root.left.right != null)
						root.left = leftRotate(root.left);
				}

				/* At last do second rotation for root */
				return (root.left == null) ? root : rightRotate(root);
			}
			else /*Third Case: Key lies in right subtree */
			{
				/* Key is not in tree, we will return */
				if (root.right == null) return root;

				/* Zig-Zag [Right Left] */
				if (root.right.ID > key)
				{
					/* Bring the key as root of right-left */
					root.right.left = splay(root.right.left, key);

					/* Then we first rotation for root.right */
					if (root.right.left != null)
						root.right = rightRotate(root.right);
				}
				else if (root.right.ID < key)/* Zag-Zag (Right Right) */
				{
					/* Bring the key as root of right-right and do first rotation*/
					root.right.right = splay(root.right.right, key);
					root = leftRotate(root);
				}

				/* Do second rotation for root */
				return (root.right == null) ? root : leftRotate(root);
			}
		}

		/*A helper method for insert method*/
		private product insert(product root, product k)
		{
			/* CASE 1: If tree is empty */
			if (root == null) return k;

			/* Bring the closest leaf node to root */
			root = splay(root, k.ID);

			/* If key is already present, then return */
			if (root.ID == k.ID)
			{
				isDuplicate = true;
				return root;
			}

			/* Otherwise allocate memory for new node */
			product newnode = k;
			isDuplicate = false;
			/* If root's key is greater, make root as right child of newnode and copy 
			the left child of root to newnode */
			if (root.ID > k.ID)
			{
				newnode.right = root;
				newnode.left = root.left;
				root.left = null;
			}
			else /* If root's key is smaller, make root as left child of newnode 
		     and copy the right child of root to newnode */
			{
				newnode.left = root;
				newnode.right = root.right;
				root.right = null;
			}

			return newnode; /* newnode becomes new root */
		}

		/*A method for inorder traversal*/
		public void inOrder()
		{
			inOrder(root);
		}

		/*Helper method for inOrder*/
		private void inOrder(product root)
		{
			if (root != null)
			{
				inOrder(root.left);
				Console.WriteLine("ID: " + root.ID + "\nNAME: " + root.NAME +
				   "\nBRAND: " + root.BRAND + "\nPRICE ($): " + root.COST + "\n");
				inOrder(root.right);
			}
		}

		/*Helper method for search*/
		/* The search function for Splay tree. 
		 * Note that this function returns the
		 * new root of Splay Tree. If key is
		 * present in tree then, it is moved to root.
		 */
		private product search(product root, int key)
		{
			return findNode(key);
		}
		/*A helper method for search method*/
		private product findNode(int id)
		{
			product PrevNode = null;
			product t = root;
			while (t != null)
			{
				PrevNode = t;
				if (id > t.ID)
					t = t.right;
				else if (id < t.ID)
					t = t.left;
				else if (id == t.ID)
				{
					splay(t, t.ID);
					return t;
				}

			}
			if (PrevNode != null)
			{
				splay(PrevNode, PrevNode.ID);
				return null;
			}
			return null;
		}
		/*Helper method for delete method*/
		/*The delete function for Splay tree. Note that this function
		  returns the new root of Splay Tree after removing the key*/
		private product delete(product root, int key)
		{
			product temp;
			if (root == null)
				return null;

			/* Splay the given key*/
			root = splay(root, key);

			/* If key is not present, then return root*/
			if (key != root.ID)
				return root;

			/* If key is present If left child of root does not exist make root->right as root */
			if (root.left == null)
			{
				temp = root;
				root = root.right;
			}
			/* Else if left child exits */
			else
			{
				temp = root;
				/*Note: Since key == root->key,
				so after Splay(key, root->lchild),
				the tree we get will have no right child tree
				and maximum node in left subtree will get splayed*/
				// New root
				root = splay(root.left, key);

				// Make right child of previous root  as
				// new root's right child
				root.right = temp.right;
			}
			// return root of the new Splay Tree
			return root;
		}

		public void insert(product obj)
        {
			root = insert(root, obj);
		}

        public void delete(int id)
        {
			root = delete(root, id);
		}

        public product search(int id)
        {
			return search(root, id);
		}

		/*Splay Tree node to array*/
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

	class Splay
    {
        static void Main(string[] args)
        {
			SplayTree_Class splay = new SplayTree_Class();
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
							splay.insert(p);
						}
						if (tokens.Length == 2)
						{
							product p = new product(int.Parse(tokens[0]), tokens[1], null, null);
							splay.insert(p);
						}
						if (tokens.Length == 3)
						{
							product p = new product(int.Parse(tokens[0]), tokens[1], tokens[2], null);
							splay.insert(p);
						}
						if (tokens.Length == 4)
						{
							product p = new product(int.Parse(tokens[0]), tokens[1], tokens[2], tokens[3]);
							splay.insert(p);
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
						splay.insert(cs);
						if (SplayTree_Class.isDuplicate == false)
						{
							Console.WriteLine("\n------Inserted Successfully-------");
						}
						else
						{
							Console.WriteLine("\n``````````````````````````````````````");
							Console.WriteLine("ID already registered, Try another.");
						}
						break;

					/*To delete record by id*/
					case 2:
						Console.WriteLine("``````````````````````````````````````");
						Console.Write("Enter ID to Delete >> ");
						int idDel = Convert.ToInt32(Console.ReadLine());

						/*Firstly to search*/
						product temp = splay.search(idDel);
						if (temp != null)
						{
							Console.WriteLine("ID: " + temp.ID + "\nNAME: " + temp.NAME +
							"\nBRAND: " + temp.BRAND + "\nPRICE ($): " + temp.COST + "\n");
							Console.WriteLine("\nConfirm to delete [yes(1)]");
							int conf = Convert.ToInt32(Console.ReadLine());
							if (conf == 1)
							{
								Console.WriteLine("Item Deleted!");
								splay.delete(idDel);
							}
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

						temp = splay.search(idSrch);
						if (temp != null)
						{
							Console.WriteLine("ID: " + temp.ID + "\nNAME: " + temp.NAME +
							"\nBRAND: " + temp.BRAND + "\nPRICE ($): " + temp.COST + "\n");
						}
						else
						{
							Console.WriteLine("Item not present");
						}
						break;

					/*to retrieve all the records*/
					case 4:
						Console.WriteLine("_____________________________________");
						int total = splay.size(splay.root);
						Console.WriteLine("\tTotal Records: " + total);
						Console.WriteLine("``````````````````````````````````````");
						splay.inOrder();

						break;

					/*to save records in file and exit*/
					case 5:
						/*First to retrieve records from bst*/
						splay.nToA(splay.root);

						/*Secondly, to shuffle and store in file*/
						try
						{
							// Write file using StreamWriter  
							using (StreamWriter writer = new StreamWriter(fileName))
							{
								//Collections.shuffle(BST_Class.nodesToArray);
								var shuffled = SplayTree_Class.nodesToArray.OrderBy(x => Guid.NewGuid()).ToList();
								for (int i = 0; i < shuffled.Count; i++)
								{
									temp = SplayTree_Class.nodesToArray[i];
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
						int tot = splay.size(splay.root);
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
